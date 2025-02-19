import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { RouteNames } from '@/lib/RouteNames';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import Link from 'next/link';
import { FC, ReactNode } from 'react';

interface UserDropdownProps {
  children: ReactNode;
}

const UserDropdown: FC<UserDropdownProps> = ({ children }) => {
  const { logOut } = useUserSession();

  return (
    <DropdownMenu>
      <DropdownMenuTrigger className="flex border border-border rounded-full h-12 aspect-square overflow-hidden">
        {children}
      </DropdownMenuTrigger>
      <DropdownMenuContent className="min-w-48">
        <DropdownMenuGroup>
          <DropdownMenuLabel>Sesijas darbības</DropdownMenuLabel>
          <DropdownMenuItem asChild>
            <Link href={RouteNames.CurrentProfile}>Profils</Link>
          </DropdownMenuItem>
          <DropdownMenuItem asChild>
            <Link href={RouteNames.CurrentStructure}>Struktūvienība</Link>
          </DropdownMenuItem>
          <DropdownMenuItem onClick={() => logOut()}>Iziet</DropdownMenuItem>
        </DropdownMenuGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export default UserDropdown;
