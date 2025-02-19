import { useRemoveStructure } from '@/lib/fetching/api/hooks/structures/useRemoveStructure';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { Button } from '@/components/ui/button';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { useCurrentStructure } from '@/lib/fetching/api/hooks/structures/useCurrentStructure';
import { useToast } from '@/hooks/use-toast';

function DeleteStructureButton() {
  const { mutateAsync } = useRemoveStructure();
  const { sessionData, refresh, logOut } = useUserSession();
  const { data, isError, isLoading, isSuccess } = useCurrentStructure();
  const { toast } = useToast();
  const handleDeleteStructure = async () => {
    if (!sessionData?.structureId)
      throw Error('can not delete structure without proper session');
    const result = await mutateAsync({
      structureId: sessionData.structureId,
    });
    if (!result.ok) {
      console.error('couldnt remove current structure');
      toast({
        variant: 'destructive',
        title: 'Neizdevās izdzēst tekošo struktūrvienību.',
      });
      return;
    }
    const refreshResult = await refresh();
    if (!refreshResult.ok) {
      console.error('couldnt refresh session after current structure deletion');
      toast({
        variant: 'destructive',
        title: 'Lietotāja sesija tika pārtraukta',
        description:
          'LIetotāja sesija nevarēja tikt atsvaidzināta, kā dēļ lietotājs tika izrakstīts.',
      });
      logOut();
      return;
    }
    toast({
      variant: 'destructive',
      title: 'Struktūrvienība veiksmīgi dzēsta',
    });
  };
  return (
    <>
      <AlertDialog>
        <AlertDialogTrigger asChild>
          <Button
            variant={'ghost'}
            className="hover:bg-destructive hover:text-destructive-foreground"
          >
            Dzēst struktūru
          </Button>
        </AlertDialogTrigger>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>
              Vai tiešām jūs vēlaties dzēst struktūrvienību?
            </AlertDialogTitle>
            <AlertDialogDescription>
              Šī darbība nevar tikt atcelta. pēc šīs darbības struktūrvienība ar
              nosaukumu "{' '}
              <span>
                {isLoading && <>ielādē struktūrvienības nosaukumu...</>}
                {!isLoading && isError && (
                  <>Nevarēja ielādēt struktūvenības nosukumu!</>
                )}
                {!isLoading && isSuccess && !data.body && (
                  <>Nevarēja ielādēt struktūrvienības nosaukumu!</>
                )}
                {!isLoading && isSuccess && data.body && <>{data.body.name}</>}
              </span>{' '}
              " tiks neatgriezeniski dzēsta.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Atcelt</AlertDialogCancel>
            <AlertDialogAction onClick={handleDeleteStructure}>
              Piekrist
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}

export default DeleteStructureButton;
