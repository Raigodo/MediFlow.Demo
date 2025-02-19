import { z } from 'zod';
import { joinSchema } from './joinSchema';

const messages = {
	nonEmpty: 'Šis lauks ir obligāts',
	tooLong: (field: string, max: number) =>
		`${field} jābūt īsākam par ${max} simboliem`,
};

export const registerSchema = joinSchema.extend({
	userName: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(64, { message: messages.tooLong('Lietotāja vārdam', 64) }),
	userSurname: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(64, { message: messages.tooLong('Lietotāja uzvārdam', 64) }),
});
