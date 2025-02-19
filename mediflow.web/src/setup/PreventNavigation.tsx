import { useRouter } from 'next/navigation';
import { FC, ReactNode, useEffect, useRef } from 'react';

interface PreventNavigationProps {
  children: ReactNode;
  shouldPrevent: boolean;
}

const PreventNavigation: FC<PreventNavigationProps> = ({
  children,
  shouldPrevent,
}) => {
  const router = useRouter();

  const shouldPreventRef = useRef(shouldPrevent);

  useEffect(() => {
    shouldPreventRef.current = shouldPrevent;
  }, [shouldPrevent]);

  useEffect(() => {
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      if (shouldPrevent) {
        event.preventDefault();
        event.returnValue = '';
      }
    };
    window.addEventListener('beforeunload', handleBeforeUnload);

    return () => {
      window.removeEventListener('beforeunload', handleBeforeUnload);
    };
  }, [shouldPrevent]);

  useEffect(() => {
    const checkIfShouldPrevent = () => {
      if (
        shouldPreventRef.current &&
        !window.confirm(
          'Nesaglabātās izmaiņas netiks saglabātas. Vai tiešām gribat turpināt?',
        )
      ) {
        return false;
      }
      return true;
    };

    const originalPush = router.push;

    router.push = (url: string) => {
      if (checkIfShouldPrevent()) {
        originalPush(url);
      }
    };

    return () => {
      router.push = originalPush;
    };
  }, [router, shouldPrevent]);

  return <>{children}</>;
};

export default PreventNavigation;

//usage example
/* 
const [should, setShould] = useState(false);
const router = useRouter();

<PreventNavigation shouldPrevent={true}>
  <div className="mx-auto mt-6 w-fit">Svecināti atpakaļ</div>
  <Button
    onClick={() => {
      console.warn('user click');
      router.push(RouteNames.JournalIndex);
    }}
  >
    naviagte away!
  </Button>
  <Checkbox checked={should} onCheckedChange={() => setShould(!should)} />
</PreventNavigation>; 
*/
