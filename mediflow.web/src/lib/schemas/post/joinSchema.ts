import { z } from 'zod';
import { loginSchema } from './loginSchema';

const messages = {
	nonEmpty: 'Ievadiet ielūguma tokenu',
	tooLong: 'Ielūguma tokenam jābūt ne garākam par 36 simboliem',
};

export const joinSchema = loginSchema.extend({
	invitationToken: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(36, { message: messages.tooLong }),
});
