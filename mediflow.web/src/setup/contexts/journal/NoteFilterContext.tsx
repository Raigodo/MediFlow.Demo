'use client';

import { NoteFilterFlagStates } from '@/lib/domain/values/NoteFilterFlagStates';
import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useState,
} from 'react';
import { useSelectedEmployeeRoleScope } from '../app/SelectedEmployeeRoleScopeContext';

type NoteFilterParameters = {
  dateFrom: Date | null;
  dateTo: Date | null;
  employeeId: string | null;
  flagState: NoteFilterFlagStates;
};
type NoteFilterMetadata = {
  isTodayIncluded: boolean;
  hasAnyConstraint: boolean;
};
type NoteFilterActions = {
  updateFilter: (params: NoteFilterParameters) => void;
};

type SelectedClientContext = NoteFilterParameters &
  NoteFilterMetadata &
  NoteFilterActions;

const checkIsTodayIncluded = (date: Date | null) => {
  if (!date) return true;
  const today = new Date();
  return (
    date.getUTCFullYear() === today.getUTCFullYear() &&
    date.getUTCMonth() === today.getUTCMonth() &&
    date.getUTCDate() === today.getUTCDate()
  );
};

const Context = createContext<SelectedClientContext | undefined>(undefined);

const NoteFilterContextProvider = ({ children }: { children: ReactNode }) => {
  const [filter, setFilter] = useState<NoteFilterParameters>({
    dateFrom: null,
    dateTo: null,
    employeeId: null,
    flagState: NoteFilterFlagStates.Any,
  });

  const metadata: NoteFilterMetadata = {
    isTodayIncluded: checkIsTodayIncluded(filter.dateTo),
    hasAnyConstraint:
      filter.dateFrom !== null ||
      filter.dateTo !== null ||
      filter.employeeId !== null ||
      filter.flagState !== NoteFilterFlagStates.Any,
  };

  const updateFilter: SelectedClientContext['updateFilter'] = ({
    dateFrom,
    dateTo,
    ...rest
  }) => {
    if (dateFrom && dateTo && dateFrom > dateTo)
      throw Error('dates doesnt intersect (invalid filter state)');
    setFilter({ dateFrom, dateTo, ...rest });
  };

  const { employeeRoleScope } = useSelectedEmployeeRoleScope();
  useEffect(() => {
    setFilter({ ...filter, employeeId: null });
  }, [employeeRoleScope]);

  return (
    <Context.Provider
      value={{
        ...filter,
        ...metadata,
        updateFilter,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export default NoteFilterContextProvider;

export const useNoteFilter = () => {
  const context = useContext(Context);
  if (!context) throw Error('no note filter context');
  return context;
};
