'use client';

import { RouteNames } from '@/lib/RouteNames';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { redirect, usePathname } from 'next/navigation';

function Index() {
  const { lifecycle, isAdmin } = useUserSession();
  const pathname = usePathname();
  const getRoute = () => {
    if (lifecycle.isBooting) return pathname;
    if (lifecycle.ok) {
      if (isAdmin) return RouteNames.AdminHome;
      return RouteNames.Home;
    }
    return RouteNames.Login;
  };
  redirect(getRoute());
}

export default Index;
