import { z } from 'zod';
import { InvalidityGroups } from '../../domain/values/InvalidityGroups';
import { InvalidityFlags } from '../../domain/values/InvalidityTypes';

const messages = {
	tooLongInput: 'Pārsniegts atļauto simbolu skaits',
	nonEmpty: 'Šis lauks ir obligāts',
	invalidDate: 'Izvēlaties derīgu datumu',
	invalidLength: (length: number) =>
		`Šim laukam jābūt tieši ${length} simbolus garam`,
	invalidEnum: 'Izvēlieties derīgu vērtību',
};

export const createClientSchema = z.object({
	clientId: z
		.string()
		.nonempty({ message: 'Ievadiet klienta identifikatoru' })
		.max(64, { message: messages.tooLongInput }),
	clientName: z
		.string()
		.nonempty({ message: 'Ievadiet klienta vārdu' })
		.max(64, { message: messages.tooLongInput }),
	clientBirthDate: z.date({ message: messages.invalidDate }),
	clientPersonalCode: z.string().length(12, {
		message: messages.invalidLength(12),
	}),
	clientSurname: z
		.string()
		.nonempty({ message: 'Ievadiet klienta uzvārdu' })
		.max(64, { message: 'Klienta uzvārdam jābūt īsākam par 64 simboliem' }),
	clientLanguage: z
		.string()
		.nonempty({ message: 'Ievadiet klienta valodu' })
		.max(32, { message: messages.tooLongInput }),
	clientReligion: z
		.string()
		.nonempty({ message: 'Ievadiet klienta reliģiju' })
		.max(32, { message: messages.tooLongInput }),
	clientInvalidityGroup: z.nativeEnum(InvalidityGroups, {
		message: messages.invalidEnum,
	}),
	clientInvalidityFlag: z.nativeEnum(InvalidityFlags, {
		message: messages.invalidEnum,
	}),
	clientInvalidityExpiresOn: z
		.date({ message: 'Izvēlaties invaliditātes beigu termiņu' })
		.optional(),
});
