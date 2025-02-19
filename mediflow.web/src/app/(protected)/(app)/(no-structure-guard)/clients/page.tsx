'use client';

import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { redirect } from 'next/navigation';

function Index() {
  const { clientId } = useSelectedClient();
  if (clientId) {
    redirect(RouteNames.Client(clientId));
  }
  return <>no client selected</>;
}

export default Index;
