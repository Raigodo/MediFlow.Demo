import { CirclePlusIcon } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';
import { useManagers } from '@/lib/fetching/api/hooks/users/useManagers';
import { UserEntity } from '@/lib/domain/entities/UserEntity';
import { useOwnedStructures } from '@/lib/fetching/api/hooks/structures/useOwnedStructures';
import NoManagersYet from './placeholders/NoManagersYet';
import { AdminModalKeys } from '@/lib/modal-keys/AdminModalKeys';
import ManagerNoStructureYet from './placeholders/ManagerNoStructureYet';
import { useSelectedStructure } from '@/setup/contexts/admin/SelectedStructureContext';
import React from 'react';
import LoadingSidelistData from '@/components/layout/placeholders/LoadingSidelistData';
import { useSelectedUser } from '@/setup/contexts/app/SelectedUserContext';

function ManagersSidelist() {
  const { data, isError, isSuccess, isLoading } = useManagers();
  const modalManager = useModalManager();
  return (
    <>
      <div className="mr-2 pt-1 pb-2 border-muted border-b">
        <button
          className="group flex justify-start gap-1 hover:bg-transparent pl-0.5 font-semibold text-gray-400 hover:text-foreground transition-colors"
          onClick={() => {
            modalManager.open({
              modalKey: AdminModalKeys.CreateUser,
            });
          }}
        >
          <div className="group-hover:text-primary text-gray-400 translate-y-[0.5px]">
            <CirclePlusIcon />
          </div>
          Pievienot lietotāju
        </button>
      </div>
      <div className="flex flex-col gap-1.5 px-1 overflow-y-auto scrollbar-thin scrollbar-track-transparent">
        {isLoading && <LoadingSidelistData />}
        {!isLoading && isError && <>Error...</>}
        {!isLoading && isSuccess && <ManagersVList managers={data.body} />}
      </div>
    </>
  );
}

export default ManagersSidelist;

const ManagersVList = ({ managers }: { managers: UserEntity[] }) => {
  const { userId, selectUser } = useSelectedUser();
  if (managers.length <= 0) return <NoManagersYet />;

  return (
    <>
      {managers.map((manager) => {
        const isSelected = manager.id === userId;
        return (
          <React.Fragment key={manager.id}>
            <Button
              variant={'outline'}
              className={`flex w-full justify-start gap-3 overflow-hidden rounded-[0px] rounded-r-[0.3em] border-l-2 py-7 pl-2 text-start transition-colors transition-none hover:border-l-[3.5px] hover:bg-secondary hover:transition-all ${
                isSelected && 'border-l-[3px] bg-primary hover:bg-primary'
              }`}
              onClick={() => selectUser({ userId: manager.id })}
            >
              <div>
                <div>
                  {manager.name} {manager.surname}
                </div>
                <div className="font-normal">{manager.id}</div>
              </div>
            </Button>
            {isSelected && <ManagersVListItemSubitems manager={manager} />}
          </React.Fragment>
        );
      })}
    </>
  );
};

const ManagersVListItemSubitems = ({ manager }: { manager: UserEntity }) => {
  const { structure: selectedStructure, selectStructure } =
    useSelectedStructure();
  const { data, isError, isSuccess, isLoading } = useOwnedStructures({
    managerId: manager.id,
  });
  return (
    <>
      {isLoading && <LoadingSidelistData />}
      {!isLoading && isError && <>Error...</>}
      {!isLoading && isSuccess && data.body.length <= 0 && (
        <ManagerNoStructureYet />
      )}
      {!isLoading && isSuccess && data.body.length > 0 && (
        <div className="flex flex-col gap-1 mb-2">
          {data.body.map((structure) => {
            const isSelected = structure.id === selectedStructure?.id;
            return (
              <div className="flex pl-8" key={structure.id}>
                <Button
                  variant="ghost"
                  className={`grow justify-start rounded-[0px] rounded-r-[0.3em] border-2 border-l-0 text-start ${
                    isSelected && 'bg-secondary'
                  } `}
                  onClick={() => selectStructure({ structure })}
                >
                  {structure.name}
                </Button>
              </div>
            );
          })}
        </div>
      )}
    </>
  );
};
