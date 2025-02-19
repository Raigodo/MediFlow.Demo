import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { UserRoles } from '@/lib/domain/values/UserRoles';
import { z } from 'zod';

export const getSessionSchema = z.object({
	accessToken: z.string(),
	expirationDate: z.string(),
	sessionData: z.object({
		userId: z.string(),
		userRole: z.nativeEnum(UserRoles),
		structureId: z.string().nullish(),
		employeeId: z.string().nullish(),
		employeeRole: z.nativeEnum(EmployeeRoles).nullish(),
	}),
});
