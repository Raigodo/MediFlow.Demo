import { z } from 'zod';

const messages = {
	nonEmpty: 'Ievadiet struktūras nosaukumu',
	tooLongInput: 'Struktūras nosaukumam jābūt īsākam par 128 simboliem',
};

export const createStructureSchema = z.object({
	structureName: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(128, { message: messages.tooLongInput }),
});
