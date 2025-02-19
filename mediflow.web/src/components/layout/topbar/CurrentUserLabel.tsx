import { UserEntity } from '@/lib/domain/entities/UserEntity';
import { useCurrentUser } from '@/lib/fetching/api/hooks/users/useCurrentUser';

function CurrentUserLabel() {
  const { data, isSuccess, isError, isLoading } = useCurrentUser();
  return (
    <>
      {isLoading && <>Loading...</>} {!isLoading && isError && <>Error...</>}
      {!isLoading && isSuccess && <RenderData user={data.body} />}
    </>
  );
}

export default CurrentUserLabel;

const RenderData = ({ user }: { user: UserEntity | null }) => {
  return (
    <>
      {!user && <>No user</>}
      {user?.name} {user?.surname}
    </>
  );
};
