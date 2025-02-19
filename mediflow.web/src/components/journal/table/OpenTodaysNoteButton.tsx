import { Button } from '@/components/ui/button';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { RouteNames } from '@/lib/RouteNames';
import Link from 'next/link';

function OpenTodaysNoteButton() {
  const { isCurrentRole } = useSelectedEmployeeRoleScope();
  const { clientId } = useSelectedClient();
  const { employeeRoleScope: employeeRole } = useSelectedEmployeeRoleScope();
  return (
    <>
      {isCurrentRole && clientId && employeeRole && (
        <Link href={RouteNames.TodaysNote(clientId)}>
          <Button className="bg-secondary/90 hover:bg-primary size-full">
            Šīsdienas ieraksts
          </Button>
        </Link>
      )}
    </>
  );
}

export default OpenTodaysNoteButton;
