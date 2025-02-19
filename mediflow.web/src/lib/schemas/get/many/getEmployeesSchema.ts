import { z } from 'zod';
import { getEmployeeSchema } from '../one/getEmployeeSchema';

export const getEmployeesSchema = z.array(getEmployeeSchema);
