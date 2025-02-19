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
import { Button } from '@/components/ui/button';
import { loginSchema } from '@/lib/schemas/post/loginSchema';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import MutateButton from '@/components/common/MutateButton';

function Index() {
  const defaultValues: z.infer<typeof loginSchema> = {
    email: '',
    password: '',
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(loginSchema),
    defaultValues,
  });

  const { logIn } = useUserSession();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await logIn(values);
    if (response.ok) {
      toast({
        title: 'Sveicināti MediFlow!',
      });
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('login failed');
      toast({
        variant: 'destructive',
        title: 'Neizdevās pieslēgties',
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
          <div className="flex justify-end mt-6">
            <MutateButton type="submit" loading={form.formState.isSubmitting}>
              Pieslēgties
            </MutateButton>
          </div>
        </form>
      </Form>
    </div>
  );
}

export default Index;
