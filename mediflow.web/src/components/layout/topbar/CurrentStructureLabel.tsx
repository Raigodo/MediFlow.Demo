import { useCurrentStructure } from '@/lib/fetching/api/hooks/structures/useCurrentStructure';

function StructureLabel() {
  const { data, isSuccess, isError, isLoading } = useCurrentStructure();
  return (
    <>
      {isLoading && <>Loading...</>}
      {!isLoading && isError && <>Error......</>}
      {!isLoading && isSuccess && (
        <>
          {!data.body && <>No structure!</>}
          {data.body && <>{data.body.name}</>}
        </>
      )}
    </>
  );
}

export default StructureLabel;
