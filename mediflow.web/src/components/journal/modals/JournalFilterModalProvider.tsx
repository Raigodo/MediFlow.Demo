import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Label } from '@/components/ui/label';
import { JournalModalKeys } from '@/lib/modal-keys/JournalModalKeys';
import { useSelectedEmployeeRoleScope } from '@/setup/contexts/app/SelectedEmployeeRoleScopeContext';
import { useNoteFilter } from '@/setup/contexts/journal/NoteFilterContext';
import ModalWrapper, { ModalWrapperItem } from '@/setup/ModalWrapper';
import { useEffect, useState } from 'react';
import EmployeeSelect from '../table/filter/CreatorSelect';
import { DatePicker } from '@/components/common/DatePicker';
import ImportanceFlagSelect from '../table/filter/ImportanceFlagSelect';

const JournalFilterModalProvider = () => {
  return (
    <ModalWrapper
      modalKey={JournalModalKeys.FilterModal}
      modalComponent={JournalFilterModal}
    />
  );
};

export default JournalFilterModalProvider;

const JournalFilterModal: ModalWrapperItem = ({ isOpen, onClose }) => {
  const { employeeRoleScope } = useSelectedEmployeeRoleScope();
  const { dateFrom, dateTo, employeeId, flagState, updateFilter } =
    useNoteFilter();

  const [tempFilter, setTempFilter] = useState({
    from: dateFrom ?? undefined,
    to: dateTo ?? undefined,
    flagState: flagState,
    employeeId,
  });
  const onDateFromChange = (from: Date | undefined) => {
    const temp = { ...tempFilter, from };
    if (from && temp.to && from > temp.to) temp.to = undefined;
    setTempFilter(temp);
  };
  const onDateToChange = (to: Date | undefined) => {
    const temp = { ...tempFilter, to };
    if (to && temp.from && to < temp.from) temp.from = undefined;
    setTempFilter(temp);
  };
  const handleApplyChanges = () => {
    updateFilter({
      ...tempFilter,
      dateFrom: tempFilter.from ?? null,
      dateTo: tempFilter.to ?? null,
    });
    onClose();
  };

  useEffect(() => {
    if (isOpen && !employeeRoleScope) {
      console.error('can not open filter without employee role scope');
      onClose();
    }
  }, [isOpen]);

  if (!employeeRoleScope) return null;

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(value) => value !== isOpen && !value && onClose()}
    >
      <DialogContent className="grid grid-rows-[auto,1fr] w-fit">
        <DialogHeader>
          <DialogTitle>Ierakstu filtrs</DialogTitle>
          <DialogDescription>
            Iestatīt ierakstu filtrācijas parametrus
          </DialogDescription>
        </DialogHeader>
        <div>
          <div className="gap-4 grid grid-cols-[1fr_1fr]">
            <div className="flex flex-col gap-2">
              <Label>Sākot no datuma</Label>
              <DatePicker
                date={tempFilter.from}
                onSelected={onDateFromChange}
              />
            </div>
            <div className="flex flex-col gap-2">
              <Label>Līdz datumam</Label>
              <DatePicker date={tempFilter.to} onSelected={onDateToChange} />
            </div>
          </div>
        </div>
        <div className="gap-4 grid grid-cols-[1fr_1fr]">
          <div className="flex flex-col gap-2">
            <Label>Darbinieks</Label>
            <EmployeeSelect
              employeeId={employeeId}
              onChange={(employee) => {
                setTempFilter({
                  ...tempFilter,
                  employeeId: employee?.id ?? null,
                });
              }}
            />
          </div>
          <div className="flex flex-col gap-2">
            <Label>Svarīgums</Label>
            <ImportanceFlagSelect
              value={tempFilter.flagState}
              onValueChange={(flagState) =>
                setTempFilter({ ...tempFilter, flagState })
              }
            />
          </div>
        </div>

        <div>
          <div className="flex justify-center gap-6">
            <Button
              onClick={handleApplyChanges}
              className="shadow-md mt-2 w-[60%]"
            >
              Pielietot
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};
