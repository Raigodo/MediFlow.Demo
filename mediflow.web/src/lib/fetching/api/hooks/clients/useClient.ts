import { useQuery } from '@tanstack/react-query';
import { clientQueryKeys } from './clientQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useClient = (clientId: string) => {
	const { clients } = useRequests();
	return useQuery({
		queryKey: clientQueryKeys.one({ clientId }),
		queryFn: async () => {
			const res = await clients.getOne({ clientId });
			if (!res.ok) throw res;
			return res;
		},
	});
};
