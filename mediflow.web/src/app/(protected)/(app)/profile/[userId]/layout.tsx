'use client';

import SelectedUserContextProvider from '@/setup/contexts/app/SelectedUserContext';
import UserContextSync from '@/setup/data-sync-midlewares/UserContextSync';
import { FC, ReactNode } from 'react';

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  return (
    <SelectedUserContextProvider>
      <UserContextSync clientIdPartIndex={1}>{children}</UserContextSync>
    </SelectedUserContextProvider>
  );
};

export default Layout;
