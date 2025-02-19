import { useQuery } from '@tanstack/react-query';
import { noteQueryKeys } from './noteQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { NoteFilterFlagStates } from '@/lib/domain/values/NoteFilterFlagStates';

type params = Parameters<ReturnType<typeof useRequests>['notes']['getMany']>[0];

export const useNotes = (req: params) => {
	const { notes } = useRequests();
	const { sessionData } = useUserSession();

	if (!sessionData || (!req.employeeRole && sessionData.employeeRole))
		throw Error('can not get notes without session and employee role');
	return useQuery({
		queryKey: noteQueryKeys.many({
			clientId: req.clientId,
			role: req.employeeRole ?? sessionData.employeeRole!,
			employeeId: req.employeeId,
			from: req.dateFrom,
			to: req.dateTo,
			flag: req.flagState ?? NoteFilterFlagStates.Any,
		}),
		queryFn: async () => {
			const res = await notes.getMany(req);
			if (!res.ok) throw res;
			return res;
		},
	});
};
