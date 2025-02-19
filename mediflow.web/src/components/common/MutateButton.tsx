import { FC, ReactNode } from 'react';
import { Button, ButtonProps } from '@/components/ui/button';
import { Loader2Icon } from 'lucide-react';
import { cn } from '@/lib/utils';

interface MutateButtonProps extends ButtonProps {
  children: ReactNode;
  loading: boolean;
}

const MutateButton: FC<MutateButtonProps> = ({
  children,
  loading,
  disabled,
  className,
  ...props
}) => {
  return (
    <Button
      className={cn('w-full grid grid-cols-1 grid-rows-1', className)}
      type="button"
      disabled={loading || disabled}
      {...props}
    >
      <div
        className={`col-start-1 row-start-1 place-self-center ${
          loading && '-translate-x-[100vw]'
        }`}
      >
        {children}
      </div>
      {loading && (
        <div className="place-self-center col-start-1 row-start-1">
          <Loader2Icon className="animate-spin" size={20} />
        </div>
      )}
    </Button>
  );
};

export default MutateButton;
