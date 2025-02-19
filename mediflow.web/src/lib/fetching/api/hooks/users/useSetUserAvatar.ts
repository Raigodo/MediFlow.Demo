import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<
	ReturnType<typeof useRequests>['users']['setAvatar']
>[0];

export const useSetUserAvatar = () => {
	const { users } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			const res = await users.setAvatar(req);
			return res;
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: userQueryKeys.currentAvatar,
			});
		},
	});
};
