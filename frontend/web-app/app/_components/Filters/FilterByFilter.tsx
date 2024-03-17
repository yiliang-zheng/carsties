import { cn } from "@/utils/cn";
import { filterByOptions } from "@/app/_hooks/useParamsStore";
import type { FilterByOption } from "@/app/_hooks/useParamsStore";

type FilterByProps = {
  filterBy: FilterByOption;
  onFilterByChange: (option: FilterByOption) => void;
};

const FilterByFilter = ({ filterBy, onFilterByChange }: FilterByProps) => {
  return (
    <>
      <div className="text-sm leading-[1.125rem] font-bold text-gray-900 uppercase">
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
                `,
              {
                "bg-transparent text-grey-900":
                  filterByOption.value !== filterBy,
              },
              {
                "bg-black text-white": filterByOption.value === filterBy,
              }
            )}
            onClick={() => onFilterByChange(filterByOption.value)}
          >
            {filterByOption.name}
          </div>
        ))}
      </div>
    </>
  );
};

FilterByFilter.displayName = "FilterByFilter";
export default FilterByFilter;
