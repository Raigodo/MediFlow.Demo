import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import {
  useParams,
  usePathname,
  useRouter,
  useSearchParams,
} from 'next/navigation';
import { FC, ReactNode, useEffect, useState } from 'react';

interface ClientContextSyncProps {
  children: ReactNode;
  onSyncFailed?: () => void;
  clientIdPartIndex: number;
}

const ClientContextSync: FC<ClientContextSyncProps> = ({
  children,
  onSyncFailed,
  clientIdPartIndex,
}) => {
  const { clientId } = useParams();
  const { clientId: selectedClientId, selectClient } = useSelectedClient();
  const { push } = useRouter();
  const pathname = usePathname();
  const queryString = useSearchParams().toString();

  if (typeof clientId !== 'string') {
    onSyncFailed?.();
    throw Error('extract clientId from route params failed');
  }

  const [isFirstRender, setIsFirstRender] = useState(true);
  const syncRequired = clientId !== selectedClientId;

  useEffect(() => {
    setIsFirstRender(false);
  }, []);

  useEffect(() => {
    if (syncRequired) {
      if (isFirstRender && !selectedClientId) {
        selectClient({ clientId });
      } else if (selectedClientId) {
        const parts = pathname.split('/').filter(Boolean);
        if (parts.length <= clientIdPartIndex) {
          onSyncFailed?.();
          throw Error('failed to sync route with selected client');
        }
        parts[clientIdPartIndex] = selectedClientId;
        push(`/${parts.join('/')}?${queryString}`);
      }
    }
  }, [selectedClientId, clientId, clientIdPartIndex, isFirstRender]);

  if (syncRequired && isFirstRender) return null;
  return <>{children}</>;
};

export default ClientContextSync;
