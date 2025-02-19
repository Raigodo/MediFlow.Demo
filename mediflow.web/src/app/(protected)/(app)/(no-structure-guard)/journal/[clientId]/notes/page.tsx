'use client';

import JournalFilterModalProvider from '@/components/journal/modals/JournalFilterModalProvider';
import ViewNoteModalProvider from '@/components/journal/modals/ViewNoteModalProvider';
import OpenJournalFilterButton from '@/components/journal/table/filter/OpenJournalFilterButton';
import NotesTable from '@/components/journal/table/NotesTable';
import OpenTodaysNoteButton from '@/components/journal/table/OpenTodaysNoteButton';

function Index() {
  return (
    <>
      <div className="grid grid-cols-[1fr_auto] mr-5">
        <div className="flex gap-2 grid-cols-3 bg-transparent mt-2.5 px-2.5 w-fit *:h-8 *:grow">
          <OpenTodaysNoteButton />
        </div>
        <div className="mt-8">
          <OpenJournalFilterButton />
        </div>
      </div>
      <div className="mx-5 mb-10">
        <div className="flex justify-between mb-4">
          {/* <FilterBadges /> */}
        </div>
        <NotesTable />
      </div>
      <JournalFilterModalProvider />
      <ViewNoteModalProvider />
    </>
  );
}

export default Index;
