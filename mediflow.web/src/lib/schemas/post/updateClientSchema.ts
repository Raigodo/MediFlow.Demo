import { z } from 'zod';
import { createClientSchema } from './createClientSchema';

const messages = {
	nonEmpty: 'Šis lauks ir obligāts',
	invalidPhoneNumber: 'Ievadiet derīgu tālruņa numuru',
};

function createUpdateClientSchema() {
	const partialPart = createClientSchema.omit({ clientId: true }).partial();
	const clientId = createClientSchema.pick({ clientId: true });
	return partialPart.merge(clientId).extend({
		clientContacts: z.array(
			z.object({
				clientContactId: z.string().optional(),
				clientContactTitle: z
					.string()
					.nonempty({ message: messages.nonEmpty }),
				clientContactPhoneNumber: z
					.string()
					.nonempty({ message: messages.nonEmpty })
					.regex(/^\+?[0-9\s\-()]{7,15}$/, {
						message: messages.invalidPhoneNumber,
					}),
			}),
		),
	});
}

export const updateClientSchema = createUpdateClientSchema();
