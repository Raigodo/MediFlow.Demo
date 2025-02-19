import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { z } from 'zod';
import { getInvitationSchema } from '@/lib/schemas/get/one/getInvitationSchema';
import { getInvitationsSchema } from '@/lib/schemas/get/many/getInvitationsSchema';
import { InvitationEntity } from '@/lib/domain/entities/InvitationEntity';

type OneInvitationResponse = z.infer<typeof getInvitationSchema>;
type ManyInvitationsResponse = z.infer<typeof getInvitationsSchema>;

const responseToEntity = (response: OneInvitationResponse) => {
	return {
		...response,
		issuedAt: new Date(response.issuedAt),
	} as InvitationEntity;
};

export const invitationRequests = (customFetch = fetch) => {
	const create = async (request: {
		accessToken: string;
		employeeRole: EmployeeRoles;
	}) => {
		const response = await makeHttpRequest<OneInvitationResponse>(
			customFetch,
			baseUrlWithPath(`api/employees/invitations`),
			{
				method: 'POST',
				body: { employeeRole: request.employeeRole },
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const getAll = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<ManyInvitationsResponse>(
			customFetch,
			baseUrlWithPath(`api/employees/invitations`),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((invitation) =>
				responseToEntity(invitation),
			),
		};
	};

	const remove = async (request: {
		accessToken: string;
		invitationId: string;
	}) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(
				`api/employees/invitations/${request.invitationId}`,
			),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	return {
		create,
		getAll,
		remove,
	};
};
