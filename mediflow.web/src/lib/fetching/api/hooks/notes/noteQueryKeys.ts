import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { NoteFilterFlagStates } from '@/lib/domain/values/NoteFilterFlagStates';

export const noteQueryKeys = {
	all: ['notes'],
	one: (req: { clientId: string; noteId: string }) => [
		'notes',
		`notes:${req.noteId}`,
		'clients',
		`clients:${req.clientId}`,
	],
	many: (req: {
		clientId: string;
		role: EmployeeRoles;
		from: Date | null;
		to: Date | null;
		flag: NoteFilterFlagStates;
		employeeId: string | null;
	}) => {
		return [
			'notes',
			`notes:filter={from=${
				req.from?.toISOString().split('T')[0] ?? null
			} to=${req.to?.toISOString().split('T')[0] ?? null} flag=${
				req.flag
			} role=${req.role} creator=${req.employeeId}}`,
			'clients',
			`clients:${req.clientId}`,
		];
	},
	todays: (req: { clientId: string; employeeRole: EmployeeRoles }) => [
		'notes',
		`notes:{role=${req.employeeRole}}`,
		'clients',
		`clients:${req.clientId}`,
	],
};
