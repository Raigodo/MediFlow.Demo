import type { InvalidityGroups } from '../values/InvalidityGroups';
import type { InvalidityFlags } from '../values/InvalidityTypes';

export type ClientEntity = {
	id: string;
	structureId: string;
	personalCode: string;
	birthDate: Date;
	name: string;
	surname: string;
	language: string;
	religion: string;
	invalidtiyGroup: InvalidityGroups;
	invalidityFlag: InvalidityFlags;
	invalidityExpiresOn: Date | null;
	joinedOn: Date;
	contacts: {
		id: string;
		title: string;
		phoneNumber: string;
	}[];
};
