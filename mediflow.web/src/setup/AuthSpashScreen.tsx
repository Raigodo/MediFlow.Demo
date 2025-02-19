import { ReactNode } from 'react';
import { useUserSession } from './contexts/core/UserSessionContext';

function AuthSplashScreen({ children }: { children: ReactNode }) {
  const { lifecycle } = useUserSession();
  if (lifecycle.isBooting === false) {
    return <>{children}</>;
  }
  return (
    <div className="flex justify-center items-center w-screen h-screen text-2xl">
      MediFlow
    </div>
  );
}

export default AuthSplashScreen;
