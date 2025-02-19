'use client';

import ClientNotFound from '@/components/client/placeholders/ClientNotFound';
import ClientOverviewLoading from '@/components/client/placeholders/ClientOverviewLoading';
import { useClient } from '@/lib/fetching/api/hooks/clients/useClient';
import { useParams, useRouter } from 'next/navigation';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import EditClientForm from '@/components/client/EditClientForm';

function Index() {
  const { clientId } = useParams();
  const { data, isSuccess, isError, isLoading } = useClient(clientId as string);
  const { resetSelection } = useSelectedClient();
  const { push } = useRouter();

  const navigateToOverview = () => {
    push(RouteNames.Client(clientId as string));
  };

  const handleClientRemoval = () => {
    resetSelection();
    push(RouteNames.ClientsIndex);
  };

  return (
    <>
      {isLoading && <ClientOverviewLoading />}
      {!isLoading && isError && <ClientNotFound />}
      {!isLoading && isSuccess && !data.body && <ClientNotFound />}
      {!isLoading && isSuccess && data.body && (
        <EditClientForm
          client={data.body}
          onMutated={navigateToOverview}
          onDiscard={navigateToOverview}
          onRemoved={handleClientRemoval}
        />
      )}
    </>
  );
}

export default Index;
