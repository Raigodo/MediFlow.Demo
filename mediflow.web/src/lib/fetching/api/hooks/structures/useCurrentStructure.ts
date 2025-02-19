import { useQuery } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useCurrentStructure = () => {
	const { strucures } = useRequests();
	return useQuery({
		queryKey: structureQueryKeys.current,
		queryFn: async () => {
			const res = await strucures.getCurrent();
			if (!res.ok) throw res;
			return res;
		},
	});
};
