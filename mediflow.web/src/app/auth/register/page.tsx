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
import { registerSchema } from '@/lib/schemas/post/registerSchema';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { useToast } from '@/hooks/use-toast';
import { setError } from '@/lib/utils';
import { loginSchema } from '@/lib/schemas/post/loginSchema';
import MutateButton from '@/components/common/MutateButton';

function Index() {
  const defaultValues: z.infer<typeof registerSchema> = {
    userName: '',
    userSurname: '',
    email: '',
    password: '',
    invitationToken: '',
  };

  const form = useForm<typeof defaultValues>({
    resolver: zodResolver(registerSchema),
    defaultValues,
  });

  const { register, logIn } = useUserSession();
  const { toast } = useToast();

  async function onSubmit(values: typeof defaultValues) {
    const response = await register(values);
    if (response.ok) {
      const loginSuccessful = await loginAfterRegistration(values);
      if (loginSuccessful) {
        toast({
          title: 'Sveicināti MediFlow!',
        });
      } else {
        console.error('login after register failed');
        toast({
          variant: 'destructive',
          title: 'Neizdevās izveidot sesiju.',
          description:
            'Registrācija veiksmīgi pabeigta, bet neizdevās izveidot sesiju.',
        });
      }
      return;
    }
    if (response.errors) {
      setError(form, response.errors);
    } else {
      console.error('register failed');
      toast({
        variant: 'destructive',
        title: 'Registrāciija neizdevās',
      });
    }
  }

  const loginAfterRegistration = async (
    values: z.infer<typeof loginSchema>,
  ) => {
    const response = await logIn(values);
    if (response.ok) {
      return true;
    }
    return false;
  };

  return (
    <div className="flex flex-col grow">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <div className="flex gap-4 *:grow">
            <FormField
              control={form.control}
              name="userName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>UserName</FormLabel>
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
                  <FormLabel>userSurname</FormLabel>
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
                <FormLabel>epasts</FormLabel>
                <FormControl>
                  <Input
                    placeholder="epasts@gmail.com"
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
                    placeholder="parole"
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
            name="invitationToken"
            render={({ field }) => (
              <FormItem>
                <FormLabel>uzaicinājuma kods</FormLabel>
                <FormControl>
                  <Input
                    placeholder="xxxxx-xxxxxx-xxxxx"
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
              Registrēties
            </MutateButton>
          </div>
        </form>
      </Form>
    </div>
  );
}

export default Index;
