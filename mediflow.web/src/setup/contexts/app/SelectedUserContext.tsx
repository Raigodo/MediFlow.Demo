'use client';

import { createContext, ReactNode, useContext, useState } from 'react';

type SelectedClientContext = {
  userId: string | null;
  selectUser: (params: { userId: string }) => void;
  resetSelection: () => void;
};

const Context = createContext<SelectedClientContext | undefined>(undefined);

const SelectedUserContextProvider = ({ children }: { children: ReactNode }) => {
  const [userId, setUserId] = useState<SelectedClientContext['userId']>(null);
  const selectUser: SelectedClientContext['selectUser'] = ({ userId }) => {
    setUserId(userId);
  };
  const resetSelection = () => {
    setUserId(null);
  };
  return (
    <Context.Provider value={{ userId, selectUser, resetSelection }}>
      {children}
    </Context.Provider>
  );
};

export default SelectedUserContextProvider;

export const useSelectedUser = () => {
  const context = useContext(Context);
  if (!context) throw Error('no client context');
  return context;
};
