'use client';

import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useState,
} from 'react';
import { useUserSession } from '../core/UserSessionContext';
import { UserRoles } from '@/lib/domain/values/UserRoles';

type SelectedEmployeeMetadata = {
  isCurrentRole: boolean;
  accessibleRoles: EmployeeRoles[];
};
type SelectedEmployeeParameters = {
  employeeRoleScope: EmployeeRoles | null;
};
type SelectedEmployeeActions = {
  selectEmployeeRoleScope: (params: {
    employeeRoleScope: EmployeeRoles;
  }) => void;
  resetSelection: () => void;
};
type SelectedEmployeeContext = SelectedEmployeeMetadata &
  SelectedEmployeeParameters &
  SelectedEmployeeActions;

const Context = createContext<SelectedEmployeeContext | undefined>(undefined);

const SelectedEmployeeRoleScopeContextProvider = ({
  children,
}: {
  children: ReactNode;
}) => {
  const { sessionData } = useUserSession();
  const [employeeRole, setEmployeeRole] =
    useState<SelectedEmployeeContext['employeeRoleScope']>(null);
  const metadata: SelectedEmployeeMetadata = {
    isCurrentRole: employeeRole === sessionData?.employeeRole,
    accessibleRoles: getAccessibleRoles(
      sessionData?.employeeRole ?? null,
      sessionData?.userRole ?? null,
    ),
  };

  const selectEmployeeRoleScope: SelectedEmployeeContext['selectEmployeeRoleScope'] =
    ({ employeeRoleScope: employeeRole }) => {
      setEmployeeRole(employeeRole);
    };

  const resetSelection = () => {
    setEmployeeRole(null);
  };

  useEffect(() => {
    setEmployeeRole(sessionData?.employeeRole ?? null);
  }, [sessionData?.employeeRole]);

  return (
    <Context.Provider
      value={{
        employeeRoleScope: employeeRole,
        ...metadata,
        selectEmployeeRoleScope,
        resetSelection,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export default SelectedEmployeeRoleScopeContextProvider;

export const useSelectedEmployeeRoleScope = () => {
  const context = useContext(Context);
  if (!context) throw Error('no selected employee role context');
  return context;
};

const roleHierarchy: Record<EmployeeRoles, EmployeeRoles[]> = {
  [EmployeeRoles.Nurse]: [
    EmployeeRoles.Nurse,
    EmployeeRoles.Caregiver,
    EmployeeRoles.SocialCaregiver,
    EmployeeRoles.SocialRehabilitator,
  ],
  [EmployeeRoles.Caregiver]: [
    EmployeeRoles.Caregiver,
    EmployeeRoles.SocialCaregiver,
    EmployeeRoles.SocialRehabilitator,
  ],
  [EmployeeRoles.SocialCaregiver]: [
    EmployeeRoles.SocialCaregiver,
    EmployeeRoles.SocialRehabilitator,
  ],
  [EmployeeRoles.SocialRehabilitator]: [EmployeeRoles.SocialRehabilitator],
};

const getAccessibleRoles = (
  employeeRole: EmployeeRoles | null,
  userRole: UserRoles | null,
) => {
  if (!userRole) {
    return [];
  }
  if (userRole === UserRoles.Manager || userRole === UserRoles.Admin) {
    return roleHierarchy[EmployeeRoles.Nurse]; //nurse can view all, so this way manager can also view all
  }
  if (!employeeRole) {
    return [];
  }
  return roleHierarchy[employeeRole] || [];
};
