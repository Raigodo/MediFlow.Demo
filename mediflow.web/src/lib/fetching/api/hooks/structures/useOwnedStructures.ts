import { useQuery } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useOwnedStructures = (params?: { managerId: string }) => {
	const { strucures } = useRequests();
	const { sessionData, lifecycle } = useUserSession();
	if (!lifecycle.ok || !sessionData?.userId)
		throw Error('can not make request without active session');
	return useQuery({
		queryKey: structureQueryKeys.owned(
			params?.managerId ?? sessionData.userId,
		),
		queryFn: async () => {
			const res = await strucures.getOwned({
				managerId: params?.managerId,
			});
			if (!res.ok) throw res;
			return res;
		},
	});
};
