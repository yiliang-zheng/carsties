import React, { useState } from "react";

import { pageSizes } from "@/app/_hooks/useParamsStore";
import { cn } from "@/utils/cn";
import DownArrowIcon from "@/app/_components/Icons/DownArrow";
import UpArrowIcon from "@/app/_components/Icons/UpArrow";

import type { PageSizeOption } from "@/app/_hooks/useParamsStore";
type Props = {
  pageSize: PageSizeOption;
  onPageSizeChange: (size: PageSizeOption) => void;
};

const PageSize = ({ pageSize, onPageSizeChange }: Props) => {
  const [open, setOpen] = useState(false);

  return (
    <div className="relative cursor-pointer min-w-[6rem]">
      <div
        className={cn(
          "h-10 flex w-full items-center justify-between gap-2.5 rounded border bg-white px-2.5 py-1.5 hover:border-green-600",
          {
            "border-grey-400": !open,
            "border-green-600": open,
          }
        )}
        onClick={() => setOpen(!open)}
      >
        <span className="uppercase">Page Size - {pageSize}</span>
        <span className="font-normal normal-case not-italic leading-none antialiased text-grey-400">
          {open ? <UpArrowIcon /> : <DownArrowIcon />}
        </span>
      </div>
      <div className="absolute inset-0 bottom-auto">
        <div
          className={cn(
            "absolute w-full overflow-hidden rounded border border-grey-400 bg-white bottom-1 right-0",
            { hidden: !open }
          )}
        >
          {pageSizes.map((size, idx) => (
            <div
              key={idx}
              className={cn(
                "px-2.5 py-1 hover:bg-grey-200 hover:text-green-600 bg-grey-200",
                { "text-green-600": size === pageSize }
              )}
              onClick={() => {
                onPageSizeChange(size);
                setOpen(false);
              }}
            >
              {size}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

PageSize.displayName = "PageSize";
export default PageSize;
