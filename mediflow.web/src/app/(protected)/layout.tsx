'use client';

import { RouteNames } from '@/lib/RouteNames';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { redirect } from 'next/navigation';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  const { lifecycle } = useUserSession();
  if (!lifecycle.ok) {
    redirect(RouteNames.Index);
  }
  return <>{children}</>;
}

export default Layout;
