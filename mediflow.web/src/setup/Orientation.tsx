import { useIsMobile } from '@/hooks/use-mobile';
import { FC, ReactNode, useEffect } from 'react';

interface OrientationProps {
  children: ReactNode;
}

const Orientation: FC<OrientationProps> = ({ children }) => {
  const isMobile = useIsMobile();
  useEffect(() => {
    if (isMobile) (screen.orientation as any).lock('landscape');
  }, []);

  return <>{children}</>;
};

export default Orientation;
