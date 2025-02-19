'use client';

import LoadingNoteOverview from '@/components/journal/placeholders/LoadingNoteOverview';
import LoadingNoteOverviewError from '@/components/journal/placeholders/LoadingNoteOverviewError';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { useNote } from '@/lib/fetching/api/hooks/notes/useNote';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import {
  checkIfIsTodaysNote,
  useSelectedNote,
} from '@/setup/contexts/journal/SelectedNoteContext';
import { CircleAlertIcon } from 'lucide-react';
import { redirect } from 'next/navigation';

function Index() {
  const { clientId } = useSelectedClient();
  const { noteId } = useSelectedNote();
  if (!clientId) throw Error('open existing note but no client selected');
  if (!noteId) throw Error('open existing note but no note selected');
  const { data, isLoading, isSuccess, isError } = useNote({
    noteId,
    clientId,
  });
  const { isEmployee } = useUserSession();
  if (
    !isLoading &&
    isSuccess &&
    data.body &&
    checkIfIsTodaysNote(data.body) &&
    isEmployee
  ) {
    redirect(RouteNames.EditNote({ clientId, noteId: data.body.id }));
  }
  return (
    <>
      {isLoading && <LoadingNoteOverview />}
      {!isLoading && isError && <LoadingNoteOverviewError />}
      {!isLoading && isSuccess && !data.body && <LoadingNoteOverviewError />}
      {!isLoading && isSuccess && data.body && (
        <ViewReadonlyNoteOverview note={data.body} />
      )}
    </>
  );
}

export default Index;

const ViewReadonlyNoteOverview = ({ note }: { note: NoteEntity }) => {
  return (
    <div className="gap-4 grid w-[800px] min-w-fit">
      <div className="gap-x-10 gap-y-1 grid grid-cols-[auto_1fr] grid-rows-[auto_auto]">
        <Label className="col-start-1 row-start-1 text-xl">
          Skatīt ierakstu
        </Label>
        <Label className="col-start-1 row-start-2 font-normal text-base">
          Skatīt eksistējošo žurnāla ierakstu
        </Label>
        <TooltipProvider>
          <Tooltip>
            <TooltipTrigger
              className="justify-self-end col-start-2 row-span-2 mb-auto rounded-full"
              asChild
            >
              <div
                className={`p-1 ${
                  note.isFlagged ? 'text-destructive' : 'text-secondary'
                }`}
              >
                <CircleAlertIcon size={24} />
              </div>
            </TooltipTrigger>
            <TooltipContent className="-translate-x-1">
              <p>
                Ieraksts {note.isFlagged ? 'ir' : 'nav'} atzīmēts kā svarīgs
              </p>
            </TooltipContent>
          </Tooltip>
        </TooltipProvider>
      </div>
      <div className="gap-0.5 grid">
        <div>
          <Label>Izveidoja: </Label>
          <span>
            {note.creator.name} {note.creator.surname}
          </span>
        </div>
        <div>
          <Label>Amats: </Label>
          <span>{note.creator.role}</span>
        </div>
        <div>
          <Label>Datums: </Label>
          <span>
            {note.createdOn.toLocaleString('lv-LV', { dateStyle: 'medium' })}
          </span>
        </div>
      </div>
      <Textarea
        placeholder="izskatās ka ieraksts bija atstāts tukšs"
        className="shadow-lg px-3 py-2 border rounded-md focus-visible:ring-0 h-[250px] min-h-32 text-lg resize-none"
        value={note.content}
        readOnly
      />
    </div>
  );
};
