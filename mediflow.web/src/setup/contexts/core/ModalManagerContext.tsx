'use client';

import { AdminModalKeys } from '@/lib/modal-keys/AdminModalKeys';
import { AppModalKeys } from '@/lib/modal-keys/AppModalKeys';
import { JournalModalKeys } from '@/lib/modal-keys/JournalModalKeys';
import { ManagerModalKeys } from '@/lib/modal-keys/ManagerModalKeys';
import { createContext, ReactNode, useContext, useState } from 'react';

export type ModalKeys =
  | AdminModalKeys
  | ManagerModalKeys
  | JournalModalKeys
  | AppModalKeys;

type ModalManagerContext = {
  isOpen: boolean;
  currentModalKey: ModalKeys | null;
  open: (params: { modalKey: ModalKeys }) => void;
  close: () => void;
};

const Context = createContext<ModalManagerContext | undefined>(undefined);

const ModalManagerContextProvider = ({ children }: { children: ReactNode }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [currentModalKey, setCurrentModalKey] =
    useState<ModalManagerContext['currentModalKey']>(null);
  const close = () => {
    setIsOpen(false);
    setCurrentModalKey(null);
  };
  const open: ModalManagerContext['open'] = ({ modalKey }) => {
    setIsOpen(true);
    setCurrentModalKey(modalKey);
  };
  return (
    <Context.Provider value={{ isOpen, currentModalKey, open, close }}>
      {children}
    </Context.Provider>
  );
};

export default ModalManagerContextProvider;

export const useModalManager = () => {
  const context = useContext(Context);
  if (!context) throw Error('no modal manager context');
  return context;
};
