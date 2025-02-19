import { z } from 'zod';
import { getStructureSchema } from '../one/getStructureSchema';

export const getStructuresSchema = z.array(getStructureSchema);
