'use client';

import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import NoteFilterContext from '@/setup/contexts/journal/NoteFilterContext';
import SelectedNoteContext from '@/setup/contexts/journal/SelectedNoteContext';
import { ReactNode } from 'react';

function Layout({ children }: { children: ReactNode }) {
  const { clientId } = useSelectedClient();
  if (!clientId) throw Error('no client selected in journal layout');
  return (
    <NoteFilterContext>
      <SelectedNoteContext>{children}</SelectedNoteContext>
    </NoteFilterContext>
  );
}

export default Layout;
