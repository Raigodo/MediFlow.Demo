import { useQuery } from '@tanstack/react-query';
import { employeeQueryKeys } from './employeeQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useCurrentEmployee = () => {
	const { employees } = useRequests();
	return useQuery({
		queryKey: employeeQueryKeys.current,
		queryFn: async () => {
			const res = await employees.getCurrent();
			if (!res.ok) throw res;
			return res;
		},
	});
};
