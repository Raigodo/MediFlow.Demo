import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<ReturnType<typeof useRequests>['users']['remove']>[0];

export const useRemoveUser = () => {
	const { users } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return users.remove(req);
		},
		onSuccess: (_, req) => {
			queryClient.removeQueries({ queryKey: userQueryKeys.one(req) });
			queryClient.invalidateQueries({
				queryKey: userQueryKeys.all,
			});
		},
	});
};
