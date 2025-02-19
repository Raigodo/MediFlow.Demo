import { useMutation, useQueryClient } from '@tanstack/react-query';
import { invitationQueryKeys } from './invitationQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['invitations']['remove']
>[0];

export const useRemoveInvitation = () => {
	const { invitations } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return invitations.remove(req);
		},
		onSuccess: (_, req) => {
			queryClient.removeQueries({
				queryKey: invitationQueryKeys.one(req),
			});
			queryClient.invalidateQueries({
				queryKey: invitationQueryKeys.all,
			});
		},
	});
};
