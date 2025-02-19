import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<
	ReturnType<typeof useRequests>['users']['removeAvatar']
>[0];

export const useRemoveUserAvatar = () => {
	const { users } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return users.removeAvatar(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: userQueryKeys.currentAvatar,
			});
		},
	});
};
