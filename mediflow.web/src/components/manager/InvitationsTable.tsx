import { useInvitations } from '@/lib/fetching/api/hooks/invitations/useInvitations';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import LoadingInvitations from './placeholders/LoadingInvitations';
import LoadingInvitationsError from './placeholders/LoadingInvitationsError';
import NoInvitationsYet from './placeholders/NoInvitationsYet';
import { Button } from '@/components/ui/button';
import { CopyIcon } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from '@/components/ui/tooltip';

function InvitationsTable() {
  const { toast } = useToast();
  const { data, isLoading, isError, isSuccess } = useInvitations();
  const copyInvitationToken = (invitationToken: string) => {
    navigator.clipboard
      .writeText(invitationToken)
      .then(() => {
        toast({ title: 'Ielūgums nokopēts' });
      })
      .catch((err) => {
        console.error('Failed to copy text: ', err);
        toast({
          variant: 'destructive',
          title: 'neizdevās nokopēt tekstu',
        });
      });
  };
  return (
    <>
      {isLoading && <LoadingInvitations />}
      {!isLoading && isError && <LoadingInvitationsError />}
      {!isLoading && isSuccess && data.body.length <= 0 && <NoInvitationsYet />}
      {!isLoading && isSuccess && data.body.length > 0 && (
        <Table>
          <TableHeader className="bg-muted">
            <TableRow className="hover:bg-transparent">
              <TableHead className="w-[200px]">Izveidošanas datums</TableHead>
              <TableHead className="w-[200px]">Amats</TableHead>
              <TableHead>Ielūguma kods</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TooltipProvider>
              {data.body.map((invitaion) => (
                <TableRow key={invitaion.id}>
                  <TableCell>
                    {invitaion.issuedAt.toLocaleString('lv-LV', {
                      dateStyle: 'medium',
                    })}
                  </TableCell>
                  <TableCell>{invitaion.employeeRole}</TableCell>
                  <TableCell className="relative flex justify-between items-center gap-2">
                    <p>{invitaion.id}</p>
                    <div className="right-2 absolute">
                      <Tooltip>
                        <TooltipTrigger asChild>
                          <Button
                            size={'icon'}
                            variant={'ghost'}
                            onClick={() => copyInvitationToken(invitaion.id)}
                          >
                            <CopyIcon />
                          </Button>
                        </TooltipTrigger>
                        <TooltipContent>Nokopēt ielūgumu</TooltipContent>
                      </Tooltip>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TooltipProvider>
          </TableBody>
        </Table>
      )}
    </>
  );
}

export default InvitationsTable;
