import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { RouteNames } from '@/lib/RouteNames';
import { useSelectedClient } from '@/setup/contexts/app/SelectedClientContext';
import { UserRoundIcon } from 'lucide-react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

function ClientToolbarItem() {
  const { clientId } = useSelectedClient();
  const route = clientId
    ? RouteNames.Client(clientId)
    : RouteNames.ClientsIndex;
  const pathname = usePathname();
  const isActive = pathname.startsWith(RouteNames.ClientsIndex);
  return (
    <div className="w-full">
      <Tooltip>
        <TooltipTrigger asChild>
          <Link href={route}>
            <Button
              variant={'ghost'}
              className={`${isActive && 'bg-secondary'}`}
            >
              <UserRoundIcon />
            </Button>
          </Link>
        </TooltipTrigger>
        <TooltipContent side="right">
          <p>Klienta dati</p>
        </TooltipContent>
      </Tooltip>
    </div>
  );
}

export default ClientToolbarItem;
