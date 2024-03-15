import {
  useParamsStore,
  sortByOptions,
  filterByOptions,
} from "@/app/_hooks/useParamsStore";
import { useShallow } from "zustand/react/shallow";
import { cn } from "@/utils/cn";

import CloseIcon from "@/app/_components/Icons/Close";
import FilterIcon from "@/app/_components/Icons/Filter";
const Filter = () => {
  const params = useParamsStore(
    useShallow((state) => ({
      filterBy: state.filterBy,
      orderBy: state.orderBy,
    }))
  );

  const isInitialValue = useParamsStore((state) => state.isInitialState());
  const setParams = useParamsStore((state) => state.setParams);
  const reset = useParamsStore((state) => state.resetParams);

  return (
    <div className="flex-col gap-2.5 py-3 mr-3 flex w-full shrink-0 lg:w-[12.5rem]">
      <div className="flex flex-row justify-between items-end">
        <div className="text-xl leading-6 font-bold text-gray-900">Filter</div>
        <FilterIcon className="w-[1.5rem] h-[1.5rem] lg:hidden" />
        {!isInitialValue && (
          <div
            className="text-gray-600 hidden lg:flex lg:items-center lg:cursor-pointer lg:hover:underline"
            onClick={reset}
          >
            <CloseIcon />
            Reset
          </div>
        )}
      </div>

      <div className="h-[0.0625rem] w-full bg-grey-600 border-b-[1px] border-b-gray-900"></div>
      <div className="text-sm leading-[1.125rem] font-bold text-gray-900 uppercase hidden lg:block">
        Sort By
      </div>
      <div className="flex flex-row flex-wrap gap-2">
        {sortByOptions.map((sortByOption, idx) => (
          <div
            key={idx}
            className={cn(
              `rounded-full 
              px-3 
              py-1.5 
              font-medium 
              leading-none 
              border 
              border-grey-900              
              hover:bg-grey-200
              hover:text-grey-900 
              cursor-pointer 
              flex-row 
              items-center 
              justify-center 
              gap-1 
              hidden lg:block`,
              {
                "bg-transparent text-grey-900":
                  sortByOption.value !== params.orderBy,
              },
              {
                "bg-black text-white": sortByOption.value === params.orderBy,
              }
            )}
            onClick={() => setParams({ orderBy: sortByOption.value })}
          >
            {sortByOption.name}
          </div>
        ))}
      </div>

      <div className="text-sm leading-[1.125rem] font-bold text-gray-900 uppercase hidden lg:block">
        Filter By
      </div>
      <div className="flex flex-row flex-wrap gap-2">
        {filterByOptions.map((filterByOption, idx) => (
          <div
            key={idx}
            className={cn(
              `rounded-full 
              px-3 
              py-1.5 
              font-medium 
              leading-none 
              border 
              border-grey-900              
              hover:bg-grey-200
              hover:text-grey-900 
              cursor-pointer 
              flex-row 
              items-center 
              justify-center 
              gap-1 
              hidden lg:block`,
              {
                "bg-transparent text-grey-900":
                  filterByOption.value !== params.filterBy,
              },
              {
                "bg-black text-white": filterByOption.value === params.filterBy,
              }
            )}
            onClick={() => setParams({ filterBy: filterByOption.value })}
          >
            {filterByOption.name}
          </div>
        ))}
      </div>
    </div>
  );
};

Filter.displayName = "Filter";
export default Filter;
