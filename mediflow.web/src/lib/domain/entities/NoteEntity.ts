import type { EmployeeRoles } from '../values/EmployeeRoles';
import { LinkableItemTypes } from '../values/LinkableItemTypes';

export type NoteEntity = {
	id: string;
	clientId: string;
	content: string;
	isFlagged: boolean;
	createdOn: Date;
	linkedItems: Record<LinkableItemTypes, string[]>;
	creator: {
		id: string;
		role: EmployeeRoles;
		name: string | null;
		surname: string | null;
	};
};
