export const invitationQueryKeys = {
	all: ['invitations'],
	one: (req: { invitationId: string }) => [
		'invitations',
		`invitations:${req.invitationId}`,
	],
};
