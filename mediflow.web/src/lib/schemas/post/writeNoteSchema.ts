import { z } from 'zod';

const messages = {
	nonEmpty: 'Ievadiet žurnāla ieraksta saturu',
	tooLong: 'Piezīmei jābūt īsākai par 1000 simboliem',
};

export const writeNoteSchema = z.object({
	isImportant: z.boolean(),
	noteContent: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(1000, { message: messages.tooLong }),
});
