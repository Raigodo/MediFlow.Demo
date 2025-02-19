import { z } from 'zod';

const messages = {
	email: 'Ievadiet derīgu e-pasta adresi',
	passwordEmpty: 'Parole ir obligāta',
	passwordMin: 'Parolei jābūt vismaz 6 simbolus garai',
	passwordMax: 'Parolei jābūt ne garākai par 16 simboliem',
};

export const loginSchema = z.object({
	email: z.string().email({ message: messages.email }),
	password: z
		.string()
		.nonempty({ message: messages.passwordEmpty })
		.min(6, { message: messages.passwordMin })
		.max(16, { message: messages.passwordMax }),
});
