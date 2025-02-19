import { useState } from 'react';
import { Button } from '@/components/ui/button';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { EmployeeEntity } from '@/lib/domain/entities/EmployeeEntity';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { useEmployees } from '@/lib/fetching/api/hooks/employees/useEmployees';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { EmployeeRoles } from '@/lib/domain/values/EmployeeRoles';
import { Separator } from '@/components/ui/separator';
import LoadingCreators from '../../placeholders/LoadingCreators';
import LoadingCreatorsError from '../../placeholders/LoadingCreatorsError';
import { useEmployee } from '@/lib/fetching/api/hooks/employees/useEmployee';

const CreatorSelect = CreatorSelectGuarded;
export default CreatorSelect;

function CreatorSelectGuarded({
  employeeId,
  onChange,
}: {
  employeeId: string | null;
  onChange: (value: EmployeeEntity | null) => void;
}) {
  const { employeeRoleScope } = useSelectedEmployeeRoleScope();
  if (!employeeRoleScope) {
    throw Error('can not open creator select without employee role selected');
  }
  if (employeeId) {
    return (
      <CreatorSelectWithEmployeeSelected
        employeeId={employeeId}
        employeeRoleScope={employeeRoleScope}
        onChange={onChange}
      />
    );
  }
  return (
    <CreatorSelectLoading
      selectedEmployee={null}
      employeeRoleScope={employeeRoleScope}
      onChange={onChange}
    />
  );
}

const CreatorSelectWithEmployeeSelected = ({
  employeeId,
  employeeRoleScope,
  onChange,
}: {
  employeeId: string;
  employeeRoleScope: EmployeeRoles;
  onChange: (value: EmployeeEntity | null) => void;
}) => {
  const { data, isLoading, isError, isSuccess } = useEmployee(employeeId);
  return (
    <>
      {isLoading && <>Loading...</>}
      {!isLoading && isError && <>Error...</>}
      {!isLoading && isSuccess && !data.body && <>Error...</>}
      {!isLoading && isSuccess && data.body && (
        <CreatorSelectLoading
          selectedEmployee={data.body}
          employeeRoleScope={employeeRoleScope}
          onChange={onChange}
        />
      )}
    </>
  );
};

function CreatorSelectLoading({
  selectedEmployee,
  employeeRoleScope,
  onChange,
}: {
  selectedEmployee: EmployeeEntity | null;
  employeeRoleScope: EmployeeRoles;
  onChange: (value: EmployeeEntity | null) => void;
}) {
  const { data, isLoading, isError, isSuccess } = useEmployees();
  const [value, setValue] = useState(selectedEmployee);
  const [open, setOpen] = useState(false);
  const handleCreatorSelect = (employee: EmployeeEntity | null) => {
    setValue(employee);
    setOpen(false);
    onChange(employee);
  };
  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button variant={'outline'} role="combobox" aria-expanded={open}>
          {value ? `${value.name} ${value.surname}` : 'Izvēlēties darbinieku'}
        </Button>
      </PopoverTrigger>
      <PopoverContent>
        <Command className="rounded-none">
          <CommandInput placeholder="Meklēt darbinieku..." />
          <CommandList>
            <div className="scrollbar-thumb-border h-fit max-h-72 overflow-y-auto scrollbar-thin scrollbar-track-transparent">
              {isLoading && <LoadingCreators />}
              {!isLoading && isError && <LoadingCreatorsError />}
              {!isLoading && isSuccess && (
                <EmployeeSelectListItems
                  employees={data.body}
                  onCreatorSelected={handleCreatorSelect}
                  employeeRoleScope={employeeRoleScope}
                />
              )}
            </div>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}

function EmployeeSelectListItems({
  employees,
  employeeRoleScope,
  onCreatorSelected,
}: {
  employees: EmployeeEntity[];
  employeeRoleScope: EmployeeRoles;
  onCreatorSelected: (employee: EmployeeEntity | null) => void;
}) {
  const [activeEmployees, inactiveEmployees] = employees.reduce<
    [EmployeeEntity[], EmployeeEntity[]]
  >(
    ([active, inactive], employee) => {
      if (employee.role !== employeeRoleScope) return [active, inactive];

      if (employee.unassignedOn) {
        inactive.push(employee);
      } else {
        active.push(employee);
      }
      return [active, inactive];
    },
    [[], []],
  );
  return (
    <>
      <CommandEmpty>Netika atrasts neviens darbinieks</CommandEmpty>
      <CommandGroup heading="Cits">
        <CommandItem
          onSelect={() => onCreatorSelected(null)}
          className="hover:cursor-pointer"
        >
          Atmest
        </CommandItem>
      </CommandGroup>
      {activeEmployees.length > 0 && (
        <CommandGroup heading="Aktīvie darbinieki">
          {activeEmployees.map((employee) => (
            <CommandItem
              key={employee.id}
              onSelect={() => onCreatorSelected(employee)}
              className="justify-start w-full hover:cursor-pointer"
            >
              {employee.name} {employee.surname}
            </CommandItem>
          ))}
        </CommandGroup>
      )}
      {activeEmployees.length > 0 && inactiveEmployees.length > 0 && (
        <Separator />
      )}
      {inactiveEmployees.length > 0 && (
        <CommandGroup heading="Bijušie darbinieki">
          {inactiveEmployees.map((employee) => (
            <CommandItem
              key={employee.id}
              onSelect={() => onCreatorSelected(employee)}
            >
              {employee.name} {employee.surname}
            </CommandItem>
          ))}
        </CommandGroup>
      )}
    </>
  );
}
