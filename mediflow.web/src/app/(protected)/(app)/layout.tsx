'use client';

import ClientsSidelist from '@/components/client/ClientsSidelist';
import CreateClientModalProvider from '@/components/client/modals/CreateClientModalProvider';
import Toolbar from '@/components/layout/toolbar/Toolbar';
import Topbar from '@/components/layout/topbar/Topbar';
import CreateStructureModalProvider from '@/components/manager/modals/CreateStructureModalProvider';
import { Toaster } from '@/components/ui/toaster';
import { RouteNames } from '@/lib/RouteNames';
import SelectedClientContextProvider from '@/setup/contexts/app/SelectedClientContext';
import SelectedEmployeeRoleScopeContextProvider from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { redirect } from 'next/navigation';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  const { isAdmin, isManager } = useUserSession();
  if (isAdmin) {
    redirect(RouteNames.AdminHome);
  }
  return (
    <SelectedEmployeeRoleScopeContextProvider>
      <SelectedClientContextProvider>
        <div
          className={`grid grid-rows-[64px_1fr] transition-all h-screen grid-cols-[64px_256px_1fr]`}
        >
          <div className="row-span-2 border-r border-border">
            <Toolbar />
          </div>

          <div className="col-span-2">
            <div className="mx-3 pr-7 border-b border-border size-full">
              <Topbar />
            </div>
          </div>

          <div className="bg-secondary/10 py-2 pl-2">
            <div className="gap-1.5 border-r border-border">
              <ClientsSidelist className="h-[calc(100vh-64px-16px)]" />
            </div>
          </div>

          <div className="relative bg-secondary/5 scrollbar-thumb-input/60 size-full overflow-y-auto overscroll-none scrollbar scrollbar-track-background">
            <>{children}</>
          </div>
        </div>
        {isManager && <CreateStructureModalProvider />}
        <Toaster />
        <CreateClientModalProvider />
      </SelectedClientContextProvider>
    </SelectedEmployeeRoleScopeContextProvider>
  );
}

export default Layout;
