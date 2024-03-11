const Filter = () => {
  return (
    <div className="flex-col gap-2.5 py-3 flex w-full shrink-0 lg:w-[12.5rem]">
      <div className="text-xl leading-6 font-bold text-navy">Filter</div>
      <div className="h-[0.0625rem] w-full bg-grey-600"></div>
      <div className="text-sm leading-[1.125rem] font-bold text-navy uppercase hidden lg:block">
        Sort By
      </div>
    </div>
  );
};

Filter.displayName = "Filter";
export default Filter;
