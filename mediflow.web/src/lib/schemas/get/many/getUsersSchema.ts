import { z } from 'zod';
import { getUserSchema } from '../one/getUserSchema';

export const getUsersSchema = z.array(getUserSchema);
