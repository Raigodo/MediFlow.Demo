import { useMutation, useQueryClient } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type Params = Parameters<
	ReturnType<typeof useRequests>['strucures']['remove']
>[0];

export const useRemoveStructure = () => {
	const { strucures } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: Params) => {
			return strucures.remove(req);
		},
		onSuccess: (_, req) => {
			queryClient.removeQueries({
				queryKey: structureQueryKeys.one(req),
			});
			queryClient.invalidateQueries({
				queryKey: structureQueryKeys.all,
			});
		},
	});
};
