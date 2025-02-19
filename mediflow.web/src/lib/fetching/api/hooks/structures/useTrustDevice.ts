import { useRequests } from '@/setup/contexts/core/RequestsContext';
import { useMutation } from '@tanstack/react-query';

type params = Parameters<
	ReturnType<typeof useRequests>['strucures']['trustDevice']
>[0];

export const useTrustDevice = () => {
	const { strucures } = useRequests();
	return useMutation({
		mutationFn: async (req: params) => {
			return strucures.trustDevice(req);
		},
	});
};
