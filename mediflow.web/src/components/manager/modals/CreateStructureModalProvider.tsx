import ModalWrapper, { ModalWrapperItem } from '@/setup/ModalWrapper';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { createStructureSchema } from '@/lib/schemas/post/createStructureSchema';
import { zodResolver } from '@hookform/resolvers/zod';
import { useCreateStructure } from '@/lib/fetching/api/hooks/structures/useCreateStructure';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { ManagerModalKeys } from '@/lib/modal-keys/ManagerModalKeys';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import MutateButton from '@/components/common/MutateButton';

const CreateStructureModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={ManagerModalKeys.CreateStructure}
      modalComponent={CreateStructureModal}
    />
  );
};

export default CreateStructureModalProvider;

const CreateStructureModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const defaultValues: z.infer<typeof createStructureSchema> = {
    structureName: '',
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(createStructureSchema),
    defaultValues,
  });

  const { alter } = useUserSession();
  const { mutateAsync } = useCreateStructure();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await mutateAsync(values);
    if (response.ok) {
      if (!response.body?.id) {
        toast({
          variant: 'destructive',
          title: 'Kautkas nogāja greizi',
        });
        throw Error('structure created but no structure id was provided');
      }
      toast({
        title: 'Struktūra veiksmīgi izveidota',
      });
      await alterSessionSctructureScope(response.body.id);
      return;
    }
    console.error('couldnt create structure');
    toast({
      variant: 'destructive',
      title: 'Neizdevās izveidot struktūrvienību',
    });
    setError(form, response.errors);
  }

  const alterSessionSctructureScope = async (structureId: string) => {
    const response = await alter({
      structureId,
    });
    if (response.ok) {
      onClose();
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('structure created but alter session failed');
      toast({
        variant: 'destructive',
        title: 'Sesijas atsvaidzināšan neidevās',
        description:
          'Jauna struktūrvienība tika izveidota, bet neizdevās atjaunināt tekošo sesiju',
      });
    }
  };

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(value) => value !== isOpen && !value && onClose()}
    >
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Izveidot struktūrvienību</DialogTitle>
          <DialogDescription>
            Izveidot jaunu struktūrvienību sistēmā
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="min-h-24">
            <FormField
              control={form.control}
              name="structureName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Struktūrvienības nosaukums</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="nosaukums"
                      {...field}
                      disabled={form.formState.isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <div className="flex justify-end mt-6">
              <MutateButton type="submit" loading={form.formState.isSubmitting}>
                Izveidot
              </MutateButton>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
