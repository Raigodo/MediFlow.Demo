import { z } from 'zod';

export const getStructureSchema = z.object({
	id: z.string(),
	name: z.string(),
	manager: z.object({
		userId: z.string(),
		name: z.string(),
		surname: z.string(),
	}),
});
