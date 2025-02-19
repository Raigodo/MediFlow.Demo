import { UserRoles } from '@/lib/domain/values/UserRoles';
import { createUserSchema } from '@/lib/schemas/post/createUserSchema';
import ModalWrapper, { ModalWrapperItem } from '@/setup/ModalWrapper';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { useCreateUser } from '@/lib/fetching/api/hooks/users/useCreateUser';
import { Input } from '@/components/ui/input';
import { AdminModalKeys } from '@/lib/modal-keys/AdminModalKeys';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import MutateButton from '@/components/common/MutateButton';

const CreateUserModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={AdminModalKeys.CreateUser}
      modalComponent={CreateUserModal}
    />
  );
};

export default CreateUserModalProvider;

const CreateUserModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const defaultValues: z.infer<typeof createUserSchema> = {
    userName: '',
    userSurname: '',
    email: '',
    password: '',
    userRole: UserRoles.Manager,
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(createUserSchema),
    defaultValues,
  });

  const { mutateAsync } = useCreateUser();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await mutateAsync(values);
    if (response.ok) {
      toast({
        title: 'Lietotājs veiksmīgi izveidots',
      });
      onClose();
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('couldnt create user');
      toast({
        variant: 'destructive',
        title: 'Neizdevās izveidot lietotāju',
      });
    }
  }

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(value) => value !== isOpen && !value && onClose()}
    >
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Izveidot lietotāju</DialogTitle>
          <DialogDescription>Izveidot jaunu lietotāju</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="flex flex-col gap-1.5"
          >
            <div className="gap-4 grid grid-cols-[1fr_1fr]">
              <FormField
                control={form.control}
                name="userName"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Vārds</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="vārds"
                        {...field}
                        disabled={form.formState.isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="userSurname"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Uzvārds</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="uzvārds"
                        {...field}
                        disabled={form.formState.isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Epasts</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="mansepasts@gmail.com"
                      {...field}
                      disabled={form.formState.isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Parole</FormLabel>
                  <FormControl>
                    <Input
                      type="password"
                      placeholder="mansepasts@gmail.com"
                      {...field}
                      disabled={form.formState.isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="userRole"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Lietotāja loma</FormLabel>
                  <FormControl>
                    <Select
                      onValueChange={field.onChange}
                      {...field}
                      disabled={form.formState.isSubmitting}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="izvēlēties lietotāja lokmu" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectGroup>
                          <SelectLabel>Lietotāja lomas</SelectLabel>
                          {Object.values(UserRoles).map((role) => (
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
            <div className="flex justify-center mt-8">
              <MutateButton
                type="submit"
                className="w-48"
                loading={form.formState.isSubmitting}
              >
                Izveidot
              </MutateButton>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
