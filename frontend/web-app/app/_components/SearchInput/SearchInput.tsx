"use client";
import React, { useRef } from "react";
import { useParamsStore } from "@/app/_hooks/useParamsStore";

const SearchInput = () => {
  const searchRef = useRef<HTMLInputElement>(null);
  const setParams = useParamsStore((state) => state.setParams);

  return (
    <div className="w-full flex items-center border-2 border-gray-300 rounded-full py-2 shadow-sm">
      <input
        ref={searchRef}
        type="search"
        className="
        flex-grow 
        bg-transparent         
        border-transparent
        border-b-gray-500        
        ring-0
        pl-5        
        text-gray-600 
        placeholder-gray-500 placeholder:text-lg        
        focus:outline-none
        focus:border-transparent
        focus:ring-0"
        placeholder="Search cars by make, model and color..."
      />

      <button
        className="p-2 bg-gray-600 rounded-full mx-2 cursor-pointer"
        onClick={(e) => {
          e.preventDefault();
          if (!!searchRef.current) {
            setParams({ searchTerm: searchRef.current.value });
          }
        }}
      >
        <svg
          className="w-4 h-4 text-white"
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
      </button>
    </div>
  );
};

SearchInput.displayName = "SearchInput";
export default SearchInput;
