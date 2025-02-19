import { useMutation, useQueryClient } from '@tanstack/react-query';
import { clientQueryKeys } from './clientQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<ReturnType<typeof useRequests>['clients']['add']>[0];

export const useCreateClient = () => {
	const { clients } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return clients.add(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: clientQueryKeys.all,
			});
		},
	});
};
