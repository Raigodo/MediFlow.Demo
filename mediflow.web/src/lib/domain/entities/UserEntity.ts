import type { UserRoles } from '../values/UserRoles';

export type UserEntity = {
	id: string;
	role: UserRoles;
	name: string;
	surname: string;
	email: string;
};
