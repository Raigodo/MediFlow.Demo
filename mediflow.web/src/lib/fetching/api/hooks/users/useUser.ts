import { useQuery } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useUser = (userId: string) => {
	const { users } = useRequests();
	return useQuery({
		queryKey: userQueryKeys.one({ userId }),
		queryFn: async () => {
			const res = await users.getOne({ userId });
			if (!res.ok) throw res;
			return res;
		},
	});
};
