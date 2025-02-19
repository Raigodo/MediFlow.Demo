'use client';

import EmployeesTable from '@/components/employee/EmployeesTable';
import StructureLabel from '@/components/layout/topbar/CurrentStructureLabel';
import { Label } from '@/components/ui/label';

function Index() {
  return (
    <>
      <div className="gap-8 grid grid-rows-[auto_1fr] px-5 pt-6 pb-10 size-full">
        <div className="text-lg">
          <StructureLabel />
        </div>
        <div className="flex flex-col gap-4">
          <Label>Darbinieki</Label>
          <EmployeesTable />
        </div>
      </div>
    </>
  );
}

export default Index;
