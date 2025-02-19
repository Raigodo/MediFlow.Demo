import { StructureEntity } from '@/lib/domain/entities/StructureEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { getStructureSchema } from '@/lib/schemas/get/one/getStructureSchema';
import { getStructuresSchema } from '@/lib/schemas/get/many/getStructuresSchema';
import { z } from 'zod';

type OneStructureResponse = z.infer<typeof getStructureSchema>;
type ManyStructuresResponse = z.infer<typeof getStructuresSchema>;

const responseToEntity = (response: OneStructureResponse) => {
	return response as StructureEntity;
};

export const structureRequests = (customFetch = fetch) => {
	const getOne = async (request: {
		accessToken: string;
		structureId: string;
	}) => {
		const response = await makeHttpRequest<OneStructureResponse>(
			customFetch,
			baseUrlWithPath(`api/structures/${request.structureId}`),
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

	const getCurrent = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<OneStructureResponse>(
			customFetch,
			baseUrlWithPath(`api/structures/current`),
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

	const getOwned = async (request: {
		accessToken: string;
		managerId?: string;
	}) => {
		const response = await makeHttpRequest<ManyStructuresResponse>(
			customFetch,
			baseUrlWithPath(
				`api/structures/owned${
					request.managerId && `?managerId=${request.managerId}`
				}`,
			),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((structure) =>
				responseToEntity(structure),
			),
		};
	};

	const getParticipating = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<ManyStructuresResponse>(
			customFetch,
			baseUrlWithPath(`api/structures/participating`),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((structure) =>
				responseToEntity(structure),
			),
		};
	};

	const create = async (request: {
		accessToken: string;
		structureName: string;
	}) => {
		const response = await makeHttpRequest<OneStructureResponse>(
			customFetch,
			baseUrlWithPath(`api/structures`),
			{
				method: 'POST',
				body: { structureName: request.structureName },
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const update = async (request: {
		accessToken: string;
		structureName: string;
	}) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/structures/current`),
			{
				method: 'PATCH',
				body: { structureName: request.structureName },
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	const remove = async (request: {
		accessToken: string;
		structureId: string;
	}) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/structures/${request.structureId}`),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	const trustDevice = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath('api/structures/current/trust-device'),
			{
				method: 'POST',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	return {
		getOne,
		getCurrent,
		getOwned,
		getParticipating,
		create,
		update,
		remove,
		trustDevice,
	};
};
