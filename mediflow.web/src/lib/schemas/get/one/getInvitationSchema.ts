import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { z } from 'zod';

export const getInvitationSchema = z.object({
	id: z.string(),
	structureId: z.string(),
	structureName: z.string(),
	employeeRole: z.nativeEnum(EmployeeRoles),
	issuedAt: z.string(),
});
