import type { EmployeeRoles } from '../values/EmployeeRoles';

export type EmployeeEntity = {
	id: string;
	userId: string | null;
	structureId: string;
	name: string | null;
	surname: string | null;
	role: EmployeeRoles;
	assignedOn: Date;
	unassignedOn: Date | null;
};
