import type { EmployeeRoles } from '../values/EmployeeRoles';

export type InvitationEntity = {
	id: string;
	structureId: string;
	structureName: string;
	employeeRole: EmployeeRoles;
	issuedAt: Date;
};
