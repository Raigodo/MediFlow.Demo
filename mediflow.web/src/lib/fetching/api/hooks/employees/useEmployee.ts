import { useQuery } from '@tanstack/react-query';
import { employeeQueryKeys } from './employeeQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useEmployee = (employeeId: string) => {
	const { employees } = useRequests();
	return useQuery({
		queryKey: employeeQueryKeys.one({ employeeId }),
		queryFn: async () => {
			const res = await employees.getOne({ employeeId });
			if (!res.ok) throw res;
			return res;
		},
	});
};
