'use client';

import CreateInvitationModalProvider from '@/components/manager/modals/CreateInvitationModalProvider';
import CreateStructureModalProvider from '@/components/manager/modals/CreateStructureModalProvider';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  return (
    <>
      {children}
      <CreateStructureModalProvider />
      <CreateInvitationModalProvider />
    </>
  );
}

export default Layout;
