import React, { useState } from "react";
import { useParamsStore } from "@/app/_hooks/useParamsStore";

import type { SortByOption, FilterByOption } from "@/app/_hooks/useParamsStore";
import SortByFilter from "@/app/_components/Filters/SortByFilter";
import FilterByFilter from "@/app/_components/Filters/FilterByFilter";

type FilterModalProps = {
  onCloseModalClick: () => void;
  initSearchTerm?: string;
  initOrderBy: SortByOption;
  initFilterBy: FilterByOption;
};

const FilterModal = ({
  initSearchTerm,
  initOrderBy,
  initFilterBy,
  onCloseModalClick,
}: FilterModalProps) => {
  const [search, setSearch] = useState(initSearchTerm);
  const [orderBy, setOrderBy] = useState<SortByOption>(initOrderBy);
  const [filterBy, setFilterBy] = useState<FilterByOption>(initFilterBy);

  const setParams = useParamsStore((state) => state.setParams);
  const reset = useParamsStore((state) => state.resetParams);
  return (
    <div
      tabIndex={-1}
      aria-hidden="true"
      className="overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full bg-gray-500 bg-opacity-50"
    >
      <div className="relative p-4 w-full max-w-2xl max-h-full">
        <div className="relative bg-white rounded-lg shadow dark:bg-gray-700">
          <div className="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600">
            <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
              Filter
            </h3>
            <button
              type="button"
              className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
              onClick={onCloseModalClick}
            >
              <svg
                className="w-3 h-3"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 14"
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                />
              </svg>
              <span className="sr-only">Close modal</span>
            </button>
          </div>

          <div className="p-4 md:p-5 space-y-4 flex flex-col">
            <div className="text-sm leading-[1.125rem] font-bold font-dm_sans text-navy uppercase">
              Search Car
            </div>
            <div className="relative">
              <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
                <svg
                  className="w-4 h-4 text-gray-500 dark:text-gray-400"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 20 20"
                >
                  <path
                    stroke="currentColor"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth="2"
                    d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
                  />
                </svg>
              </div>
              <input
                type="search"
                className="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-gray-500 focus:border-gray-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                placeholder="Search cars by make, model and color..."
                value={search}
                onInput={(e) => {
                  const element = e.target as HTMLInputElement;
                  setSearch(element.value);
                }}
              />
            </div>
            <SortByFilter orderBy={orderBy} onSortByChange={setOrderBy} />
            <FilterByFilter
              filterBy={filterBy}
              onFilterByChange={setFilterBy}
            />
          </div>

          <div className="flex items-center justify-between p-4 md:p-5 border-t border-gray-200 rounded-b dark:border-gray-600">
            <button
              type="button"
              className="flex-1 flex-grow text-white text-md font-bold bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 rounded-lg px-5 py-2.5 text-center dark:bg-gray-600 dark:hover:bg-gray-700 dark:focus:ring-gray-800"
              onClick={() => {
                setParams({
                  searchTerm: search,
                  orderBy,
                  filterBy,
                });
                onCloseModalClick();
              }}
            >
              Apply Filter
            </button>
            <button
              type="button"
              className="flex-1 flex-grow py-2.5 px-5 ms-3 text-md font-bold text-gray-900 focus:outline-none bg-white rounded-lg border-2 border-gray-800 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
              onClick={() => {
                reset();
                onCloseModalClick();
              }}
            >
              Clear All
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

FilterModal.displayName = "FilterModal";
export default FilterModal;
