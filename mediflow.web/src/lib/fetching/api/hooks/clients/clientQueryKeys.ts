export const clientQueryKeys = {
	all: ['clients'],
	one: (req: { clientId: string }) => ['clients', `clients:${req.clientId}`],
};
