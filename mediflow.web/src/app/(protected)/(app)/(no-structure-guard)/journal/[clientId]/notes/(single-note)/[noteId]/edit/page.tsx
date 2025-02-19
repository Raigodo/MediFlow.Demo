'use client';

import EditNoteForm from '@/components/journal/EditNoteForm';
import LoadingNoteOverview from '@/components/journal/placeholders/LoadingNoteOverview';
import LoadingNoteOverviewError from '@/components/journal/placeholders/LoadingNoteOverviewError';
import { useTodaysNote } from '@/lib/fetching/api/hooks/notes/useTodaysNote';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useRouter } from 'next/navigation';

function Index() {
  const { push } = useRouter();
  const { clientId } = useSelectedClient();
  if (!clientId) throw Error('open todays note but no client selected');
  const goBack = () => {
    push(RouteNames.Notes(clientId));
  };
  const { employeeRoleScope } = useSelectedEmployeeRoleScope();
  const { data, isLoading, isSuccess, isError } = useTodaysNote({
    clientId,
    employeeRole: employeeRoleScope,
  });
  return (
    <>
      {isLoading && <LoadingNoteOverview />}
      {!isLoading && isError && <LoadingNoteOverviewError />}
      {!isLoading && isSuccess && !data.body && <LoadingNoteOverviewError />}
      {!isLoading && isSuccess && data.body && (
        <EditNoteForm note={data.body} onNoteMutated={goBack} />
      )}
    </>
  );
}

export default Index;
