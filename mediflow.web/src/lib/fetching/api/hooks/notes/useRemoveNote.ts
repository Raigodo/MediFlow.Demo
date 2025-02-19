import { useMutation, useQueryClient } from '@tanstack/react-query';
import { noteQueryKeys } from './noteQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';

type params = Parameters<
	ReturnType<typeof useRequests>['notes']['removeTodays']
>[0];

export const useRemoveNote = () => {
	const { notes } = useRequests();
	const { sessionData } = useUserSession();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return notes.removeTodays(req);
		},
		onSuccess: (_, req) => {
			if (!sessionData?.employeeRole) {
				queryClient.removeQueries({ queryKey: noteQueryKeys.all });
				return;
			}
			queryClient.removeQueries({
				queryKey: noteQueryKeys.todays({
					...req,
					employeeRole: sessionData.employeeRole,
				}),
			});
			queryClient.invalidateQueries({
				queryKey: noteQueryKeys.all,
			});
		},
	});
};
