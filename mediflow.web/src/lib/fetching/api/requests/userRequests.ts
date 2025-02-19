import { UserEntity } from '@/lib/domain/entities/UserEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { UserRoles } from '@/lib/domain/values/UserRoles';
import { z } from 'zod';
import { getUserSchema } from '@/lib/schemas/get/one/getUserSchema';
import { getUsersSchema } from '@/lib/schemas/get/many/getUsersSchema';

type OneUserResponse = z.infer<typeof getUserSchema>;
type ManyUsersResponse = z.infer<typeof getUsersSchema>;

const responseToEntity = (response: OneUserResponse) => {
	return response as UserEntity;
};

export const userRequests = (customFetch = fetch) => {
	const getOne = async (request: { accessToken: string; userId: string }) => {
		const response = await makeHttpRequest<OneUserResponse>(
			customFetch,
			baseUrlWithPath(`api/users/${request.userId}`),
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

	const getAllManagers = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<ManyUsersResponse>(
			customFetch,
			baseUrlWithPath('api/users/managers'),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((user) => responseToEntity(user)),
		};
	};

	const getCurrent = async (request: { accessToken: string }) => {
		const response = await makeHttpRequest<OneUserResponse>(
			customFetch,
			baseUrlWithPath('api/users/current'),
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

	const create = async (request: {
		accessToken: string;
		email: string;
		password: string;
		userName: string;
		userSurname: string;
		userRole: UserRoles;
	}) => {
		const response = await makeHttpRequest<OneUserResponse>(
			customFetch,
			baseUrlWithPath('api/users'),
			{
				method: 'POST',
				body: {
					email: request.email,
					password: request.password,
					userName: request.userName,
					userSurname: request.userSurname,
					userRole: request.userRole,
				},
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: response.body && responseToEntity(response.body),
		};
	};

	const update = async (request: { accessToken: string; userId: string }) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/profile/${request.userId}`),
			{
				method: 'PATCH',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	const remove = async (request: { accessToken: string; userId: string }) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/profile/${request.userId}`),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	const getAvatar = async (request: {
		accessToken: string;
		userId: string | undefined;
	}) => {
		const url = baseUrlWithPath(
			request.userId
				? `api/users/${request.userId}/avatar`
				: 'api/users/current/avatar',
		);
		const response = await makeHttpRequest<Blob>(customFetch, url, {
			method: 'GET',
			headers: { Authorization: 'Bearer ' + request.accessToken },
			responseContentType: 'BLOB',
		});
		if (!response.ok) return { ...response, body: null };
		return {
			...response,
			body: response.body ? URL.createObjectURL(response.body) : null,
		};
	};

	const setAvatar = (request: { accessToken: string; avatarImage: File }) => {
		const formData = new FormData();
		formData.append('file', request.avatarImage);
		return makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/users/current/avatar`),
			{
				method: 'PUT',
				headers: { Authorization: 'Bearer ' + request.accessToken },
				requestContentType: 'BLOB',
				body: formData,
			},
		);
	};

	const removeAvatar = (request: { accessToken: string }) => {
		return makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/users/current/avatar`),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
	};

	return {
		getOne,
		getAllManagers,
		getCurrent,
		update,
		remove,
		create,
		getAvatar,
		setAvatar,
		removeAvatar,
	};
};
