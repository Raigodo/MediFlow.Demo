import { Button } from '@/components/ui/button';
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip';
import { HomeIcon } from 'lucide-react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

function ToolbarItem({
  route,
  title,
  icon: Icon,
}: {
  icon: typeof HomeIcon;
  route: string;
  title: string;
}) {
  const pathname = usePathname();
  return (
    <div className="w-fit">
      <Tooltip>
        <TooltipTrigger asChild>
          <Link href={route}>
            <Button
              variant={'ghost'}
              className={`${pathname.startsWith(route) && 'bg-secondary'}`}
            >
              <Icon />
            </Button>
          </Link>
        </TooltipTrigger>
        <TooltipContent side="right">
          <p>{title}</p>
        </TooltipContent>
      </Tooltip>
    </div>
  );
}

export default ToolbarItem;
