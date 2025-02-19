import { z } from 'zod';
import { UserRoles } from '../../domain/values/UserRoles';

const messages = {
	email: 'Ievadiet derīgu e-pasta adresi',
	passwordEmpty: 'Parole ir obligāta',
	passwordMin: 'Parolei jābūt vismaz 6 simbolus garai',
	passwordMax: 'Parolei jābūt ne garākai par 16 simboliem',
	nonEmpty: 'Šis lauks ir obligāts',
	tooLong: (field: string, max: number) =>
		`${field} jābūt īsākam par ${max} simboliem`,
	invalidEnum: 'Izvēlieties derīgu lomu lietotājam',
};

export const createUserSchema = z.object({
	email: z.string().email({ message: messages.email }),
	password: z
		.string()
		.nonempty({ message: messages.passwordEmpty })
		.min(6, { message: messages.passwordMin })
		.max(16, { message: messages.passwordMax }),
	userName: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(64, { message: messages.tooLong('Lietotāja vārdam', 64) }),
	userSurname: z
		.string()
		.nonempty({ message: messages.nonEmpty })
		.max(64, { message: messages.tooLong('Lietotāja uzvārdam', 64) }),
	userRole: z.nativeEnum(UserRoles, { message: messages.invalidEnum }),
});
