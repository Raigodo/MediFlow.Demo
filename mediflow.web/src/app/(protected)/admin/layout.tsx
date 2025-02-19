'use client';

import Toolbar from '@/components/layout/toolbar/Toolbar';
import Topbar from '@/components/layout/topbar/Topbar';
import { RouteNames } from '@/lib/RouteNames';
import SelectedStructureContextProvider from '@/setup/contexts/admin/SelectedStructureContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { redirect } from 'next/navigation';
import { ReactNode } from 'react';
import ManagersSidelist from '@/components/admin/ManagersSidelist';
import { Toaster } from '@/components/ui/toaster';
import CreateUserModalProvider from '@/components/admin/modals/CreateUserModalProvider';
import SelectedUserContextProvider from '@/setup/contexts/app/SelectedUserContext';

function Layout({ children }: { children: ReactNode }) {
  const { isAdmin } = useUserSession();
  if (!isAdmin) {
    redirect(RouteNames.Home);
  }
  return (
    <SelectedUserContextProvider>
      <SelectedStructureContextProvider>
        <div
          className={`grid grid-rows-[64px_1fr] transition-all h-screen grid-cols-[64px_300px_1fr]`}
        >
          <div className="row-span-2 border-r border-border">
            <Toolbar />
          </div>

          <div className="col-span-2">
            <div className="mx-3 pr-7 border-b border-border size-full">
              <Topbar />
            </div>
          </div>

          <div className="bg-secondary/20 pb-2 pl-2">
            <div className="flex flex-col gap-1.5 pt-2 border-r border-border h-full">
              <ManagersSidelist />
            </div>
          </div>

          <div className="bg-secondary/5 size-full overflow-y-auto overscroll-none scrollbar scrollbar-track-transparent">
            {children}
          </div>
        </div>
        <Toaster />
        <CreateUserModalProvider />
      </SelectedStructureContextProvider>
    </SelectedUserContextProvider>
  );
}

export default Layout;
