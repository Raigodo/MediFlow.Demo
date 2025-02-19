import { useEmployees } from '@/lib/fetching/api/hooks/employees/useEmployees';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import LoadingEmployees from './placeholders/LoadingEmployees';
import LoadingEmployeesError from './placeholders/LoadingEmployeesError';
import NoEmployeesYet from './placeholders/NoEmployeesYet';
import { ScrollArea } from '@/components/ui/scroll-area';

function EmployeesTable() {
  const { data, isLoading, isSuccess, isError } = useEmployees();
  return (
    <ScrollArea>
      <Table>
        <TableHeader className="bg-muted">
          <TableRow className="hover:bg-transparent">
            <TableHead className="w-[200px]">Pievienošanās datums</TableHead>
            <TableHead className="w-[200px]">Amats</TableHead>
            <TableHead>Vārds uzvārds</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {isLoading && (
            <TableRow>
              <TableCell colSpan={3}>
                <LoadingEmployees />
              </TableCell>
            </TableRow>
          )}
          {!isLoading && isError && (
            <TableRow>
              <TableCell colSpan={3}>
                <LoadingEmployeesError />
              </TableCell>
            </TableRow>
          )}
          {!isLoading && isSuccess && data.body.length <= 0 && (
            <TableRow>
              <TableCell colSpan={3}>
                <NoEmployeesYet />
              </TableCell>
            </TableRow>
          )}
          {!isLoading && isSuccess && data.body.length > 0 && (
            <>
              {data.body.map((employee, i) => (
                <TableRow key={i}>
                  <TableCell>
                    {employee.assignedOn.toLocaleString('lv-LV', {
                      dateStyle: 'medium',
                    })}
                  </TableCell>
                  <TableCell>{employee.role}</TableCell>
                  <TableCell className="flex justify-between gap-2">
                    <p>
                      {employee.name} {employee.surname}
                    </p>
                  </TableCell>
                </TableRow>
              ))}
            </>
          )}
        </TableBody>
      </Table>
    </ScrollArea>
  );
}

export default EmployeesTable;
