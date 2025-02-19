import { buttonVariants } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { RouteNames } from '@/lib/RouteNames';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { HomeIcon } from 'lucide-react';
import Link from 'next/link';

function LogoToolbarItem() {
  const { isAdmin } = useUserSession();
  const homeRoute = isAdmin ? RouteNames.AdminHome : RouteNames.Home;
  return (
    <div className="w-full">
      <Tooltip>
        <TooltipTrigger asChild>
          <Link
            href={homeRoute}
            className={buttonVariants({ variant: 'default' })}
          >
            <HomeIcon />
          </Link>
        </TooltipTrigger>
        <TooltipContent side="right">
          <p>Uz sākumlapu</p>
        </TooltipContent>
      </Tooltip>
    </div>
  );
}

export default LogoToolbarItem;
