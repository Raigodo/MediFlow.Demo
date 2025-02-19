'use client';

import { clientRequests } from '@/lib/fetching/api/requests/clientRequests';
import { employeeRequests } from '@/lib/fetching/api/requests/employeeRequests';
import { invitationRequests } from '@/lib/fetching/api/requests/invitationRequests';
import { noteRequests } from '@/lib/fetching/api/requests/noteRequests';
import { sessionRequests } from '@/lib/fetching/api/requests/sessionRequests';
import { structureRequests } from '@/lib/fetching/api/requests/structureRequests';
import { userRequests } from '@/lib/fetching/api/requests/userRequests';
import { createContext, ReactNode, useContext } from 'react';

const requestOptions = {
  clients: clientRequests(),
  employees: employeeRequests(),
  invitations: invitationRequests(),
  notes: noteRequests(),
  strucures: structureRequests(),
  session: sessionRequests(),
  users: userRequests(),
};

type ServerApiContext = typeof requestOptions;

const Context = createContext<ServerApiContext | undefined>(undefined);

const ApiContextProvider = ({ children }: { children: ReactNode }) => {
  return <Context.Provider value={requestOptions}>{children}</Context.Provider>;
};

export default ApiContextProvider;

export const useServerApi = () => {
  const context = useContext(Context);
  if (!context) throw Error('no api context');
  return context;
};
