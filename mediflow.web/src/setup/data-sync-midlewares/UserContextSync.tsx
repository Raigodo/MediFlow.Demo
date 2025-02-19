import {
  useParams,
  usePathname,
  useRouter,
  useSearchParams,
} from 'next/navigation';
import { FC, ReactNode, useEffect, useState } from 'react';
import { useSelectedUser } from '../contexts/app/SelectedUserContext';

interface UserContextSyncProps {
  children: ReactNode;
  onSyncFailed?: () => void;
  clientIdPartIndex: number;
}

const UserContextSync: FC<UserContextSyncProps> = ({
  children,
  onSyncFailed,
  clientIdPartIndex: userIdPartIndex,
}) => {
  const { userId } = useParams();
  const { userId: selectedUserId, selectUser } = useSelectedUser();
  const { push } = useRouter();
  const pathname = usePathname();
  const queryString = useSearchParams().toString();

  if (typeof userId !== 'string') {
    onSyncFailed?.();
    throw Error('extract userId from route params failed');
  }

  const [isFirstRender, setIsFirstRender] = useState(true);
  const syncRequired = userId !== selectedUserId;

  useEffect(() => {
    setIsFirstRender(false);
  }, []);

  useEffect(() => {
    if (syncRequired) {
      if (isFirstRender && !selectedUserId) {
        selectUser({ userId });
      } else if (selectedUserId) {
        const parts = pathname.split('/').filter(Boolean);
        if (parts.length <= userIdPartIndex) {
          onSyncFailed?.();
          throw Error('failed to sync route with selected user');
        }
        parts[userIdPartIndex] = selectedUserId;
        push(`/${parts.join('/')}?${queryString}`);
      }
    }
  }, [selectedUserId, userId, isFirstRender]);

  if (syncRequired && isFirstRender) return null;
  return <>{children}</>;
};

export default UserContextSync;
