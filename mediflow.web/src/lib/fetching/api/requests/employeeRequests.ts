import { EmployeeEntity } from '@/lib/domain/entities/EmployeeEntity';
import { makeHttpRequest } from '../base/makeHttpRequest';
import { baseUrlWithPath } from '../helpers/url';
import { z } from 'zod';
import { getEmployeeSchema } from '@/lib/schemas/get/one/getEmployeeSchema';
import { getEmployeesSchema } from '@/lib/schemas/get/many/getEmployeesSchema';

type OneEmployeeResponse = z.infer<typeof getEmployeeSchema>;
type ManyEmployeesResponse = z.infer<typeof getEmployeesSchema>;

const responseToEntity = (response: OneEmployeeResponse) => {
	return {
		...response,
		userId: response.userId ?? null,
		name: response.name ?? null,
		surname: response.surname ?? null,
		assignedOn: new Date(response.assignedOn),
		unassignedOn: response.unassignedOn
			? new Date(response.unassignedOn)
			: null,
	} as EmployeeEntity;
};

export const employeeRequests = (customFetch = fetch) => {
	const getOne = async (request: {
		accessToken: string;
		employeeId: string;
	}) => {
		const response = await makeHttpRequest<OneEmployeeResponse>(
			customFetch,
			baseUrlWithPath(`api/employees/${request.employeeId}`),
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
		const response = await makeHttpRequest<OneEmployeeResponse>(
			customFetch,
			baseUrlWithPath(`api/employees/current`),
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
		const response = await makeHttpRequest<ManyEmployeesResponse>(
			customFetch,
			baseUrlWithPath('api/employees'),
			{
				method: 'GET',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return {
			...response,
			body: (response.body ?? []).map((employee) =>
				responseToEntity(employee),
			),
		};
	};

	const remove = async (request: {
		accessToken: string;
		employeeId: string;
	}) => {
		const response = await makeHttpRequest<void>(
			customFetch,
			baseUrlWithPath(`api/employees/${request.employeeId}`),
			{
				method: 'DELETE',
				headers: { Authorization: 'Bearer ' + request.accessToken },
			},
		);
		return response;
	};

	return {
		getOne,
		getCurrent,
		getAll,
		remove,
	};
};
