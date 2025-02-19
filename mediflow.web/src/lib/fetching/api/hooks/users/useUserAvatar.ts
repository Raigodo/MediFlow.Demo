import { useQuery } from '@tanstack/react-query';
import { userQueryKeys } from './userQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useUserAvatar = (userId?: string) => {
	const { users } = useRequests();
	return useQuery({
		queryKey: userQueryKeys.currentAvatar,
		queryFn: async () => {
			const res = await users.getAvatar({ userId });
			if (res.status === 404) return res;
			if (!res.ok) throw res;
			return res;
		},
	});
};
