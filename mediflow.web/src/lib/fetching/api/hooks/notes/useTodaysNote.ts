import { useQuery } from '@tanstack/react-query';
import { noteQueryKeys } from './noteQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';

type params = Parameters<
	ReturnType<typeof useRequests>['notes']['getTodays']
>[0];

export const useTodaysNote = (req: params) => {
	const { notes } = useRequests();
	const { sessionData } = useUserSession();
	if (!sessionData?.employeeRole)
		throw Error('current session has no employee role');
	return useQuery({
		queryKey: noteQueryKeys.todays({
			...req,
			employeeRole: sessionData.employeeRole,
		}),
		queryFn: async () => {
			const res = await notes.getTodays(req);
			if (!res.ok) throw res;
			return res;
		},
	});
};
