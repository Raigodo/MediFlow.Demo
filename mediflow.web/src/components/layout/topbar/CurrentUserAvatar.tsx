import { Avatar, AvatarImage } from '@/components/ui/avatar';
import { useUserAvatar } from '@/lib/fetching/api/hooks/users/useUserAvatar';

function CurrentUserAvatar() {
  const { data, isSuccess, isError, isLoading } = useUserAvatar();
  return (
    <Avatar className="size-full">
      {isLoading && <>Loading...</>}
      {!isLoading && (
        <AvatarImage
          className="size-full"
          src={
            isSuccess && data.body ? data.body : 'https://github.com/shadcn.png'
          }
          alt="avatar"
        />
      )}
    </Avatar>
  );
}

export default CurrentUserAvatar;
