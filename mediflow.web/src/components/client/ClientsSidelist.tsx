import { ClientEntity } from '@/lib/domain/entities/ClientEntity';
import { useClients } from '@/lib/fetching/api/hooks/clients/useClients';
import { CirclePlusIcon } from 'lucide-react';
import LoadingSidelistData from '@/components/layout/placeholders/LoadingSidelistData';
import { Button } from '@/components/ui/button';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';
import { AppModalKeys } from '@/lib/modal-keys/AppModalKeys';
import NoClientsYet from './placeholders/NoClientsYet';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { useToast } from '@/hooks/use-toast';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import React from 'react';
import { cn } from '@/lib/utils';
import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';

function ClientsSidelist({ className }: { className?: string }) {
  const modalManager = useModalManager();
  const { hasStructure } = useUserSession();
  const { toast } = useToast();
  const handleOpenCreateClientModal = () => {
    if (!hasStructure) {
      toast({
        variant: 'destructive',
        title: 'Nav izvēlēta neviena struktūrvienība',
        description:
          'Lai izveidotu jaunu klientu vispirms nepieciešams izvēlēties struktūrvienību kurā darbojamies.',
      });
      return;
    }
    modalManager.open({
      modalKey: AppModalKeys.CreateClient,
    });
  };
  return (
    <div className={cn('flex flex-col grow ', className)}>
      <div className="mr-2 pt-1 pb-2 border-muted border-b">
        <button
          className="group flex justify-start gap-1 hover:bg-transparent pl-0.5 w-full font-semibold text-secondary hover:text-foreground transition-colors"
          onClick={handleOpenCreateClientModal}
        >
          <div className="group-hover:text-primary text-secondary translate-y-[0.5px]">
            <CirclePlusIcon />
          </div>
          Pievienot klientu
        </button>
      </div>
      <div className="flex flex-col gap-1.5 px-1 scrollbar-thumb-border w-full overflow-y-auto scrollbar-thin scrollbar-track-transparent">
        {!hasStructure && <>Nav izvēlēta struktūra</>}
        {hasStructure && <ClientSidelistloading />}
      </div>
    </div>
  );
}

export default ClientsSidelist;

const ClientSidelistloading = () => {
  const { data, isError, isSuccess, isLoading } = useClients();
  return (
    <>
      {isLoading && <LoadingSidelistData />}
      {!isLoading && isError && <>Error...</>}
      {!isLoading && isSuccess && <ClientVList clients={data.body} />}
    </>
  );
};

const ClientVList = ({ clients }: { clients: ClientEntity[] }) => {
  const { clientId, selectClient } = useSelectedClient();
  if (clients.length <= 0) return <NoClientsYet />;
  const handleClientClick = (clientId: string) => {
    selectClient({ clientId });
  };

  return (
    <>
      {clients.map((client) => {
        const isSelected = clientId === client.id;
        return (
          <React.Fragment key={client.id}>
            <Button
              variant={'outline'}
              className={`flex w-full justify-start gap-3 overflow-hidden rounded-[0px] rounded-r-[0.3em] border-l-2 py-7 pl-2 text-start transition-colors transition-none hover:border-l-[3.5px] hover:bg-secondary hover:transition-all ${
                isSelected &&
                'border-l-[3px] bg-primary hover:bg-primary text-primary-foreground'
              }`}
              onClick={() => handleClientClick(client.id)}
            >
              <div>
                <div>
                  {client.name} {client.surname}
                </div>
                <div className="font-normal">{client.id}</div>
              </div>
            </Button>
            {isSelected && <ClientVListSubitems />}
          </React.Fragment>
        );
      })}
    </>
  );
};

const ClientVListSubitems = () => {
  const { employeeRoleScope, selectEmployeeRoleScope, accessibleRoles } =
    useSelectedEmployeeRoleScope();

  const handleRoleScopeClick = (role: EmployeeRoles) => {
    selectEmployeeRoleScope({ employeeRoleScope: role });
  };

  return (
    <div className="flex flex-col gap-1 mb-2">
      {accessibleRoles.map((role) => {
        const isCurrentRole = employeeRoleScope === role;
        return (
          <div className="flex pl-8" key={role}>
            <Button
              variant="ghost"
              className={`grow justify-start rounded-[0px] rounded-r-[0.3em] border-2 border-l-0 text-start ${
                isCurrentRole && 'bg-secondary text-secondary-foreground'
              } `}
              onClick={() => handleRoleScopeClick(role)}
            >
              {role}
            </Button>
          </div>
        );
      })}
    </div>
  );
};
