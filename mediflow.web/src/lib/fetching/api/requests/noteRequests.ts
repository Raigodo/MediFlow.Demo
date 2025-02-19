import { NoteEntity } from '@/lib/domain/entities/NoteEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { NoteFilterFlagStates } from '@/lib/domain/values/NoteFilterFlagStates';
import { format } from 'date-fns';
import { z } from 'zod';
import { getNoteSchema } from '@/lib/schemas/get/one/getNoteSchema';
import { getNotesSchema } from '@/lib/schemas/get/many/getNotesSchema';

type OneNoteResponse = z.infer<typeof getNoteSchema>;
type manyNotessResponse = z.infer<typeof getNotesSchema>;

const responseToEntity = (response: OneNoteResponse) => {
	return {
		...response,
		createdOn: new Date(response.createdOn),
	} as NoteEntity;
};

export const noteRequests = (customFetch = fetch) => {
	const getOne = async (request: {
		accessToken: string;
		clientId: string;
		noteId: string;
	}) => {
		const response = await makeHttpRequest<OneNoteResponse>(
			customFetch,
			baseUrlWithPath(
				`api/journal/clients/${request.clientId}/notes/${request.noteId}`,
			),
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

	const getTodays = async (request: {
		accessToken: string;
		clientId: string;
		employeeRole: EmployeeRoles | null;
	}) => {
		let path = `api/journal/clients/${request.clientId}/notes/today`;
		if (request.employeeRole) path += `?role=${request.employeeRole}`;
		const response = await makeHttpRequest<OneNoteResponse>(
			customFetch,
			baseUrlWithPath(path),
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

	const getMany = async (request: {
		accessToken: string;
		clientId: string;
		employeeRole: EmployeeRoles | null;
		dateFrom: Date | null;
		dateTo: Date | null;
		employeeId: string | null;
		flagState: NoteFilterFlagStates | null;
		page?: number;
		pageSize?: number;
	}) => {
		let path = `api/journal/clients/${request.clientId}/notes`;
		console.log(request);

		const queryParams: string[] = [];
		if (request.dateFrom)
			queryParams.push(`from=${format(request.dateFrom, 'yyyy-MM-dd')}`);

		if (request.dateTo)
			queryParams.push(`to=${format(request.dateTo, 'yyyy-MM-dd')}`);

		if (request.employeeRole)
			queryParams.push(`role=${request.employeeRole}`);

		if (request.employeeId)
			queryParams.push(`employeeId=${request.employeeId}`);

		if (request.flagState && request.flagState !== NoteFilterFlagStates.Any)
			queryParams.push(
				`flagged=${request.flagState === NoteFilterFlagStates.Flagged}`,
			);

		if (request.page) queryParams.push(`page=${request.page}`);
		if (request.pageSize) queryParams.push(`pageSize=${request.pageSize}`);

		if (queryParams.length >= 0) path += `?${queryParams.join('&')}`;

		const response = await makeHttpRequest<manyNotessResponse>(
			customFetch,
			baseUrlWithPath(path),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((note) => responseToEntity(note)),
		};
	};

	const writeTodays = async (request: {
		accessToken: string;
		clientId: string;
		isImportant: boolean;
		noteContent: string;
	}) => {
		const response = await makeHttpRequest<OneNoteResponse>(
			customFetch,
			baseUrlWithPath(
				`api/journal/clients/${request.clientId}/notes/today`,
			),
			{
				method: 'PUT',
				body: {
					noteContent: request.noteContent,
					noteIsImportant: request.isImportant,
					linkedItems: {},
				},
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const removeTodays = async (request: {
		accessToken: string;
		clientId: string;
	}) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(
				`api/journal/clients/${request.clientId}/notes/today`,
			),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	return {
		getOne,
		getMany,
		getTodays,
		writeTodays,
		removeTodays,
	};
};
