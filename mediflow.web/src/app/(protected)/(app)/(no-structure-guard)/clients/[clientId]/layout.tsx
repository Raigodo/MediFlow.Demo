'use client';

import ClientContextSync from '@/setup/data-sync-midlewares/ClientContextSync';
import { RouteNames } from '@/lib/RouteNames';
import { FC, ReactNode } from 'react';
import { ErrorBoundary } from 'next/dist/client/components/error-boundary';
import { redirect } from 'next/navigation';

interface layoutProps {
  children: ReactNode;
}

const layout: FC<layoutProps> = ({ children }) => {
  return (
    <ErrorBoundary errorComponent={() => redirect(RouteNames.ClientsIndex)}>
      <ClientContextSync
        clientIdPartIndex={1}
        onSyncFailed={() => redirect(RouteNames.ClientsIndex)}
      >
        <div className="flex px-2 py-12">
          <div className="flex grow shrink-1"></div>
          <div className="select-text">{children}</div>
          <div className="flex grow-[3] shrink-1" />
        </div>
      </ClientContextSync>
    </ErrorBoundary>
  );
};

export default layout;
