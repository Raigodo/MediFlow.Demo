import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<ReturnType<typeof useRequests>['users']['create']>[0];

export const useCreateUser = () => {
	const { users } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return users.create(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: userQueryKeys.all,
			});
		},
	});
};
