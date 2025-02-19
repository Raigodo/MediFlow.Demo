import { useTrustDevice } from '@/lib/fetching/api/hooks/structures/useTrustDevice';
import { Button } from '@/components/ui/button';
import { useToast } from '@/hooks/use-toast';

function TrustDeviceButton() {
  const { mutateAsync: trustDeviceMutation } = useTrustDevice();
  const { toast } = useToast();

  const handleTrustDevice = async () => {
    const result = await trustDeviceMutation();
    if (result.ok) {
      toast({
        title: 'Ierīce veiksmīgi piesaistīta.',
        description: 'Šī ierīce ir piesaistīta pie tekošās struktūrvienības.',
      });
    } else {
      toast({
        variant: 'destructive',
        title: 'Neizdevās piesaistīt ierīci',
        description:
          'Ierīces piesaistīšana tekošajai struktūrvienībai neizdevās',
      });
    }
  };

  return (
    <Button
      variant={'ghost'}
      className="hover:bg-primary"
      onClick={handleTrustDevice}
    >
      Pievienot iekārtu
    </Button>
  );
}

export default TrustDeviceButton;
