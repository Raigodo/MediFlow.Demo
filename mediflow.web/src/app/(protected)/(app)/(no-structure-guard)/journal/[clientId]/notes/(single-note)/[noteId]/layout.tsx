'use client';

import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { useSelectedNote } from '@/setup/contexts/journal/SelectedNoteContext';
import { redirect, useParams } from 'next/navigation';
import { FC, ReactNode, useEffect } from 'react';

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  const { noteId } = useParams();
  const { clientId } = useSelectedClient();
  const { noteId: selectedNoteId, selectNote } = useSelectedNote();

  if (!clientId) throw Error('no client selected in note view');
  if (typeof noteId !== 'string') {
    console.error('extract note id from route params failed');
    redirect(RouteNames.Notes(clientId));
  }

  useEffect(() => {
    if (!selectedNoteId && typeof noteId === 'string') {
      selectNote({ noteId: noteId as string });
    }
  }, []);

  if (!selectedNoteId) {
    return null;
  }
  return <>{children}</>;
};

export default Layout;
