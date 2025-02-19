'use client';

import { Tabs, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Toaster } from '@/components/ui/toaster';
import { RouteNames } from '@/lib/RouteNames';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { redirect, usePathname, useRouter } from 'next/navigation';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  const pathName = usePathname();
  const { push } = useRouter();
  const { lifecycle } = useUserSession();
  if (lifecycle.ok) {
    redirect(RouteNames.Home);
  }

  return (
    <>
      <div className="flex size-full">
        <div className="flex bg-secondary grow">
          <h1 className="mt-12 ml-16 font-bold text-4xl">MediFlow</h1>
        </div>
        <div className="w-2/6 h-screen shrink-0">
          <div className="bg-background px-8 py-6 h-full overflow-y-auto scrollbar-thin">
            <Tabs
              className="py-4"
              value={pathName}
              onValueChange={(value) => push(value)}
            >
              <TabsList className="flex gap-2 grid-cols-3 bg-transparent w-full *:grow">
                <TabsTrigger
                  value={RouteNames.Login}
                  className={
                    'bg-muted data-[state=active]:grow-[2] data-[state=active]:bg-primary data-[state=active]:text-foreground'
                  }
                >
                  Ieiet
                </TabsTrigger>
                <TabsTrigger
                  value={RouteNames.Join}
                  className={
                    'bg-muted data-[state=active]:grow-[2] data-[state=active]:bg-primary data-[state=active]:text-foreground'
                  }
                >
                  Pievienoties
                </TabsTrigger>
                <TabsTrigger
                  value={RouteNames.Register}
                  className={
                    'bg-muted data-[state=active]:grow-[2] data-[state=active]:bg-primary data-[state=active]:text-foreground'
                  }
                >
                  Regitrēties
                </TabsTrigger>
              </TabsList>
            </Tabs>
            {children}
          </div>
        </div>
      </div>
      <Toaster />
    </>
  );
}

export default Layout;
