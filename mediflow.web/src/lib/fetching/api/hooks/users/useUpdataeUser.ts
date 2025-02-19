import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<ReturnType<typeof useRequests>['users']['update']>[0];

export const useUpdataeUser = () => {
	const { users } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return users.update(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: userQueryKeys.all,
			});
		},
	});
};
