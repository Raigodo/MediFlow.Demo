import { useQuery } from '@tanstack/react-query';
import { invitationQueryKeys } from './invitationQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useInvitations = () => {
	const { invitations } = useRequests();
	return useQuery({
		queryKey: invitationQueryKeys.all,
		queryFn: async () => {
			const res = await invitations.getAll();
			if (!res.ok) throw res;
			return res;
		},
	});
};
