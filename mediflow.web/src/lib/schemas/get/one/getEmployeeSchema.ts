import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { z } from 'zod';

export const getEmployeeSchema = z.object({
	id: z.string(),
	structureId: z.string(),
	role: z.nativeEnum(EmployeeRoles),
	userId: z.string().optional(),
	name: z.string().optional(),
	surname: z.string().optional(),
	assignedOn: z.string(),
	unassignedOn: z.string().optional(),
});
