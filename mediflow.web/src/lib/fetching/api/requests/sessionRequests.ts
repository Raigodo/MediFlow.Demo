import { SessionEntity } from '@/lib/domain/entities/SessionEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { UserEntity } from '@/lib/domain/entities/UserEntity';
import { z } from 'zod';
import { getSessionSchema } from '@/lib/schemas/get/one/getSessionSchema';
import { getUserSchema } from '@/lib/schemas/get/one/getUserSchema';

type SessionResponse = z.infer<typeof getSessionSchema>;
type UserResponse = z.infer<typeof getUserSchema>;

const responseToEntity = (response: SessionResponse) => {
	return {
		...response,
		expirationDate: new Date(response.expirationDate),
	} as SessionEntity;
};

export const sessionRequests = (customFetch = fetch) => {
	const logIn = async (request: { email: string; password: string }) => {
		const response = await makeHttpRequest<SessionResponse>(
			customFetch,
			baseUrlWithPath('api/auth/login'),
			{
				method: 'POST',
				body: { email: request.email, password: request.password },
				credentials: 'include',
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const register = async (request: {
		email: string;
		password: string;
		userName: string;
		userSurname: string;
		invitationToken: string;
	}) => {
		const response = await makeHttpRequest<UserResponse>(
			customFetch,
			baseUrlWithPath('api/auth/register'),
			{
				method: 'POST',
				body: {
					email: request.email,
					password: request.password,
					userName: request.userName,
					userSurname: request.userSurname,
					invitationId: request.invitationToken,
				},
				credentials: 'include',
			},
		);
		return {
			...response,
			body: response.body as UserEntity | null,
		};
	};

	const join = async (request: {
		email: string;
		password: string;
		invitationToken: string;
	}) => {
		const response = await makeHttpRequest<SessionResponse>(
			customFetch,
			baseUrlWithPath('api/auth/join'),
			{
				method: 'POST',
				body: {
					email: request.email,
					password: request.password,
					invitationId: request.invitationToken,
				},
				credentials: 'include',
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const refresh = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<SessionResponse>(
			customFetch,
			baseUrlWithPath('api/auth/refresh'),
			{
				method: 'POST',
				headers: { Authorization: 'Bearer ' + request.accessToken },
				credentials: 'include',
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const alter = async (request: {
		accessToken: string;
		structureId: string;
	}) => {
		const response = await makeHttpRequest<SessionResponse>(
			customFetch,
			baseUrlWithPath('api/auth/alter'),
			{
				method: 'POST',
				body: { structureId: request.structureId },
				headers: { Authorization: 'Bearer ' + request.accessToken },
				credentials: 'include',
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	return { logIn, register, join, refresh, alter };
};
