import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { LinkableItemTypes } from '@/lib/domain/values/LinkableItemTypes';
import { z } from 'zod';

export const getNoteSchema = z.object({
	id: z.string(),
	clientId: z.string(),
	content: z.string(),
	isFlagged: z.boolean(),
	createdOn: z.string(),
	linkedItems: z.object({
		[LinkableItemTypes.Ambulance]: z.array(z.string()).optional(),
		[LinkableItemTypes.Evaluation]: z.array(z.string()).optional(),
		[LinkableItemTypes.MedicalCare]: z.array(z.string()).optional(),
		[LinkableItemTypes.Medicaments]: z.array(z.string()).optional(),
	}),
	creator: z.object({
		id: z.string(),
		role: z.nativeEnum(EmployeeRoles),
		name: z.string().optional(),
		surname: z.string().optional(),
	}),
});
