'use client';

import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { redirect } from 'next/navigation';

function Index() {
  const { clientId } = useSelectedClient();
  redirect(RouteNames.Notes(clientId!));
}

export default Index;
