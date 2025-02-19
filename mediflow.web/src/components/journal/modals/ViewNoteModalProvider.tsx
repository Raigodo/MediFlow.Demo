import ModalWrapper, { ModalWrapperItem } from '@/setup/ModalWrapper';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { useSelectedNote } from '@/setup/contexts/journal/SelectedNoteContext';
import { useNote } from '@/lib/fetching/api/hooks/notes/useNote';
import { Textarea } from '@/components/ui/textarea';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { JournalModalKeys } from '@/lib/modal-keys/JournalModalKeys';
import { Button } from '@/components/ui/button';
import { CircleAlertIcon, PencilIcon } from 'lucide-react';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useRouter } from 'next/navigation';
import { RouteNames } from '@/lib/RouteNames';

const ViewNoteModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={JournalModalKeys.ViewNoteModal}
      modalComponent={RadonlyNoteModal}
    />
  );
};

export default ViewNoteModalProvider;

const RadonlyNoteModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const { noteId } = useSelectedNote();
  const { clientId } = useSelectedClient();
  const { employeeRoleScope: employeeRole } = useSelectedEmployeeRoleScope();
  const { resetSelection } = useSelectedNote();

  const handleClose = () => {
    resetSelection();
    onClose();
  };

  if (!clientId || !employeeRole || !noteId) {
    handleClose();
    return;
  }

  return (
    <StartLoading
      isOpen={isOpen}
      onClose={onClose}
      clientId={clientId}
      noteId={noteId}
    />
  );
};

const StartLoading = ({
  isOpen,
  onClose,
  clientId,
  noteId,
}: {
  isOpen: boolean;
  onClose: () => void;
  clientId: string;
  noteId: string;
}) => {
  const { data, isSuccess, isError, isLoading } = useNote({
    clientId,
    noteId,
  });
  return (
    <Dialog
      open={isOpen}
      onOpenChange={(value) => value !== isOpen && !value && onClose()}
    >
      <DialogContent className="grid grid-rows-[auto_1fr] w-[calc(max(400px,min(700px,80vw)))] max-w-none h-fit max-h-[500px] select-text">
        {(isLoading || (!isLoading && isError)) && (
          <DialogHeader>
            <div className="flex gap-12">
              <div className="gap-1.5 grid w-fit">
                <DialogTitle>Ielādē ierakstu</DialogTitle>
                <DialogDescription>
                  Notiek ieraksta ielādēšana
                </DialogDescription>
              </div>
            </div>
          </DialogHeader>
        )}
        {isLoading && <>Loading...</>}
        {!isLoading && isError && <>Error...</>}
        {!isLoading && isSuccess && !data.body && <>Error...</>}
        {!isLoading && isSuccess && data.body && (
          <RenderModalContent
            note={data.body}
            clientId={clientId}
            onClose={onClose}
          />
        )}
      </DialogContent>
    </Dialog>
  );
};

const RenderModalContent = ({
  note,
  clientId,
  onClose,
}: {
  note: NoteEntity;
  clientId: string;
  onClose: () => void;
}) => {
  const { push } = useRouter();
  const handleViewNoteClick = () => {
    push(RouteNames.NoteOverview({ noteId: note.id, clientId }));
    onClose();
  };
  return (
    <>
      <DialogHeader>
        <div className="flex gap-12">
          <div className="gap-1.5 grid w-fit">
            <DialogTitle>Skatīt ierakstu</DialogTitle>
            <DialogDescription>Ieraksta kopsavilkums</DialogDescription>
          </div>
          <TooltipProvider>
            <Tooltip defaultOpen={false}>
              <TooltipTrigger asChild>
                <CircleAlertIcon
                  size={20}
                  className={`${note.isFlagged && 'text-destructive'}`}
                />
              </TooltipTrigger>
              <TooltipContent side="right">
                {note.isFlagged ? (
                  <p>Ieraksts ir atzīmēts kā svarīgs</p>
                ) : (
                  <p>Ieraksts nav atzīmēts kā svarīgs</p>
                )}
              </TooltipContent>
            </Tooltip>
          </TooltipProvider>
        </div>
      </DialogHeader>
      <div className="gap-4 grid grid-rows-[auto_1fr]">
        <div>
          <div>
            Izveidoja: {note.creator.name} {note.creator.surname}
          </div>
          <div>Amats: {note.creator.role}</div>
          <div>
            Datums:{' '}
            {note.createdOn.toLocaleString('lv-LV', { dateStyle: 'medium' })}
          </div>
        </div>
        <Textarea
          placeholder="izskatās ka ieraksts bija atstāts tukšs"
          className="shadow-lg px-3 py-2 border rounded-md focus-visible:ring-0 h-[200px] min-h-32 text-lg resize-none"
          value={note.content}
          onClick={handleViewNoteClick}
          readOnly
        />
      </div>
      <div className="right-10 -bottom-2.5 absolute bg-background rounded-md size-11">
        <Button
          size={'icon'}
          className="size-full"
          onClick={handleViewNoteClick}
        >
          <PencilIcon />
        </Button>
      </div>
    </>
  );
};
