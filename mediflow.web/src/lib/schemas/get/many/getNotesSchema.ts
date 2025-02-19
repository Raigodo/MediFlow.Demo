import { z } from 'zod';
import { getNoteSchema } from '../one/getNoteSchema';

export const getNotesSchema = z.array(getNoteSchema);
