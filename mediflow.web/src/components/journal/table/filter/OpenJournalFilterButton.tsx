import { SlidersHorizontalIcon } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { useModalManager } from '@/setup/contexts/core/ModalManagerContext';
import { JournalModalKeys } from '@/lib/modal-keys/JournalModalKeys';

function OpenJournalFilterButton() {
  const { open } = useModalManager();
  const handleClick = () => {
    open({ modalKey: JournalModalKeys.FilterModal });
  };
  return (
    <TooltipProvider>
      <Tooltip>
        <TooltipTrigger asChild>
          <Button variant={'ghost'} onClick={handleClick}>
            <SlidersHorizontalIcon /> Filtrēt
          </Button>
        </TooltipTrigger>
        <TooltipContent side="bottom">
          <p>Filtrēt ierakstus</p>
        </TooltipContent>
      </Tooltip>
    </TooltipProvider>
  );
}

export default OpenJournalFilterButton;
