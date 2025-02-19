'use client';

import ClientContextSync from '@/setup/data-sync-midlewares/ClientContextSync';
import RoleScopeContextSync from '@/setup/data-sync-midlewares/RoleScopeContextSync';
import { RouteNames } from '@/lib/RouteNames';
import { FC, ReactNode } from 'react';
import { redirect } from 'next/navigation';
import { ErrorBoundary } from 'next/dist/client/components/error-boundary';

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  return (
    <ErrorBoundary errorComponent={() => redirect(RouteNames.JournalIndex)}>
      <ClientContextSync
        clientIdPartIndex={1}
        onSyncFailed={() => redirect(RouteNames.JournalIndex)}
      >
        <RoleScopeContextSync>
          <>{children}</>
        </RoleScopeContextSync>
      </ClientContextSync>
    </ErrorBoundary>
  );
};

export default Layout;
