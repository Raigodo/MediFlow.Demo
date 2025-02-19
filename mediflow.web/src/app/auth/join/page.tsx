'use client';

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { joinSchema } from '@/lib/schemas/post/joinSchema';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import MutateButton from '@/components/common/MutateButton';

function Index() {
  const defaultValues: z.infer<typeof joinSchema> = {
    email: '',
    password: '',
    invitationToken: '',
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(joinSchema),
    defaultValues,
  });

  const { join } = useUserSession();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await join(values);
    if (response.ok) {
      toast({
        title: 'Svecināti MediFlow!',
      });
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('join failed');
      toast({
        variant: 'destructive',
        title: 'Neizdevās pievienoties',
        description: 'neizdevās izveidot sesiju',
      });
    }
  }

  return (
    <div className="flex flex-col grow">
      <Form {...form}>
        <form
          onSubmit={form.handleSubmit(onSubmit)}
          inert={form.formState.isSubmitting}
        >
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel>epasts</FormLabel>
                <FormControl>
                  <Input placeholder="epasts@gmail.com" {...field} />
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
                  <Input type="password" placeholder="parole" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="invitationToken"
            render={({ field }) => (
              <FormItem>
                <FormLabel>uzaicinājuma kods</FormLabel>
                <FormControl>
                  <Input placeholder="xxxxx-xxxxxx-xxxxx" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <div className="flex justify-end mt-6">
            <MutateButton type="submit" loading={form.formState.isSubmitting}>
              Pievienoties
            </MutateButton>
          </div>
        </form>
      </Form>
    </div>
  );
}

export default Index;
