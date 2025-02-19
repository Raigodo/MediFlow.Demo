import { useMutation, useQueryClient } from '@tanstack/react-query';
import { noteQueryKeys } from './noteQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['notes']['writeTodays']
>[0];

export const useWriteNote = () => {
	const { notes } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return notes.writeTodays(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: noteQueryKeys.all,
			});
		},
	});
};
