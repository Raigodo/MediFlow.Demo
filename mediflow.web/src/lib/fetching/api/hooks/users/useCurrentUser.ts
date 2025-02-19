import { useQuery } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useCurrentUser = () => {
	const { users } = useRequests();
	return useQuery({
		queryKey: userQueryKeys.currentUser,
		queryFn: async () => {
			const res = await users.getCurrent();
			if (!res.ok) throw res;
			return res;
		},
	});
};
