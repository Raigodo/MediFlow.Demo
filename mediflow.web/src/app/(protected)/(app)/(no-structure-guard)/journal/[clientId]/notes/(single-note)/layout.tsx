'use client';

import { Button } from '@/components/ui/button';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { Undo2Icon } from 'lucide-react';
import Link from 'next/link';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  const { clientId } = useSelectedClient();
  if (!clientId) throw Error('no client selected');

  return (
    <>
      <div className="top-4 left-2 absolute">
        <Link href={RouteNames.Notes(clientId)}>
          <Button size={'icon'} variant={'ghost'}>
            <Undo2Icon />
          </Button>
        </Link>
      </div>
      <div className="flex px-2 py-12">
        <div className="flex grow shrink-1"></div>
        <div className="select-text">{children}</div>
        <div className="flex grow-[2] shrink-1" />
      </div>
    </>
  );
}

export default Layout;
