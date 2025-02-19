import { useQuery } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useStructure = (structureId: string) => {
	const { strucures } = useRequests();
	return useQuery({
		queryKey: structureQueryKeys.one({ structureId }),
		queryFn: async () => {
			const res = await strucures.getOne({ structureId });
			if (!res.ok) throw res;
			return res;
		},
	});
};
