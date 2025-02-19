import { ComponentType, useEffect, useRef, useState } from 'react';
import {
  ModalKeys,
  useModalManager,
} from './contexts/core/ModalManagerContext';

export type ModalWrapperItem = ComponentType<{
  isOpen: boolean;
  onClose: () => void;
}>;

function ModalWrapper({
  modalComponent: ModalComponent,
  modalKey,
}: {
  modalComponent: ModalWrapperItem;
  modalKey: ModalKeys;
}) {
  const { currentModalKey, close } = useModalManager();
  const [isOpen, setIsOpen] = useState(false);

  //handle smooth close
  const [isFading, setIsFading] = useState(false);
  const timerId = useRef<NodeJS.Timeout>(undefined);

  useEffect(() => {
    if (currentModalKey === modalKey && !isOpen) {
      setIsOpen(true);
      setIsFading(false);
      if (timerId.current) clearTimeout(timerId.current);
      timerId.current = undefined;
    }
    //start fading
    if (currentModalKey !== modalKey && isOpen) {
      setIsOpen(false);
      setIsFading(true);
      timerId.current = setTimeout(() => {
        setIsFading(false);
        timerId.current = undefined;
      }, 1000);
    }
  }, [currentModalKey, modalKey, isOpen]);

  return (
    <>
      {(isOpen || isFading) && (
        <ModalComponent isOpen={isOpen} onClose={close} />
      )}
    </>
  );
}

export default ModalWrapper;
