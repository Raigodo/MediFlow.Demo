'use client';

import CreateStructureModalProvider from '@/components/manager/modals/CreateStructureModalProvider';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { FC, ReactNode } from 'react';

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  const { hasStructure, isManager } = useUserSession();
  if (hasStructure) return <>{children}</>;
  return <>{isManager && <CreateStructureModalProvider />}</>;
};

export default Layout;
