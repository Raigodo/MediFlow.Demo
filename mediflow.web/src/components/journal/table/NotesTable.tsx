import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { useNotes } from '@/lib/fetching/api/hooks/notes/useNotes';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useNoteFilter } from '@/setup/contexts/journal/NoteFilterContext';
import NoEmployeeRoleSelected from '@/components/layout/placeholders/NoEmployeeRoleSelected';
import LoadingNotes from '../placeholders/LoadingNotes';
import NoNotes from '../placeholders/NoNotes';
import { useSelectedNote } from '@/setup/contexts/journal/SelectedNoteContext';
import { JournalModalKeys } from '@/lib/modal-keys/JournalModalKeys';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';
import { CalendarIcon, CircleAlertIcon, UserRoundIcon } from 'lucide-react';
import { useRouter } from 'next/navigation';
import { RouteNames } from '@/lib/RouteNames';
import LoadingNotesError from '../placeholders/LoadingNotesError';
import { ScrollArea } from '@/components/ui/scroll-area';

const NotesTable = NotesTableGuarded;
export default NotesTable;

function NotesTableGuarded() {
  const { clientId } = useSelectedClient();
  const { employeeRoleScope, isCurrentRole } = useSelectedEmployeeRoleScope();
  return (
    <ScrollArea className="shadow-sm border-b grow">
      <Table>
        <TableHeader className="bg-muted">
          <TableRow
            className={`hover:bg-transparent [&_svg]:pointer-events-none [&_svg]:size-4 ${
              (!clientId || !employeeRoleScope) && 'opacity-50'
            }`}
          >
            <TableHead className="w-[130px]">
              <div className="flex items-center gap-2">
                <CalendarIcon /> Datums
              </div>
            </TableHead>
            <TableHead className="w-[200px]">
              <div className="flex items-center gap-2">
                <UserRoundIcon /> Autors
              </div>
            </TableHead>
            <TableHead>Saturs</TableHead>
            <TableHead className="w-16">
              <div className="flex justify-center items-center">
                <CircleAlertIcon />
              </div>
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {!clientId && (
            <TableRow>
              <TableCell colSpan={4}>Nav izvēlēts neviens klients</TableCell>
            </TableRow>
          )}
          {clientId && !employeeRoleScope && (
            <TableRow>
              <TableCell colSpan={4}>
                <NoEmployeeRoleSelected />
              </TableCell>
            </TableRow>
          )}
          {clientId && employeeRoleScope && (
            <NotesTableRowsLoading
              clientId={clientId}
              employeeRole={employeeRoleScope}
              canWrite={isCurrentRole}
            />
          )}
        </TableBody>
      </Table>
    </ScrollArea>
  );
}

const NotesTableRowsLoading = ({
  clientId,
  employeeRole,
  canWrite,
}: {
  clientId: string;
  employeeRole: EmployeeRoles;
  canWrite: boolean;
}) => {
  const { dateFrom, dateTo, employeeId, flagState } = useNoteFilter();
  const { data, isSuccess, isError, isLoading } = useNotes({
    clientId,
    employeeRole,
    dateFrom,
    dateTo,
    employeeId,
    flagState,
  });

  return (
    <>
      {isLoading && (
        <TableRow>
          <TableCell colSpan={4}>
            <LoadingNotes />
          </TableCell>
        </TableRow>
      )}
      {!isLoading && isError && (
        <TableRow>
          <TableCell colSpan={4}>
            <LoadingNotesError />
          </TableCell>
        </TableRow>
      )}
      {!isLoading && isSuccess && (
        <RenderTableRows notes={data.body} canWrite={canWrite} />
      )}
    </>
  );
};

const RenderTableRows = ({
  notes,
  canWrite,
}: {
  notes: NoteEntity[];
  canWrite: boolean;
}) => {
  const { selectNote } = useSelectedNote();
  const { push } = useRouter();
  const { open } = useModalManager();
  const { hasAnyConstraint } = useNoteFilter();
  const { clientId } = useSelectedClient();
  if (!clientId) throw Error('rendering journal table without selected client');
  const onNoteClicked = (note: NoteEntity) => {
    selectNote({ noteId: note.id });
    const modalKey = JournalModalKeys.ViewNoteModal;
    open({ modalKey });
  };
  const onCreateNoteClicked = () => {
    push(RouteNames.TodaysNote(clientId));
  };
  console.log(hasAnyConstraint);

  return (
    <>
      {notes.length > 0 &&
        notes.map((note) => (
          <TableRow
            key={note.id}
            className="hover:cursor-pointer"
            onClick={() => onNoteClicked(note)}
          >
            <TableCell className="font-medium">
              {note.createdOn.toLocaleString('lv-LV', { dateStyle: 'short' })}
            </TableCell>
            <TableCell>
              {note.creator.name} {note.creator.surname}
            </TableCell>
            <TableCell>{note.content}</TableCell>
            <TableCell>
              <CircleAlertIcon
                className={`mx-auto ${
                  note.isFlagged ? 'text-destructive' : 'text-secondary'
                }`}
                size={16}
              />
            </TableCell>
          </TableRow>
        ))}
      {notes.length <= 0 && !hasAnyConstraint && canWrite && (
        <TableRow>
          <TableCell
            colSpan={4}
            className="text-center hover:cursor-pointer"
            onClick={onCreateNoteClicked}
          >
            <p>Vēl nav neviena ieraksta. Izveidot?</p>
          </TableCell>
        </TableRow>
      )}
      {notes.length <= 0 && !hasAnyConstraint && !canWrite && (
        <TableRow>
          <TableCell colSpan={4}>
            <NoNotes />
          </TableCell>
        </TableRow>
      )}
      {notes.length <= 0 && hasAnyConstraint && (
        <TableRow>
          <TableCell colSpan={4}>
            <NoNotes />
          </TableCell>
        </TableRow>
      )}
    </>
  );
};
