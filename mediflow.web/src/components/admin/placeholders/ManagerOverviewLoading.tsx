import { Loader2Icon } from 'lucide-react';

function ManagerOverviewLoading() {
  return (
    <div className="flex justify-center pt-8 size-full">
      <Loader2Icon className="animate-spin" size={20} />
    </div>
  );
}

export default ManagerOverviewLoading;
