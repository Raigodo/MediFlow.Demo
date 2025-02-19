import { useMutation, useQueryClient } from '@tanstack/react-query';
import { invitationQueryKeys } from './invitationQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<
	ReturnType<typeof useRequests>['invitations']['create']
>[0];

export const useCreateInvitation = () => {
	const { invitations } = useRequests();
	const queryClient = useQueryClient();
	return useMutation({
		mutationFn: async (req: params) => {
			return invitations.create(req);
		},
		onSuccess: () => {
			queryClient.invalidateQueries({
				queryKey: invitationQueryKeys.all,
			});
		},
	});
};
