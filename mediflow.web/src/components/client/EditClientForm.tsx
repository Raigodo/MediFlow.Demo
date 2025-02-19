import { useToast } from '@/hooks/use-toast';
import { ClientEntity } from '@/lib/domain/entities/ClientEntity';
import { useRemoveClient } from '@/lib/fetching/api/hooks/clients/useRemoveClient';
import { useUpdateClient } from '@/lib/fetching/api/hooks/clients/useUpdateClient';
import { updateClientSchema } from '@/lib/schemas/post/updateClientSchema';
import { setError } from '@/lib/utils';
import PreventNavigation from '@/setup/PreventNavigation';
import { zodResolver } from '@hookform/resolvers/zod';
import { Fragment, useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { z } from 'zod';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../ui/form';
import deepEqual from '@/lib/util/deepEqual';
import { Input } from '../ui/input';
import { DatePicker } from '../common/DatePicker';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '../ui/select';
import { InvalidityGroups } from '@/lib/domain/values/InvalidityGroups';
import { InvalidityFlags } from '@/lib/domain/values/InvalidityTypes';
import { Button } from '../ui/button';
import { CircleMinusIcon, PlusIcon, Trash2Icon, XIcon } from 'lucide-react';
import { nanoid } from 'nanoid';
import MutateButton from '../common/MutateButton';

const EditClientForm = ({
  client,
  onRemoved,
  onMutated,
  onDiscard,
}: {
  client: ClientEntity;
  onRemoved: () => void;
  onMutated: () => void;
  onDiscard: () => void;
}) => {
  const defaultValues: z.infer<typeof updateClientSchema> = {
    clientId: client.id,
    clientName: client.name,
    clientSurname: client.surname,
    clientBirthDate: client.birthDate,
    clientPersonalCode: client.personalCode,
    clientLanguage: client.language,
    clientReligion: client.religion,
    clientInvalidityGroup: client.invalidtiyGroup,
    clientInvalidityFlag: client.invalidityFlag,
    clientInvalidityExpiresOn: client.invalidityExpiresOn ?? undefined,
    clientContacts: client.contacts.map((contact) => ({
      clientContactId: contact.id,
      clientContactTitle: contact.title,
      clientContactPhoneNumber: contact.phoneNumber,
    })),
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(updateClientSchema),
    defaultValues,
  });
  const {
    fields: contactFields,
    append: appendContact,
    remove: removeContact,
  } = useFieldArray({
    name: 'clientContacts',
    control: form.control,
  });

  const { mutateAsync: updateClientMutation } = useUpdateClient();
  const { mutateAsync: deleteClientMutation } = useRemoveClient();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isRemoving, setIsRemoving] = useState(false);
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    setIsSubmitting(true);
    const response = await updateClientMutation(values).finally(() =>
      setIsSubmitting(false),
    );
    if (response.ok) {
      onMutated();
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('couldnt update client');
      toast({
        variant: 'destructive',
        title: 'Neizdevās atjaunināt klienta',
      });
    }
  }

  async function onDelete() {
    setIsRemoving(true);
    const response = await deleteClientMutation({
      clientId: client.id,
    }).finally(() => setIsRemoving(false));
    if (response.ok) {
      toast({ title: 'Klients veiksmīgi dzēsts' });
      onRemoved();
      return;
    }
    console.error('couldnt remove client');
    toast({
      variant: 'destructive',
      title: 'Neizdevās izdzēst klientu.',
    });
  }

  return (
    <PreventNavigation
      shouldPrevent={
        !(isSubmitting || deepEqual(defaultValues, form.getValues()))
      }
    >
      <Form {...form}>
        <form className="flex" onSubmit={form.handleSubmit(onSubmit)}>
          <div className="w-[550px]">
            <div className="pb-4 border-t border-r rounded-tr-sm w-full">
              <div className="flex flex-col gap-1 px-4 pt-4 border-l-4 border-l-primary">
                <h1 className="flex gap-2">
                  <FormField
                    control={form.control}
                    name="clientName"
                    render={({ field }) => (
                      <FormItem className="w-full h-14">
                        <FormControl>
                          <Input
                            placeholder="vārds"
                            className="rounded-sm size-full text-2xl md:text-2xl"
                            {...field}
                            disabled={form.formState.isSubmitting}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                  <FormField
                    control={form.control}
                    name="clientSurname"
                    render={({ field }) => (
                      <FormItem className="w-full h-14">
                        <FormControl>
                          <Input
                            placeholder="uzvārds"
                            className="rounded-sm size-full text-2xl md:text-2xl"
                            {...field}
                            disabled={form.formState.isSubmitting}
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                </h1>
                <FormField
                  control={form.control}
                  name="clientId"
                  render={({ field }) => (
                    <FormItem>
                      <FormControl>
                        <Input
                          placeholder="vārds"
                          className="px-2 py-1 rounded-sm w-36 h-full text-md md:text-md"
                          disabled
                          {...field}
                        />
                      </FormControl>
                    </FormItem>
                  )}
                />
              </div>
              <div className="flex *:flex flex-col *:justify-between gap-1.5 pt-6 pr-6 pl-14 *:border-b">
                <div className="flex justify-between items-center py-0.5">
                  Tika pievienots:
                  <DatePicker
                    disabled
                    date={new Date(client.joinedOn.toString())}
                    className="px-2 py-1 rounded-sm w-44 h-8"
                  />
                </div>
                <FormField
                  control={form.control}
                  name="clientBirthDate"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Dzimšanas datums:</FormLabel>
                      <FormMessage />
                      <FormControl>
                        <DatePicker
                          {...field}
                          date={field.value}
                          onSelected={field.onChange}
                          className="px-2 py-1 rounded-sm w-44 h-8"
                          disabled={form.formState.isSubmitting}
                        />
                      </FormControl>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="clientPersonalCode"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Personas kods:</FormLabel>
                      <FormControl>
                        <Input
                          className="px-2 py-1 rounded-sm w-44 h-8"
                          {...field}
                          disabled={form.formState.isSubmitting}
                        />
                      </FormControl>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="clientLanguage"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Valoda:</FormLabel>
                      <FormControl>
                        <Input
                          className="px-2 py-1 rounded-sm w-44 h-8"
                          {...field}
                          disabled={form.formState.isSubmitting}
                        />
                      </FormControl>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="clientReligion"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Religija:</FormLabel>
                      <FormControl>
                        <Input
                          className="px-2 py-1 rounded-sm w-44 h-8"
                          {...field}
                          disabled={form.formState.isSubmitting}
                        />
                      </FormControl>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="clientInvalidityGroup"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Invaliditātes grupa:</FormLabel>
                      <FormControl>
                        <Select
                          {...field}
                          onValueChange={field.onChange}
                          disabled={form.formState.isSubmitting}
                        >
                          <SelectTrigger className="px-2 py-1 rounded-sm w-44 h-8">
                            <SelectValue placeholder="izvēlēties" />
                          </SelectTrigger>
                          <SelectContent>
                            {Object.values(InvalidityGroups).map((group) => (
                              <SelectItem value={group} key={group}>
                                {group}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </FormControl>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="clientInvalidityFlag"
                  render={({ field }) => (
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Invaliditātes tips:</FormLabel>
                      <FormControl>
                        <Select
                          {...field}
                          onValueChange={field.onChange}
                          disabled={
                            form.getValues().clientInvalidityGroup ==
                              InvalidityGroups.None ||
                            form.formState.isSubmitting
                          }
                        >
                          <SelectTrigger className="px-2 py-1 rounded-sm w-44 h-8">
                            <SelectValue placeholder="izvēlēties" />
                          </SelectTrigger>
                          <SelectContent>
                            {Object.values(InvalidityFlags).map((group) => (
                              <SelectItem value={group} key={group}>
                                {group}
                              </SelectItem>
                            ))}
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
                    <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                      <FormLabel>Invaliditātes derīguma termiņš:</FormLabel>
                      <FormControl>
                        <DatePicker
                          {...field}
                          date={field.value}
                          onSelected={field.onChange}
                          className="px-2 py-1 rounded-sm w-44 h-8"
                          disabled={
                            form.getValues().clientInvalidityGroup ==
                              InvalidityGroups.None ||
                            form.formState.isSubmitting
                          }
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
            </div>
            <div className="mt-7 ml-12 pb-4 border-t border-r rounded-tr-sm w-[calc(100%-3rem)]">
              <div className="flex justify-between items-center px-2 py-1.5 border-l-4 border-l-primary">
                <h3 className="font-medium">Kontakti</h3>
                <Button
                  type="button"
                  variant="outline"
                  className="rounded-sm size-6"
                  disabled={form.formState.isSubmitting}
                  onClick={() => {
                    appendContact({
                      clientContactTitle: '',
                      clientContactPhoneNumber: '',
                    });
                  }}
                >
                  <PlusIcon />
                </Button>
              </div>
              <div className="flex flex-col gap-3 mt-2 ml-6">
                {contactFields.length > 0 &&
                  contactFields.map((contact, index) => (
                    <Fragment key={nanoid()}>
                      <div
                        className="items-center gap-x-2 gap-y-0 grid grid-cols-[auto_1fr] mr-6"
                        key={nanoid()}
                      >
                        <Button
                          type="button"
                          variant="ghost"
                          onClick={() => removeContact(index)}
                          className="col-start-1 row-start-1 p-0 rounded-full size-6"
                          disabled={form.formState.isSubmitting}
                        >
                          <CircleMinusIcon />
                        </Button>
                        <FormField
                          control={form.control}
                          name={`clientContacts.${index}.clientContactTitle`}
                          render={({ field }) => (
                            <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-1 py-0.5 border-b grow">
                              <FormLabel>Kontakta nosaukums:</FormLabel>
                              <FormControl>
                                <Input
                                  autoFocus={true}
                                  placeholder="kontakta nosaukums"
                                  {...field}
                                  className="rounded-sm w-44 h-8"
                                  disabled={form.formState.isSubmitting}
                                />
                              </FormControl>
                            </FormItem>
                          )}
                        />
                        <FormField
                          control={form.control}
                          name={`clientContacts.${index}.clientContactPhoneNumber`}
                          render={({ field }) => (
                            <FormItem className="flex justify-between items-center space-y-0 col-start-2 row-start-2 py-0.5 border-b grow">
                              <FormLabel>Telefona numurs:</FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="kontakta tālruņa numurs"
                                  {...field}
                                  className="m-0 rounded-sm w-44 h-8"
                                  disabled={form.formState.isSubmitting}
                                />
                              </FormControl>
                            </FormItem>
                          )}
                        />
                      </div>
                    </Fragment>
                  ))}
                {form.getValues().clientContacts.length <= 0 && (
                  <div>nav kontaktu</div>
                )}
              </div>
            </div>
          </div>

          <div className="flex flex-col justify-between ml-4 w-24 h-[350px]">
            <div className="flex flex-col gap-2 *:w-12">
              <Button
                size={'icon'}
                type="button"
                variant="ghost"
                onClick={onDiscard}
                disabled={form.formState.isSubmitting}
              >
                <XIcon />
              </Button>
              <MutateButton
                loading={isRemoving}
                disabled={form.formState.isSubmitting}
                size={'icon'}
                variant="ghost"
                onClick={onDelete}
              >
                <Trash2Icon />
              </MutateButton>
            </div>
            <div className="flex flex-col gap-2">
              <MutateButton
                loading={isSubmitting}
                disabled={form.formState.isSubmitting}
                type="submit"
              >
                Saglabāt
              </MutateButton>
              <div>
                <Button
                  type="button"
                  disabled={form.formState.isSubmitting}
                  variant="secondary"
                  onClick={onDiscard}
                  className="w-full h-8"
                >
                  Atmest
                </Button>
              </div>
            </div>
          </div>
        </form>
      </Form>
    </PreventNavigation>
  );
};

export default EditClientForm;
