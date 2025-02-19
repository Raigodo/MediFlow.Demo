import SessionOverview from './SessionOverview';

function Topbar() {
  return (
    <div className="flex justify-between items-center h-full">
      Mediflow
      <div>
        <SessionOverview />
      </div>
    </div>
  );
}

export default Topbar;
