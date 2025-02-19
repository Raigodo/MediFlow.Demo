import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { InvalidityGroups } from '@/lib/domain/values/InvalidityGroups';
import { InvalidityFlags } from '@/lib/domain/values/InvalidityTypes';
import { createClientSchema } from '@/lib/schemas/post/createClientSchema';
import ModalWrapper, { ModalWrapperItem } from '@/setup/ModalWrapper';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { useCreateClient } from '@/lib/fetching/api/hooks/clients/useCreateClient';
import { AppModalKeys } from '@/lib/modal-keys/AppModalKeys';
import { DatePicker } from '@/components/common/DatePicker';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { setError } from '@/lib/utils';
import { useToast } from '@/hooks/use-toast';
import MutateButton from '@/components/common/MutateButton';

const CreateClientModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={AppModalKeys.CreateClient}
      modalComponent={CreateClientModal}
    />
  );
};

export default CreateClientModalProvider;

const CreateClientModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const defaultValues: z.infer<typeof createClientSchema> = {
    clientId: '',
    clientName: '',
    clientSurname: '',
    clientBirthDate: new Date(),
    clientPersonalCode: '',
    clientLanguage: '',
    clientReligion: '',
    clientInvalidityGroup: InvalidityGroups.None,
    clientInvalidityFlag: InvalidityFlags.None,
    clientInvalidityExpiresOn: undefined,
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(createClientSchema),
    defaultValues,
  });

  const { selectClient } = useSelectedClient();
  const { mutateAsync } = useCreateClient();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await mutateAsync({
      ...values,
    });
    if (response.ok) {
      onClose();
      response.body && selectClient({ clientId: response.body.id });
      toast({
        title: 'Klients pievienots.',
      });
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('couldnt add client');
      toast({
        variant: 'destructive',
        title: 'Neizdevās pievienot klientu',
      });
    }
  }

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(value) => value !== isOpen && !value && onClose()}
    >
      <DialogContent className="grid grid-rows-[auto_1fr] h-fit max-h-[80vh] overflow-hidden">
        <DialogHeader>
          <DialogTitle>Klienta pievienošana</DialogTitle>
          <DialogDescription>
            Jauna klienta pievienošana sistēmai
          </DialogDescription>
        </DialogHeader>
        <div className="overflow-auto scrollbar-thin">
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-4 p-1"
              tabIndex={-1}
            >
              <div className="gap-4 grid grid-cols-[1fr_1fr]">
                <FormField
                  control={form.control}
                  name="clientName"
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
                  name="clientSurname"
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
                name="clientId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Klienta Id</FormLabel>
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
              <FormField
                control={form.control}
                name="clientPersonalCode"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Personas kods</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="xxxxx-xxxxx"
                        {...field}
                        disabled={form.formState.isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <div className="gap-4 grid grid-cols-[1fr_1fr]">
                <FormField
                  control={form.control}
                  name="clientLanguage"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Valoda</FormLabel>
                      <FormControl>
                        <Input
                          placeholder="valoda"
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
                  name="clientBirthDate"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Dzimšanas datums</FormLabel>
                      <FormControl>
                        <DatePicker
                          date={field.value}
                          onSelected={field.onChange}
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
                name="clientReligion"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Religija</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="religija"
                        {...field}
                        disabled={form.formState.isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <div className="gap-4 grid grid-cols-[1fr_1fr]">
                <FormField
                  control={form.control}
                  name="clientInvalidityGroup"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Invalditātes grupa</FormLabel>
                      <FormControl>
                        <Select
                          onValueChange={field.onChange}
                          {...field}
                          disabled={form.formState.isSubmitting}
                        >
                          <SelectTrigger>
                            <SelectValue placeholder="izvēlēties" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectGroup>
                              <SelectLabel>Invaliditātes grupas</SelectLabel>
                              {Object.values(InvalidityGroups).map((item) => (
                                <SelectItem value={item} key={item}>
                                  {item}
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
                <FormField
                  control={form.control}
                  name="clientInvalidityExpiresOn"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Derīguma termiņš</FormLabel>
                      <FormControl>
                        <DatePicker
                          date={field.value}
                          disabled={
                            form.getValues().clientInvalidityGroup ===
                              InvalidityGroups.None ||
                            form.formState.isSubmitting
                          }
                          onSelected={field.onChange}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <FormField
                control={form.control}
                name="clientInvalidityFlag"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Pastāvīgā invaliditāte</FormLabel>
                    <FormControl>
                      <RadioGroup
                        {...field}
                        onValueChange={(value) => {
                          field.onChange(value);
                        }}
                        orientation="vertical"
                        className="flex gap-4"
                        disabled={
                          form.getValues().clientInvalidityGroup ===
                            InvalidityGroups.None || form.formState.isSubmitting
                        }
                      >
                        <div>
                          <RadioGroupItem
                            value={InvalidityFlags.Temporary}
                            id="invalidity-flag-radio-group-1"
                          />
                          <Label htmlFor="invalidity-flag-radio-group-1">
                            Nē
                          </Label>
                        </div>
                        <div>
                          <RadioGroupItem
                            value={InvalidityFlags.Persistant}
                            id="invalidity-flag-radio-group-2"
                          />
                          <Label htmlFor="invalidity-flag-radio-group-2">
                            Jā
                          </Label>
                        </div>
                      </RadioGroup>
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
                  Pievienot
                </MutateButton>
              </div>
            </form>
          </Form>
        </div>
      </DialogContent>
    </Dialog>
  );
};
