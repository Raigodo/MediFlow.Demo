import { InvalidityGroups } from '@/lib/domain/values/InvalidityGroups';
import { InvalidityFlags } from '@/lib/domain/values/InvalidityTypes';
import { z } from 'zod';

export const getClientSchema = z.object({
	id: z.string(),
	structureId: z.string(),
	name: z.string(),
	surname: z.string(),
	birthDate: z.string(),
	personalCode: z.string(),
	language: z.string(),
	religion: z.string(),
	invalidtiyGroup: z.nativeEnum(InvalidityGroups),
	invalidityFlag: z.nativeEnum(InvalidityFlags),
	invalidityExpiresOn: z.string().optional(),
	joinedOn: z.string(),
	contacts: z.array(
		z.object({
			id: z.string(),
			title: z.string(),
			phoneNumber: z.string(),
		}),
	),
});
