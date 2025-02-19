import { useMutation, useQueryClient } from '@tanstack/react-query';
import { clientQueryKeys } from './clientQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['clients']['update']
>[0];

export const useUpdateClient = () => {
	const { clients } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return clients.update(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: clientQueryKeys.all,
			});
		},
	});
};
