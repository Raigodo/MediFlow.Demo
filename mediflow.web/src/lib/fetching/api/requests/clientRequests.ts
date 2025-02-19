import { ClientEntity } from '@/lib/domain/entities/ClientEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { z } from 'zod';
import { getClientSchema } from '@/lib/schemas/get/one/getClientSchema';
import { getClientsSchema } from '@/lib/schemas/get/many/getClientsSchema';
import { updateClientSchema } from '@/lib/schemas/post/updateClientSchema';
import { createClientSchema } from '@/lib/schemas/post/createClientSchema';

type OneClientResponse = z.infer<typeof getClientSchema>;
type ManyClientsResponse = z.infer<typeof getClientsSchema>;

const responseToEntity = (response: OneClientResponse) => {
	return {
		...response,
		birthDate: new Date(response.birthDate),
		invalidityExpiresOn: response.invalidityExpiresOn
			? new Date(response.invalidityExpiresOn)
			: null,
		joinedOn: new Date(response.joinedOn),
	} as ClientEntity;
};

export const clientRequests = (customFetch = fetch) => {
	const getOne = async (request: {
		accessToken: string;
		clientId: string;
	}) => {
		const response = await makeHttpRequest<OneClientResponse>(
			customFetch,
			baseUrlWithPath(`api/clients/${request.clientId}`),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const getAll = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<ManyClientsResponse>(
			customFetch,
			baseUrlWithPath('api/clients'),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((client) =>
				responseToEntity(client),
			),
		};
	};

	const add = async (
		request: z.infer<typeof createClientSchema> & {
			accessToken: string;
		},
	) => {
		const response = await makeHttpRequest<OneClientResponse>(
			customFetch,
			baseUrlWithPath(`api/clients`),
			{
				method: 'POST',
				body: {
					clientId: request.clientId,
					clientName: request.clientName,
					clientSurname: request.clientSurname,
					clientPersonalCode: request.clientPersonalCode,
					clientBirthDate: request.clientBirthDate
						.toISOString()
						?.split('T')[0],
					clientLanguage: request.clientLanguage,
					clientReligion: request.clientReligion,
					clientInvalidityGroup: request.clientInvalidityGroup,
					clientInvalidityFlag: request.clientInvalidityFlag,
					clientInvalidityExpiresOn: request.clientInvalidityExpiresOn
						?.toISOString()
						?.split('T')[0],
				},
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const update = async (
		request: z.infer<typeof updateClientSchema> & {
			accessToken: string;
		},
	) => {
		return makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/clients/${request.clientId}`),
			{
				method: 'PATCH',
				body: {
					clientName: request.clientName || undefined,
					clientSurname: request.clientSurname || undefined,
					clientPersonalCode: request.clientPersonalCode || undefined,
					clientInvalidityFlag:
						request.clientInvalidityFlag || undefined,
					clientBirthDate: request.clientBirthDate
						?.toISOString()
						?.split('T')[0],
					clientLanguage: request.clientLanguage || undefined,
					clientReligion: request.clientReligion || undefined,
					clientInvalidityGroup:
						request.clientInvalidityGroup || undefined,
					clientInvalitidyFlag:
						request.clientInvalidityFlag || undefined,
					clientInvalidityExpiresOn: request.clientInvalidityExpiresOn
						?.toISOString()
						?.split('T')[0],
					clientContacts: request.clientContacts || undefined,
				},
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
	};

	const remove = async (request: {
		accessToken: string;
		clientId: string;
	}) => {
		return makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/clients/${request.clientId}`),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
	};

	return {
		getOne,
		getAll,
		add,
		update,
		remove,
	};
};
