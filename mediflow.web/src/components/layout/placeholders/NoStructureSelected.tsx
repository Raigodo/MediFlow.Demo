import { FC, ReactNode } from 'react';

interface NoStructureSelectedProps {
  children: ReactNode;
}

const NoStructureSelected: FC<NoStructureSelectedProps> = () => {
  return (
    <div className="flex justify-center pt-8 size-full">
      Nav izvēlēta struktūra
    </div>
  );
};

export default NoStructureSelected;
