'use client';

import './globals.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ApiContextProvider from '@/setup/contexts/core/ServerApiContext';
import UserSessionContextProvider from '@/setup/contexts/core/UserSessionContext';
import RequestsContextProvider from '@/setup/contexts/core/RequestsContext';
import NoSSR from '@/setup/NoSSR';
import AuthSplashScreen from '@/setup/AuthSpashScreen';
import ModalManagerContextProvider from '@/setup/contexts/core/ModalManagerContext';
import Orientation from '@/setup/Orientation';
// import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

const staleTime = 1000 * 60 * 30; //30 minutes
const gcTime = 1000 * 60 * 5; //5 minute

const queryClient = new QueryClient({
  defaultOptions: {
    queries: { retry: 3, gcTime, staleTime, retryDelay: 1000 },
  },
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      {/* <head>
        <script src="https://unpkg.com/react-scan/dist/auto.global.js" />
      </head> */}
      <body className="overflow-hidden select-none">
        <NoSSR>
          <Orientation>
            <QueryClientProvider client={queryClient}>
              <ApiContextProvider>
                <UserSessionContextProvider>
                  <RequestsContextProvider>
                    <ModalManagerContextProvider>
                      <AuthSplashScreen>{children}</AuthSplashScreen>
                    </ModalManagerContextProvider>
                  </RequestsContextProvider>
                </UserSessionContextProvider>
              </ApiContextProvider>
              {/* <ReactQueryDevtools /> */}
            </QueryClientProvider>
          </Orientation>
        </NoSSR>
      </body>
    </html>
  );
}
