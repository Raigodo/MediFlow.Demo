import { useQuery } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useManagers = () => {
	const { users } = useRequests();
	return useQuery({
		queryKey: userQueryKeys.all,
		queryFn: async () => {
			const res = await users.getAllManagers();
			if (!res.ok) throw res;
			return res;
		},
	});
};
