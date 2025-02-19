import { z } from 'zod';
import { EmployeeRoles } from '../../domain/values/EmployeeRoles';

const messages = {
	invalidEnum: 'Izvēlieties derīgu lomu darbiniekam',
};

export const createInvitationSchema = z.object({
	employeeRole: z.nativeEnum(EmployeeRoles, {
		message: messages.invalidEnum,
	}),
});
