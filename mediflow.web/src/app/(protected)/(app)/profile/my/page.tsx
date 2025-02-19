'use client';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { useRemoveUserAvatar } from '@/lib/fetching/api/hooks/users/useRemoveUserAvatar';
import { useSetUserAvatar } from '@/lib/fetching/api/hooks/users/useSetUserAvatar';
import { Trash2Icon } from 'lucide-react';
import { useState } from 'react';

function Index() {
  const { mutateAsync: setAvatarImage } = useSetUserAvatar();
  const { mutateAsync: removeAvatarImage } = useRemoveUserAvatar();

  const [file, setFile] = useState<File | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setFile(event.target.files[0]);
    }
  };

  return (
    <div className="p-8">
      MyProfile
      <div className="flex gap-4">
        <Input type="file" accept="image/*" onChange={handleFileChange} />
        <Button onClick={() => file && setAvatarImage({ avatarImage: file })}>
          Set Avatar
        </Button>
        <Button onClick={() => removeAvatarImage({ avatarImage: file })}>
          <Trash2Icon />
        </Button>
      </div>
    </div>
  );
}

export default Index;
