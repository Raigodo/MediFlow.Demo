export const employeeQueryKeys = {
	all: ['employees'],
	one: (req: { employeeId: string }) => [
		'employees',
		`employees:${req.employeeId}`,
	],
	current: ['employees', `employees:current`],
};
