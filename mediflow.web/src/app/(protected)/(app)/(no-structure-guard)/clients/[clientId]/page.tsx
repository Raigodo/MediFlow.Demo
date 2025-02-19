'use client';

import ClientNotFound from '@/components/client/placeholders/ClientNotFound';
import ClientOverviewLoading from '@/components/client/placeholders/ClientOverviewLoading';
import { Button } from '@/components/ui/button';
import { ClientEntity } from '@/lib/domain/entities/ClientEntity';
import { useClient } from '@/lib/fetching/api/hooks/clients/useClient';
import { RouteNames } from '@/lib/RouteNames';
import { PencilIcon } from 'lucide-react';
import { useParams, useRouter } from 'next/navigation';

function Index() {
  const { clientId } = useParams();
  const { data, isSuccess, isError, isLoading } = useClient(clientId as string);
  return (
    <div className="asdadsasd">
      {isLoading && <ClientOverviewLoading />}
      {!isLoading && isError && <ClientNotFound />}
      {!isLoading && isSuccess && !data.body && <ClientNotFound />}
      {!isLoading && isSuccess && data.body && (
        <ClientOverview client={data.body} />
      )}
    </div>
  );
}

export default Index;

function ClientOverview({ client }: { client: ClientEntity }) {
  const { push } = useRouter();
  const navigateToEdit = () => {
    push(RouteNames.EditClient(client.id));
  };
  return (
    <div className="flex">
      <div className="w-fit min-w-[500px] max-w-[calc(50vw)]">
        <div className="border-t border-r rounded-tr-sm w-full">
          <div className="flex flex-col gap-2 px-4 pt-4 border-l-[6px] border-l-primary">
            <h1 className="overflow-x-auto overflow-y-hidden text-4xl text-nowrap scrollbar-thin">
              {client.name} {client.surname}
            </h1>
            <h3 className="text-2xl">{client.id}</h3>
          </div>
          <div className="flex *:flex flex-col *:justify-between gap-1.5 pt-6 pr-6 pb-4 pl-14 *:border-b">
            <p>
              <span>Tika pievienots:</span>{' '}
              {client.joinedOn.toLocaleString('lv-LV', {
                dateStyle: 'medium',
              })}
            </p>
            <p>
              <span>Dzimšanas datums:</span>{' '}
              {client.birthDate.toLocaleString('lv-LV', {
                dateStyle: 'medium',
              })}
            </p>
            <p>
              <span>Parsonas kods:</span> {client.personalCode}
            </p>
            <p>
              <span>Valoda:</span> {client.language}
            </p>
            <p>
              <span>Religija:</span> {client.religion}
            </p>
            <p>
              <span>Invaliditātes grupa:</span> {client.invalidtiyGroup}
            </p>
            <p>
              <span>Pastāvīga invaliditāte:</span> {client.invalidityFlag}
            </p>
            <p>
              <span>Invaliditātes derīguma termiņš:</span>{' '}
              {client.invalidityExpiresOn
                ? client.invalidityExpiresOn.toLocaleString('lv-LV', {
                    dateStyle: 'medium',
                  })
                : 'none'}
            </p>
          </div>
        </div>
        <div className="mt-7 ml-14 pb-4 border-t border-r rounded-tr-sm w-[calc(100%-3.5rem)]">
          <div className="items-center px-2 py-1.5 border-l-4 border-l-primary">
            <h3 className="font-medium">Kontakti</h3>
          </div>
          <div className="flex flex-col gap-3 pt-2 pl-6">
            {(!client.contacts || client.contacts.length <= 0) && (
              <div>nav kontaktu</div>
            )}
            {client.contacts?.length > 0 &&
              client.contacts.map((contact) => (
                <div
                  className="*:flex *:justify-between pr-6 *:border-b"
                  key={contact.phoneNumber}
                >
                  <p>
                    <span>Kontaktpersona:</span> {contact.title}
                  </p>
                  <p>
                    <span>Telefona numurs:</span> {contact.phoneNumber}
                  </p>
                </div>
              ))}
          </div>
        </div>
      </div>
      <div className="flex flex-col justify-between ml-4 w-24 h-[350px]">
        <div>
          <Button size={'icon'} variant="ghost" onClick={navigateToEdit}>
            <PencilIcon />
          </Button>
        </div>
      </div>
    </div>
  );
}
