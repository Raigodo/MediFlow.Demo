import { EmployeeRoles as EmployeeRoles } from '../values/EmployeeRoles';
import { UserRoles } from '../values/UserRoles';

export type SessionEntity = {
	accessToken: string;
	expirationDate: Date;
	sessionData: {
		userId: string;
		userRole: UserRoles;
		structureId: string | null;
		employeeId: string | null;
		employeeRole: EmployeeRoles | null;
	};
};
