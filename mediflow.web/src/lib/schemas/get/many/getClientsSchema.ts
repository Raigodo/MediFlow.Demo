import { z } from 'zod';
import { getClientSchema } from '../one/getClientSchema';

export const getClientsSchema = z.array(getClientSchema);
