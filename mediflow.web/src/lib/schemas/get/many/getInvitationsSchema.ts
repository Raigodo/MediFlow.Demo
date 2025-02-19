import { z } from 'zod';
import { getInvitationSchema } from '../one/getInvitationSchema';

export const getInvitationsSchema = z.array(getInvitationSchema);
