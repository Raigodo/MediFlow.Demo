import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import { FC, ReactNode, useEffect, useState } from 'react';

interface RoleScopeContextSyncProps {
  children: ReactNode;
}

const RoleScopeContextSync: FC<RoleScopeContextSyncProps> = ({ children }) => {
  const searchParams = useSearchParams();
  const { employeeRoleScope, selectEmployeeRoleScope } =
    useSelectedEmployeeRoleScope();

  const { push } = useRouter();
  const pathname = usePathname();
  const searchQuery = useSearchParams();
  const { isEmployee, sessionData } = useUserSession();

  const queryParam = searchParams.get('role');
  const queryParamRole =
    queryParam &&
    (Object.values(EmployeeRoles) as string[]).includes(queryParam!)
      ? (queryParam as EmployeeRoles)
      : undefined;

  const [isFirstRender, setIsFirstRender] = useState(true);
  useEffect(() => {
    setIsFirstRender(false);
  }, []);

  const syncRequired =
    (queryParamRole && (!isEmployee || queryParamRole !== employeeRoleScope)) ||
    employeeRoleScope !== sessionData?.employeeRole;

  useEffect(() => {
    if (!syncRequired) return;
    if (isFirstRender) {
      if (queryParamRole) {
        selectEmployeeRoleScope({
          employeeRoleScope: queryParamRole,
        });
      }
    } else if (employeeRoleScope) {
      const current = new URLSearchParams(Array.from(searchQuery.entries()));
      current.set('role', employeeRoleScope);
      push(`${pathname}?${current.toString()}`);
    } else if (queryParam) {
      const current = new URLSearchParams(Array.from(searchQuery.entries()));
      current.delete('role');
      push(`${pathname}?${current.toString()}`);
    }
  }, [queryParamRole, employeeRoleScope, isFirstRender]);

  if (syncRequired && isFirstRender) return null;
  return <>{children}</>;
};

export default RoleScopeContextSync;
