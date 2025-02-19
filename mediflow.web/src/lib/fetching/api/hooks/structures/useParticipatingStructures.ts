import { useQuery } from '@tanstack/react-query';
import { structureQueryKeys } from './structureQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useParticipatingStructures = () => {
	const { strucures } = useRequests();
	return useQuery({
		queryKey: structureQueryKeys.participating,
		queryFn: async () => {
			const res = await strucures.getParticipating();
			if (!res.ok) throw res;
			return res;
		},
	});
};
