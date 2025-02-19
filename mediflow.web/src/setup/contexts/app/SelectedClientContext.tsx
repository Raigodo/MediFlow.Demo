'use client';

import { createContext, ReactNode, useContext, useState } from 'react';

type SelectedClientContext = {
  clientId: string | null;
  selectClient: (params: { clientId: string }) => void;
  resetSelection: () => void;
};

const Context = createContext<SelectedClientContext | undefined>(undefined);

const SelectedClientContextProvider = ({
  children,
}: {
  children: ReactNode;
}) => {
  const [clientId, setClientId] =
    useState<SelectedClientContext['clientId']>(null);
  const selectClient: SelectedClientContext['selectClient'] = ({
    clientId,
  }) => {
    setClientId(clientId);
  };
  const resetSelection = () => {
    setClientId(null);
  };
  return (
    <Context.Provider value={{ clientId, selectClient, resetSelection }}>
      {children}
    </Context.Provider>
  );
};

export default SelectedClientContextProvider;

export const useSelectedClient = () => {
  const context = useContext(Context);
  if (!context) throw Error('no client context');
  return context;
};
