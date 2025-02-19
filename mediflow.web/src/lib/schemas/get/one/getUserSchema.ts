import { UserRoles } from '@/lib/domain/values/UserRoles';
import { z } from 'zod';
export type UserEntity = {
	id: string;
	role: UserRoles;
	name: string;
	surname: string;
	email: string;
};

export const getUserSchema = z.object({
	id: z.string(),
	role: z.nativeEnum(UserRoles),
	name: z.string(),
	surname: z.string(),
	email: z.string().email(),
});
