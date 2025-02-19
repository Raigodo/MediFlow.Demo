import { useMutation, useQueryClient } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['strucures']['create']
>[0];

export const useCreateStructure = () => {
	const { strucures } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return strucures.create(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: structureQueryKeys.all,
			});
		},
	});
};
