import { NoteFilterFlagStates } from '@/lib/domain/values/NoteFilterFlagStates';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';

function ImportanceFlagSelect({
  value,
  onValueChange,
}: {
  value: NoteFilterFlagStates;
  onValueChange: (value: NoteFilterFlagStates) => void;
}) {
  return (
    <Select value={value} onValueChange={onValueChange}>
      <SelectTrigger>
        <SelectValue placeholder={'izvēlēties'} />
      </SelectTrigger>
      <SelectContent>
        <SelectItem value={NoteFilterFlagStates.Any}>
          {NoteFilterFlagStates.Any}
        </SelectItem>
        <SelectItem value={NoteFilterFlagStates.Flagged}>
          {NoteFilterFlagStates.Flagged}
        </SelectItem>
        <SelectItem value={NoteFilterFlagStates.Nonflagged}>
          {NoteFilterFlagStates.Nonflagged}
        </SelectItem>
      </SelectContent>
    </Select>
  );
}

export default ImportanceFlagSelect;
