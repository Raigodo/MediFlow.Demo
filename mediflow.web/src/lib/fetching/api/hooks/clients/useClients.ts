import { useQuery } from '@tanstack/react-query';
import { clientQueryKeys } from './clientQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useClients = () => {
	const { clients } = useRequests();
	return useQuery({
		queryKey: clientQueryKeys.all,
		queryFn: async () => {
			const res = await clients.getAll();
			if (!res.ok) throw res;
			return res;
		},
	});
};
