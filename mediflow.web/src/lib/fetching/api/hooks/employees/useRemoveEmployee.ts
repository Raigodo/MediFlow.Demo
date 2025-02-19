import { useMutation, useQueryClient } from '@tanstack/react-query';
import { employeeQueryKeys } from './employeeQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['employees']['remove']
>[0];

export const useRemoveEmployee = () => {
	const { employees } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return employees.remove(req);
		},
		onSuccess: (_, req) => {
			queryClient.removeQueries({ queryKey: employeeQueryKeys.one(req) });
			queryClient.invalidateQueries({
				queryKey: employeeQueryKeys.all,
			});
		},
	});
};
