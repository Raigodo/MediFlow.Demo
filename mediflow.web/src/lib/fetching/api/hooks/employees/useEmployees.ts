import { useQuery } from '@tanstack/react-query';
import { employeeQueryKeys } from './employeeQueryKeys';
import { useRequests } from '@/setup/contexts/core/RequestsContext';

export const useEmployees = () => {
	const { employees } = useRequests();
	return useQuery({
		queryKey: employeeQueryKeys.all,
		queryFn: async () => {
			const res = await employees.getAll();
			if (!res.ok) throw res;
			return res;
		},
	});
};
