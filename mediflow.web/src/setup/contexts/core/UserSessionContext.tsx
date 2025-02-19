'use client';

import {
  createContext,
  ReactNode,
  RefObject,
  useContext,
  useRef,
  useState,
} from 'react';
import { useServerApi } from './ServerApiContext';
import { useEffect } from 'react';
import { SessionEntity } from '@/lib/domain/entities/SessionEntity';
import { UserRoles } from '@/lib/domain/values/UserRoles';
import { useQueryClient } from '@tanstack/react-query';

type requests = ReturnType<typeof useServerApi>['session'];

type LogIn = (reqeust: {
  email: string;
  password: string;
}) => ReturnType<requests['logIn']>;
type Join = (reqeust: {
  email: string;
  password: string;
  invitationToken: string;
}) => ReturnType<requests['join']>;
type Register = (request: {
  email: string;
  password: string;
  userName: string;
  userSurname: string;
  invitationToken: string;
}) => ReturnType<requests['register']>;
type Refresh = () => ReturnType<requests['refresh']>;
type Alter = (request: {
  structureId: string;
}) => ReturnType<requests['alter']>;
type LogOut = () => void;

type SessionShortcuts = {
  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;
  hasStructure: boolean;
};
type SessionLifecycle = {
  ok: boolean;
  refreshing: boolean;
  isBooting?: boolean | undefined;
};
type UserSessionContext = SessionShortcuts &
  Partial<SessionEntity> & {
    sessionRef: RefObject<SessionEntity>;
    lifecycleRef: { current: SessionLifecycle };
    lifecycle: SessionLifecycle;
    logIn: LogIn;
    join: Join;
    register: Register;
    refresh: Refresh;
    alter: Alter;
    logOut: LogOut;
  };

const Context = createContext<UserSessionContext | undefined>(undefined);
const PERSISTED_SESSION_KEY = 'mediflow.session';

const UserSessionContextProvider = ({ children }: { children: ReactNode }) => {
  const queryClient = useQueryClient();
  const { session: requests } = useServerApi();
  const sessionRef = useRef<UserSessionContext['sessionRef']['current']>(
    loadSession(),
  );
  const lifecycleRef = useRef<UserSessionContext['lifecycleRef']['current']>({
    ok: false,
    refreshing: false,
    isBooting: undefined,
  });
  const [session, setSession] = useState(sessionRef.current);
  const [lifecycle, setLifecycle] = useState(lifecycleRef.current);

  const updateSession = (state: typeof sessionRef.current) => {
    sessionRef.current = state;
    setSession(sessionRef.current);
  };
  const updateLifecycle = (state: typeof lifecycleRef.current) => {
    lifecycleRef.current = state;
    setLifecycle(lifecycleRef.current);
  };

  const sessionShorcuts: SessionShortcuts = {
    isAdmin: session?.sessionData.userRole === UserRoles.Admin,
    isManager: session?.sessionData.userRole === UserRoles.Manager,
    isEmployee: session?.sessionData.userRole === UserRoles.Employee,
    hasStructure: !!session?.sessionData.structureId,
  };

  function loadSession() {
    const serializedSessionData = localStorage.getItem(PERSISTED_SESSION_KEY);
    if (serializedSessionData) return JSON.parse(serializedSessionData);
    return null;
  }
  const persistSession = () => {
    localStorage.setItem(PERSISTED_SESSION_KEY, JSON.stringify(session));
  };
  const removePersistedSession = () => {
    localStorage.removeItem(PERSISTED_SESSION_KEY);
  };
  useEffect(() => {
    if (session?.accessToken) persistSession();
  }, [session?.accessToken]);

  useEffect(() => {
    async function bootSession() {
      updateLifecycle({
        ...lifecycleRef.current,
        isBooting: true,
      });
      await refresh();
      updateLifecycle({
        ...lifecycleRef.current,
        isBooting: false,
      });
    }
    if (session?.accessToken) bootSession();
    else {
      updateLifecycle({
        ...lifecycleRef.current,
        isBooting: false,
      });
    }
  }, []);

  const logIn: LogIn = async (request) => {
    try {
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: true,
      });
      const res = await requests.logIn(request);
      if (res.ok && res.body) {
        updateSession(res.body);
        queryClient.removeQueries();
      }
      updateLifecycle({
        ...lifecycleRef.current,
        ok: res.ok,
        refreshing: false,
      });
      return res;
    } catch (e) {
      updateLifecycle({
        ...lifecycleRef.current,
        ok: false,
        refreshing: false,
      });
      console.error('login failed');
      throw e;
    }
  };
  const join: Join = async (request) => {
    try {
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: true,
      });
      const res = await requests.join(request);
      if (res.ok && res.body) {
        updateSession(res.body);
        queryClient.removeQueries();
      }
      updateLifecycle({
        ...lifecycleRef.current,
        ok: res.ok,
        refreshing: false,
      });
      return res;
    } catch (e) {
      updateLifecycle({
        ...lifecycleRef.current,
        ok: false,
        refreshing: false,
      });
      console.error('join failed');
      throw e;
    }
  };
  const logOut: LogOut = () => {
    updateSession(null);
    updateLifecycle({
      ...lifecycleRef.current,
      ok: false,
      refreshing: false,
    });
    queryClient.removeQueries();
    removePersistedSession();
  };
  const register: Register = async (request) => {
    try {
      const res = await requests.register(request);
      if (!res.ok) {
        return res;
      }
      const res2 = await logIn(request);
      if (!res2.ok) {
        throw Error('user signed-up but could not sign-in');
      }
      return res;
    } catch (e) {
      console.error('register failed');
      throw e;
    }
  };
  const _refresh: Refresh = async () => {
    try {
      if (!session) {
        throw Error('can not refresh bcause no access token found');
      }
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: true,
      });
      const refreshResponse = await requests.refresh({
        accessToken: session.accessToken,
      });
      if (refreshResponse.ok) {
        const newSession = refreshResponse.body?.sessionData;
        const prewSession = session.sessionData;
        if (
          newSession?.structureId != prewSession.structureId ||
          newSession?.userId != prewSession.userId ||
          newSession?.employeeId != prewSession.employeeId
        ) {
          queryClient.removeQueries();
        }
        updateSession(refreshResponse.body);
      } else {
        removePersistedSession();
      }

      updateLifecycle({
        ...lifecycleRef.current,
        ok: refreshResponse.ok,
      });
      return refreshResponse;
    } catch (e) {
      console.error('refresh failed');
      throw e;
    } finally {
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: false,
      });
    }
  };
  const refreshingPromiseRef = useRef<ReturnType<Refresh> | null>(null);
  const refresh: Refresh = async () => {
    if (!refreshingPromiseRef.current) {
      refreshingPromiseRef.current = new Promise<Awaited<ReturnType<Refresh>>>(
        async (resolve, reject) => {
          try {
            resolve(await _refresh()!);
          } catch (error) {
            reject(error);
          } finally {
            refreshingPromiseRef.current = null;
          }
        },
      );
    }
    return refreshingPromiseRef.current;
  };
  const alter: Alter = async (request) => {
    try {
      if (!session) throw Error('can not refresh bcause no access token found');
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: true,
      });
      const res = await requests.alter({
        accessToken: session.accessToken,
        ...request,
      });
      if (res.ok) {
        updateSession(res.body);
        queryClient.removeQueries();
      }
      return res;
    } catch (e) {
      console.error('alter failed');
      throw e;
    } finally {
      updateLifecycle({
        ...lifecycleRef.current,
        refreshing: false,
      });
    }
  };

  return (
    <Context.Provider
      value={{
        ...session,
        lifecycle,
        sessionRef,
        lifecycleRef,
        ...sessionShorcuts,
        logIn,
        join,
        register,
        logOut,
        refresh: _refresh,
        alter,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export default UserSessionContextProvider;

export const useUserSession = () => {
  const context = useContext(Context);
  if (!context) throw Error('no user session context');
  return context;
};
