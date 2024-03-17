import React, { useState } from "react";
import { useParamsStore } from "@/app/_hooks/useParamsStore";
import { useShallow } from "zustand/react/shallow";

import CloseIcon from "@/app/_components/Icons/Close";
import FilterIcon from "@/app/_components/Icons/Filter";
import FilterModal from "@/app/_components/Filters/FilterModal";
import SortByFilter from "@/app/_components/Filters/SortByFilter";
import FilterByFilter from "@/app/_components/Filters/FilterByFilter";

const Filter = () => {
  const [filterOpen, setFilterOpen] = useState(false);
  const params = useParamsStore(
    useShallow((state) => ({
      filterBy: state.filterBy,
      orderBy: state.orderBy,
      searchTerm: state.searchTerm,
    }))
  );

  const isInitialValue = useParamsStore((state) => state.isInitialState());
  const setParams = useParamsStore((state) => state.setParams);
  const reset = useParamsStore((state) => state.resetParams);

  return (
    <div className="flex-col gap-2.5 py-3 mr-3 flex w-full shrink-0 lg:w-[12.5rem]">
      <div className="flex flex-row justify-between items-end">
        <div className="text-xl leading-6 font-bold text-gray-900">Filter</div>
        <div className="lg:hidden" onClick={() => setFilterOpen(true)}>
          <FilterIcon className="w-[1.5rem] h-[1.5rem]" />
        </div>

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
      <div className="hidden lg:block space-y-4">
        <SortByFilter
          orderBy={params.orderBy}
          onSortByChange={(option) => setParams({ orderBy: option })}
        />
        <FilterByFilter
          filterBy={params.filterBy}
          onFilterByChange={(option) => setParams({ filterBy: option })}
        />
      </div>
      {filterOpen && (
        <FilterModal
          initSearchTerm={params.searchTerm}
          initOrderBy={params.orderBy}
          initFilterBy={params.filterBy}
          onCloseModalClick={() => setFilterOpen(false)}
        />
      )}
    </div>
  );
};

Filter.displayName = "Filter";
export default Filter;
