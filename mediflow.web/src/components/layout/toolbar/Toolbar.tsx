import { GalleryHorizontalEndIcon, MonitorCogIcon } from 'lucide-react';
import LogoToolbarItem from './items/LogoToolbarItem';
import SettingsToolbarItem from './items/SettingsToolbarItem';
import { RouteNames } from '@/lib/RouteNames';
import ToolbarItem from './items/ToolbarItem';
import { useUserSession } from '@/setup/contexts/core/UserSessionContext';
import { Separator } from '@/components/ui/separator';
import { TooltipProvider } from '@/components/ui/tooltip';
import ClientToolbarItem from './items/ClientToolbarItem';
import JournalToolbarItem from './items/JournalToolbarItem';

function Toolbar() {
  const { isManager, isAdmin } = useUserSession();
  return (
    <div className="flex flex-col justify-between px-2 h-full">
      <div>
        <TooltipProvider>
          <div className="flex items-center h-16">
            <LogoToolbarItem />
          </div>
          <Separator className="mb-2 -translate-y-[1px]" />
          {!isAdmin && (
            <>
              <div className="gap-1 grid">
                <ClientToolbarItem />
                <JournalToolbarItem />
                {isManager && (
                  <>
                    <Separator />
                    <ToolbarItem
                      route={RouteNames.Manage}
                      icon={MonitorCogIcon}
                      title={'Pārvaldīt struktūru'}
                    />
                  </>
                )}
              </div>
            </>
          )}
          {isAdmin && (
            <>
              <div className="gap-1 grid">
                <Separator className="my-2 bg-border" />
                <ToolbarItem
                  route={RouteNames.AdminStructure}
                  icon={GalleryHorizontalEndIcon}
                  title={'pārvaldīt lietotājus'}
                />
              </div>
            </>
          )}
        </TooltipProvider>
      </div>
      <SettingsToolbarItem />
    </div>
  );
}

export default Toolbar;
