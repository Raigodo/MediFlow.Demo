export const structureQueryKeys = {
	all: ['structures'],
	one: (req: { structureId: string }) => [
		'structures',
		`structure:${req.structureId}`,
	],
	current: ['structures', 'structures:current'],
	owned: (managerId: string) => [
		'structures',
		`structures-owned:${managerId}`,
	],
	participating: ['structures', 'structures-participating'],
};
