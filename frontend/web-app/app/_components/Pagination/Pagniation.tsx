"use client";
import React, { useMemo, useCallback } from "react";
import { range } from "@/utils/array/array";
import { cn } from "@/utils/cn";
type Props = {
  currentPage: number;
  totalPages: number;
  onPageChange: (pageNumber: number) => void;
};

const Pagination = ({ currentPage, totalPages, onPageChange }: Props) => {
  const pageRange = useMemo(() => {
    if (totalPages <= 5) return range(1, totalPages);
    const start = Math.min(totalPages - 5, currentPage);
    const end = Math.min(currentPage + 5, totalPages);
    return range(start, end);
  }, [currentPage, totalPages]);

  const previous = useMemo(() => {
    if (totalPages <= 0) return false;
    return currentPage > 1;
  }, [currentPage, totalPages]);

  const next = useMemo(() => {
    if (totalPages <= 0) return false;

    return currentPage < totalPages;
  }, [currentPage, totalPages]);

  const isActive = useCallback(
    (page: number): boolean => {
      return page === currentPage;
    },
    [currentPage, totalPages]
  );

  if (totalPages === 0) return null;
  return (
    <ul className="inline-flex -space-x-px text-base h-10">
      <li>
        <button
          onClick={(e) => {
            e.preventDefault();
            onPageChange(currentPage - 1);
          }}
          disabled={!previous}
          className="flex items-center justify-center px-4 h-10 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg 
          hover:bg-gray-100 hover:text-gray-700
          disabled:bg-gray-100 disabled:text-gray-700"
        >
          Previous
        </button>
      </li>
      {pageRange.map((page) => (
        <li key={page}>
          <button
            className={cn(
              "flex items-center justify-center px-4 h-10 border border-gray-300",
              {
                "leading-tight text-gray-500 bg-white hover:bg-gray-100 hover:text-gray-700":
                  !isActive(page),
              },
              {
                "text-blue-600 bg-blue-50 hover:bg-blue-100 hover:text-blue-700":
                  isActive(page),
              }
            )}
            disabled={isActive(page)}
            onClick={(e) => {
              e.preventDefault();
              onPageChange(page);
            }}
          >
            {page}
          </button>
        </li>
      ))}

      <li>
        <button
          disabled={!next}
          onClick={(e) => {
            e.preventDefault();
            onPageChange(currentPage + 1);
          }}
          className="flex items-center justify-center px-4 h-10 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg
           hover:bg-gray-100 hover:text-gray-700
           disabled:bg-gray-100 disabled:text-gray-700"
        >
          Next
        </button>
      </li>
    </ul>
  );
};

Pagination.displayName = "Pagination";
export default Pagination;
