'use client';

import CreateStructureModalProvider from '@/components/manager/modals/CreateStructureModalProvider';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { FC, ReactNode } from 'react';

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  const { hasStructure } = useUserSession();
  if (hasStructure) return <>{children}</>;
  return null;
};

export default Layout;
