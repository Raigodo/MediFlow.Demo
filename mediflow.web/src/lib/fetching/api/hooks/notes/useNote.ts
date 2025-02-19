import { useQuery } from '@tanstack/react-query';
import { noteQueryKeys } from './noteQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

type params = Parameters<ReturnType<typeof useRequests>['notes']['getOne']>[0];

export const useNote = (req: params) => {
	const { notes } = useRequests();
	return useQuery({
		queryKey: noteQueryKeys.one(req),
		queryFn: async () => {
			const res = await notes.getOne(req);
			if (!res.ok) throw res;
			return res;
		},
	});
};
