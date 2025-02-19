'use client';

import { createContext, ReactNode, useContext, useMemo } from 'react';
import { useUserSession } from './UserSessionContext';
import { useServerApi } from './ServerApiContext';
import { PendingHttpResponse } from '@/lib/fetching/api/base/makeHttpRequest';
import { ValueOf } from 'next/dist/shared/lib/constants';

type IsOptional<T> = ValueOf<T> extends never ? true : false;
type OptionalObjectIfOptioanlFields<T> = {
  [K in keyof T]: IsOptional<T[K]>;
}[keyof T] extends true
  ? T | void
  : T;

type WithoutAccessToken<T> = T extends (req: infer R) => infer P
  ? (request: OptionalObjectIfOptioanlFields<Omit<R, 'accessToken'>>) => P
  : T;
type RequestsWithoutAccessToken<T> = {
  [K in keyof T]: WithoutAccessToken<T[K]>;
};
type RequestRegistryWithoutAccessToken<T> = {
  [K in keyof T]: RequestsWithoutAccessToken<T[K]>;
};
type RequestsContext = RequestRegistryWithoutAccessToken<
  Omit<ReturnType<typeof useServerApi>, 'session'>
>;

const Context = createContext<RequestsContext | undefined>(undefined);

const RequestsContextProvider = ({ children }: { children: ReactNode }) => {
  const apiRequests = useServerApi();
  const { refresh, lifecycleRef, sessionRef } = useUserSession();

  const createAccessTokenAppenderProxy = (actionSet: any) => {
    return new Proxy(actionSet, {
      get(target, prop, receiver) {
        return async (...args: any[]) => {
          const accessToken = sessionRef.current?.accessToken;
          if (!accessToken) throw Error('There is no access token');
          if (
            args[0] &&
            typeof args[0] === 'object' &&
            !Array.isArray(args[0])
          ) {
            args[0] = { ...args[0], accessToken };
          } else if (!args[0]) {
            args[0] = { accessToken };
          } else {
            throw Error(
              'append access token proxy failed to append mutate arguments',
            );
          }
          const originalMethod = Reflect.get(target, prop, receiver);
          return originalMethod.apply(target, args);
        };
      },
    });
  };

  const createAutoSessionRefreshProxy = (actionSet: any) => {
    return new Proxy(actionSet, {
      get(target, prop, receiver) {
        return async (...args: any[]) => {
          const originalMethod = Reflect.get(target, prop, receiver);
          const respnse = await (originalMethod.apply(
            target,
            args,
          ) as PendingHttpResponse<any>);
          if (!respnse) throw Error('refresh proxy got empty response');
          if (respnse.status === 401) {
            const refreshResult = await refresh();
            if (refreshResult?.ok && lifecycleRef.current.ok) {
              return originalMethod.apply(
                target,
                args,
              ) as PendingHttpResponse<any>;
            }
          }
          return respnse;
        };
      },
    });
  };

  const createOfflineCheckerProxy = (actionSet: any) => {
    return new Proxy(actionSet, {
      get(target, prop, receiver) {
        return async (...args: any[]) => {
          const originalMethod = Reflect.get(target, prop, receiver);
          if (!navigator.onLine) {
            throw Error('User is currently offline');
          }
          const respnse = await (originalMethod.apply(
            target,
            args,
          ) as PendingHttpResponse<any>);
          return respnse;
        };
      },
    });
  };

  const createServerUnreachableProxy = (actionSet: any) => {
    return new Proxy(actionSet, {
      get(target, prop, receiver) {
        return async (...args: any[]) => {
          try {
            const originalMethod = Reflect.get(target, prop, receiver);
            const respnse = await (originalMethod.apply(
              target,
              args,
            ) as PendingHttpResponse<any>);
            return respnse;
          } catch {
            throw Error('server can not be reached'); //mmost likely server is down
          }
        };
      },
    });
  };

  const requestsWithProxies = useMemo(
    () =>
      new Proxy(apiRequests as any, {
        get(target, prop: keyof RequestsContext) {
          const actionSet = target[prop];
          const serverUnreachableProxy =
            createServerUnreachableProxy(actionSet);
          const accessTokenProxy = createAccessTokenAppenderProxy(
            serverUnreachableProxy,
          );
          const refreshProxy = createAutoSessionRefreshProxy(accessTokenProxy);
          const offileProxy = createOfflineCheckerProxy(refreshProxy);
          return offileProxy;
        },
      }),
    [sessionRef],
  );

  return (
    <Context.Provider value={requestsWithProxies}>{children}</Context.Provider>
  );
};

export default RequestsContextProvider;

export const useRequests = () => {
  const context = useContext(Context);
  if (!context) throw Error('no server api context');
  return context;
};
