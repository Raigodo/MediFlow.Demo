'use client';

import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { createContext, ReactNode, useContext, useState } from 'react';

type SelectedNoteContext = {
  noteId: string | null;
  selectNote: (params: { noteId: string }) => void;
  resetSelection: () => void;
};

const Context = createContext<SelectedNoteContext | undefined>(undefined);

const SelectedNoteContextProvider = ({ children }: { children: ReactNode }) => {
  const [noteId, setNoteId] = useState<SelectedNoteContext['noteId']>(null);

  const selectNote: SelectedNoteContext['selectNote'] = ({ noteId }) => {
    setNoteId(noteId);
  };
  const resetSelection = () => {
    setNoteId(null);
  };

  return (
    <Context.Provider value={{ noteId, selectNote, resetSelection }}>
      {children}
    </Context.Provider>
  );
};

export default SelectedNoteContextProvider;

export const useSelectedNote = () => {
  const context = useContext(Context);
  if (!context) throw Error('no client context');
  return context;
};

export function checkIfIsTodaysNote(note: NoteEntity | null) {
  if (!note) return false;
  const today = new Date();
  if (
    note &&
    !isNaN(note.createdOn as any) &&
    note.createdOn &&
    note.createdOn.getUTCFullYear() === today.getUTCFullYear() &&
    note.createdOn.getUTCMonth() === today.getUTCMonth() &&
    note.createdOn.getUTCDate() === today.getUTCDate()
  )
    return true;
  return false;
}
