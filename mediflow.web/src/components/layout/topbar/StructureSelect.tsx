import { Button } from '@/components/ui/button';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Separator } from '@/components/ui/separator';
import { StructureEntity } from '@/lib/domain/entities/StructureEntity';
import { useCurrentStructure } from '@/lib/fetching/api/hooks/structures/useCurrentStructure';
import { useParticipatingStructures } from '@/lib/fetching/api/hooks/structures/useParticipatingStructures';
import { ManagerModalKeys } from '@/lib/modal-keys/ManagerModalKeys';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';

const StructureSelect = () => {
  const participatingQuery = useParticipatingStructures();
  const currentQuery = useCurrentStructure();
  const isLoading = participatingQuery.isLoading || currentQuery.isLoading;
  const isSuccess =
    participatingQuery.isSuccess &&
    (currentQuery.isSuccess ||
      (currentQuery.error && (currentQuery.error as any)?.status === 404));
  const isError =
    participatingQuery.isError ||
    (currentQuery.isError &&
      currentQuery.error &&
      (currentQuery.error as any)?.status !== 404);

  return (
    <>
      {isLoading && <>Loading...</>}
      {!isLoading && isError && <>Error...</>}
      {!isLoading && isSuccess && (
        <>
          {participatingQuery.data ? (
            <RenderStructureSelect
              participating={participatingQuery.data.body}
              current={currentQuery.data?.body ?? null}
            />
          ) : (
            <>No structure</>
          )}
        </>
      )}
    </>
  );
};

export default StructureSelect;

const RenderStructureSelect = ({
  participating,
  current,
}: {
  participating: StructureEntity[];
  current: StructureEntity | null;
}) => {
  const { open } = useModalManager();
  const { alter } = useUserSession();
  return (
    <Select
      value={current?.id}
      onValueChange={(value) => alter({ structureId: value })}
    >
      <SelectTrigger className="h-6">
        <SelectValue placeholder="izvēlēties struktūrvienību" />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectLabel className="px-2">Pieejamās struktūrvienības</SelectLabel>
          {participating.length > 0 &&
            participating.map((structure) => (
              <SelectItem value={structure.id} key={structure.id}>
                {structure.name}
              </SelectItem>
            ))}
          {participating.length <= 0 && (
            <p className="mt-2 mb-4 px-2 text-sm text-center">
              Nav nevienas struktūrvienības
            </p>
          )}
          <Separator className="mx-auto my-2 w-[80%]" />
          <Button
            variant={'ghost'}
            className="w-full font-normal"
            onClick={() =>
              open({
                modalKey: ManagerModalKeys.CreateStructure,
              })
            }
          >
            Izveidot struktūrvienību?
          </Button>
        </SelectGroup>
      </SelectContent>
    </Select>
  );
};
