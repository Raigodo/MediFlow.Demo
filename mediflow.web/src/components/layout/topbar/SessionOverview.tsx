import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import CurrentUserAvatar from './CurrentUserAvatar';
import CurrentUserLabel from './CurrentUserLabel';
import StructureSelect from './StructureSelect';
import StructureLabel from './CurrentStructureLabel';
import UserDropdown from './UserDropdown';

function SessionOverview() {
  const { isAdmin, isManager } = useUserSession();
  return (
    <>
      <div className="flex items-center size-full">
        <div className="*:justify-self-end gap-0 grid px-4">
          <div className="text-xl">
            <CurrentUserLabel />
          </div>
          {!isAdmin && (
            <div className="py-0.5 text-end">
              {isManager ? <StructureSelect /> : <StructureLabel />}
            </div>
          )}
        </div>
        <UserDropdown>
          <CurrentUserAvatar />
        </UserDropdown>
      </div>
    </>
  );
}

export default SessionOverview;
