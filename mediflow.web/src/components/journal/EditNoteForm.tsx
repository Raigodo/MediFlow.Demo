import { useForm } from 'react-hook-form';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { z } from 'zod';
import { writeNoteSchema } from '@/lib/schemas/post/writeNoteSchema';
import { zodResolver } from '@hookform/resolvers/zod';
import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { useWriteNote } from '@/lib/fetching/api/hooks/notes/useWriteNote';
import { useRemoveNote } from '@/lib/fetching/api/hooks/notes/useRemoveNote';
import { useState } from 'react';
import { Label } from '@/components/ui/label';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { Button } from '@/components/ui/button';
import { CircleAlertIcon } from 'lucide-react';
import { Textarea } from '@/components/ui/textarea';
import MutateButton from '@/components/common/MutateButton';
import { useToast } from '@/hooks/use-toast';
import PreventNavigation from '@/setup/PreventNavigation';
import deepEqual from '@/lib/util/deepEqual';
import { setError } from '@/lib/utils';

function EditNoteForm({
  note,
  onNoteMutated,
}: {
  note: NoteEntity;
  onNoteMutated: () => void;
}) {
  const defaultValues: z.infer<typeof writeNoteSchema> = {
    isImportant: note.isFlagged,
    noteContent: note.content,
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(writeNoteSchema),
    defaultValues,
  });

  const { mutateAsync: saveNoteMutation } = useWriteNote();
  const { mutateAsync: deleteNoteMutation } = useRemoveNote();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isRemoving, setIsRemoving] = useState(false);
  const { toast } = useToast();
  const formValues = form.watch();

  async function onSubmit(values: typeof defaultValues) {
    setIsSubmitting(true);
    const response = await saveNoteMutation({
      ...values,
      clientId: note.clientId,
    }).finally(() => setIsSubmitting(false));
    if (response.ok) {
      toast({
        title: `Žurnāla ieraksts ${note.id ? 'atjaunināts' : 'izveidots'}.`,
      });
      onNoteMutated();
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('couldnt delete note');
      toast({
        variant: 'destructive',
        title: 'Neizdevās izdzēst žurnāla ierakstu',
      });
    }
  }

  async function onDelete() {
    setIsRemoving(true);
    const response = await deleteNoteMutation({
      clientId: note.clientId,
    }).finally(() => setIsRemoving(false));
    if (response.ok) {
      toast({
        title: 'Žurnāla ieraksts tika veiksmīgi dzēsts',
      });
      onNoteMutated();
      return;
    }
    console.error('couldnt delete note');
    toast({
      variant: 'destructive',
      title: 'Neizdevās izdzēst žurnāla ierakstu',
    });
  }

  return (
    <PreventNavigation
      shouldPrevent={!(isSubmitting || deepEqual(defaultValues, formValues))}
    >
      <Form {...form}>
        <form
          onSubmit={form.handleSubmit(onSubmit)}
          className="gap-4 grid w-[800px] min-w-fit"
        >
          <div className="flex justify-between gap-x-10">
            <div className="gap-1 grid">
              <Label className="col-start-1 row-start-1 text-xl">
                Skatīt ierakstu
              </Label>
              <Label className="col-start-1 row-start-2 font-normal text-base">
                Skatīt šīsdienas žurnāla ierakstu
              </Label>
            </div>
            <FormField
              control={form.control}
              name="isImportant"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <TooltipProvider>
                      <Tooltip>
                        <TooltipTrigger
                          className="justify-self-end col-start-2 row-span-2"
                          asChild
                        >
                          <Button
                            size={'icon'}
                            type={'button'}
                            variant={'ghost'}
                            disabled={form.formState.isSubmitting}
                            onClick={() => {
                              field.onChange(!field.value);
                            }}
                            className={`rounded-md hover:bg-muted/50 ${
                              formValues.isImportant
                                ? 'text-destructive hover:text-destructive'
                                : 'text-secondary hover:text-foreground/50'
                            }`}
                          >
                            <CircleAlertIcon />
                          </Button>
                        </TooltipTrigger>
                        <TooltipContent className="text-foreground -translate-y-1.5">
                          <p>
                            {formValues.isImportant
                              ? 'atzīmēt ierakstu kā ne tik svarīgu'
                              : 'atzīmēt ierakstu kā svarīgu'}
                          </p>
                        </TooltipContent>
                      </Tooltip>
                    </TooltipProvider>
                  </FormControl>
                </FormItem>
              )}
            />
          </div>
          <div className="flex flex-col gap-4 w-[800px] min-w-fit">
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
                  {note.createdOn.toLocaleString('lv-LV', {
                    dateStyle: 'medium',
                  })}
                </span>
              </div>
            </div>
            <FormField
              control={form.control}
              name="noteContent"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Ieraksta saturs</FormLabel>
                  <FormControl>
                    <Textarea
                      placeholder="izskatās ka ieraksts bija atstāts tukšs"
                      className="shadow-lg px-3 py-2 border rounded-md h-[250px] min-h-32 text-lg resize-none"
                      {...field}
                      disabled={form.formState.isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <div>
              <div className="flex justify-end items-end gap-4 *:shadow-mg *:w-32 *:h-10 select-none">
                {note.id && (
                  <MutateButton
                    type="button"
                    loading={isRemoving}
                    disabled={form.formState.isSubmitting}
                    variant="destructive"
                    onClick={onDelete}
                  >
                    Dzēst
                  </MutateButton>
                )}
                <MutateButton
                  type="submit"
                  loading={isSubmitting}
                  disabled={form.formState.isSubmitting}
                >
                  Saglabāt
                </MutateButton>
              </div>
            </div>
          </div>
        </form>
      </Form>
    </PreventNavigation>
  );
}

export default EditNoteForm;
