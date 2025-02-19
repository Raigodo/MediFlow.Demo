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
import { createInvitationSchema } from '@/lib/schemas/post/createInvitationSchema';
import { zodResolver } from '@hookform/resolvers/zod';
import { useCreateInvitation } from '@/lib/fetching/api/hooks/invitations/useCreateInvitation';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { ManagerModalKeys } from '@/lib/modal-keys/ManagerModalKeys';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import MutateButton from '@/components/common/MutateButton';

const CreateInvitationModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={ManagerModalKeys.CreateInvitation}
      modalComponent={CreateInvitationModal}
    />
  );
};

export default CreateInvitationModalProvider;

const CreateInvitationModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const defaultValues: z.infer<typeof createInvitationSchema> = {
    employeeRole: EmployeeRoles.Nurse,
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(createInvitationSchema),
    defaultValues: defaultValues,
  });

  const { mutateAsync } = useCreateInvitation();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await mutateAsync(values);
    if (response.ok) {
      toast({
        title: 'Ielūgums veiksmīgi izveidots.',
      });
      onClose();
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('couldnt create invitation');
      toast({
        variant: 'destructive',
        title: 'Neizdevās izveidot ielūgumu',
      });
    }
  }

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(open) => open !== isOpen && !open && onClose()}
    >
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Izveidot ielūgumu</DialogTitle>
          <DialogDescription>
            Darbinieka ielūguma izveide uz kādu amatu
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="min-h-24">
            <FormField
              control={form.control}
              name="employeeRole"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Ieņemamais amats</FormLabel>
                  <FormControl>
                    <Select
                      onValueChange={field.onChange}
                      {...field}
                      disabled={form.formState.isSubmitting}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="izvēlēties ieņemamo amatu" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectGroup>
                          <SelectLabel>Iespējamie amati</SelectLabel>
                          {Object.values(EmployeeRoles).map((role) => (
                            <SelectItem value={role} key={role}>
                              {role}
                            </SelectItem>
                          ))}
                        </SelectGroup>
                      </SelectContent>
                    </Select>
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
