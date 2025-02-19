import { useMutation, useQueryClient } from '@tanstack/react-query';
import { clientQueryKeys } from './clientQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<
	ReturnType<typeof useRequests>['clients']['remove']
>[0];

export const useRemoveClient = () => {
	const { clients } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return clients.remove(req);
		},
		onSuccess: (_, req) => {
			queryClient.removeQueries({ queryKey: clientQueryKeys.one(req) });
			queryClient.invalidateQueries({
				queryKey: clientQueryKeys.all,
			});
		},
	});
};
