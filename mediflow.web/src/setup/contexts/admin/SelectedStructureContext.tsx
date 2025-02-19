'use client';

import { StructureEntity } from '@/lib/domain/entities/StructureEntity';
import { createContext, ReactNode, useContext, useState } from 'react';

type SelectedStructureContext = {
  structure: StructureEntity | null;
  selectStructure: (params: { structure: StructureEntity }) => void;
  resetSelection: () => void;
};

const Context = createContext<SelectedStructureContext | undefined>(undefined);

const SelectedStructureContextProvider = ({
  children,
}: {
  children: ReactNode;
}) => {
  const [structure, setStructure] =
    useState<SelectedStructureContext['structure']>(null);
  const selectStructure: SelectedStructureContext['selectStructure'] = ({
    structure,
  }) => {
    setStructure(structure);
  };
  const resetSelection = () => {
    setStructure(null);
  };
  return (
    <Context.Provider
      value={{
        structure,
        selectStructure,
        resetSelection,
      }}
    >
      {children}
    </Context.Provider>
  );
};

export default SelectedStructureContextProvider;

export const useSelectedStructure = () => {
  const context = useContext(Context);
  if (!context) throw Error('no selected structure context');
  return context;
};
