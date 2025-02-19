'use client';

import EmployeesTable from '@/components/employee/EmployeesTable';
import DeleteStructureButton from '@/components/manager/DeleteStructureButton';
import InvitationsTable from '@/components/manager/InvitationsTable';
import TrustDeviceButton from '@/components/manager/TrustDeviceButton';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { ManagerModalKeys } from '@/lib/modal-keys/ManagerModalKeys';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';

function Index() {
  const { open } = useModalManager();
  return (
    <>
      <div className="flex gap-2 grid-cols-3 bg-transparent *:bg-muted mt-2.5 px-2.5 w-fit *:h-8 *:grow">
        <Button
          variant={'ghost'}
          className="hover:bg-primary"
          onClick={() => {
            open({ modalKey: ManagerModalKeys.CreateInvitation });
          }}
        >
          Izveidot ielūgumu
        </Button>
        <Button
          variant={'ghost'}
          className="hover:bg-primary"
          onClick={() => {
            open({ modalKey: ManagerModalKeys.CreateStructure });
          }}
        >
          Izveidot struktūrvienūbu
        </Button>
        <TrustDeviceButton />
        <DeleteStructureButton />
      </div>
      <div className="*:flex *:flex-col gap-12 *:gap-4 grid mt-3 px-8 pt-6 pb-10">
        <div>
          <Label>Aktīvie ielūgimi</Label>
          <InvitationsTable />
        </div>
        <div>
          <Label>Darbinieki</Label>
          <EmployeesTable />
        </div>
      </div>
    </>
  );
}

export default Index;
