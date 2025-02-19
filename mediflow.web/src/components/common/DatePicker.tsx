'use client';

import * as React from 'react';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { CalendarIcon } from 'lucide-react';

export function DatePicker({
  className,
  date,
  disabled,
  onSelected,
}: {
  className?: string;
  date: Date | undefined;
  disabled?: boolean;
  onSelected?: (date: Date | undefined) => void;
}) {
  onSelected ??= () => {};
  const [isOpen, setIsOpen] = React.useState(false);
  const handleSelect = (date: Date | undefined) => {
    setIsOpen(false);
    if (disabled) throw Error('date picked on disabled date picker');
    if (date) onSelected(date);
    else onSelected(undefined);
  };
  return (
    <Popover open={isOpen} onOpenChange={setIsOpen}>
      <PopoverTrigger asChild>
        <Button
          variant={'outline'}
          disabled={disabled}
          className={cn(
            'justify-start text-left font-normal w-full',
            !date && 'text-muted-foreground',
            className,
          )}
        >
          <CalendarIcon className="mr-2 w-4 h-4" />
          {date?.toLocaleString('lv-LV', { dateStyle: 'medium' }) ?? (
            <span>Izvēlēties datumu</span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="p-0 w-auto">
        <Calendar
          mode="single"
          captionLayout="dropdown-buttons"
          selected={date}
          onSelect={handleSelect}
          fromYear={1960}
          toYear={2070}
        />
      </PopoverContent>
    </Popover>
  );
}
