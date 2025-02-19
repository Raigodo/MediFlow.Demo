import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { BookOpenIcon } from 'lucide-react';
import Link from 'next/link';
import { useParams, usePathname } from 'next/navigation';

function JournalToolbarItem() {
  const { clientId } = useSelectedClient();
  const route = clientId ? RouteNames.Notes(clientId) : RouteNames.JournalIndex;
  const pathname = usePathname();
  const isActive = pathname.includes(RouteNames.JournalIndex);
  return (
    <div className="w-full">
      <Tooltip>
        <TooltipTrigger asChild>
          <Link href={route}>
            <Button
              variant={'ghost'}
              className={`${isActive && 'bg-secondary'}`}
            >
              <BookOpenIcon />
            </Button>
          </Link>
        </TooltipTrigger>
        <TooltipContent side="right">
          <p>Dienas ierakstu žurnāls</p>
        </TooltipContent>
      </Tooltip>
    </div>
  );
}

export default JournalToolbarItem;
