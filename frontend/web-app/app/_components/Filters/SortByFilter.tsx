import { cn } from "@/utils/cn";
import { sortByOptions } from "@/app/_hooks/useParamsStore";
import type { SortByOption } from "@/app/_hooks/useParamsStore";

type SortByProps = {
  orderBy: SortByOption;
  onSortByChange: (option: SortByOption) => void;
};

const SortByFilter = ({ orderBy, onSortByChange }: SortByProps) => {
  return (
    <>
      <div className="text-sm leading-[1.125rem] font-bold text-gray-900 uppercase">
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
                gap-1 `,
              {
                "bg-transparent text-grey-900": sortByOption.value !== orderBy,
              },
              {
                "bg-black text-white": sortByOption.value === orderBy,
              }
            )}
            onClick={() => onSortByChange(sortByOption.value)}
          >
            {sortByOption.name}
          </div>
        ))}
      </div>
    </>
  );
};

SortByFilter.displayName = "SortByFilter";
export default SortByFilter;
