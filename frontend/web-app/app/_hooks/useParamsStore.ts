import { create } from "zustand";
import { subscribeWithSelector } from "zustand/middleware";

export const pageSizes = [3, 6, 8, 12] as const;

export const sortByOptions = [
  {
    name: "Alphabetical",
    value: "make",
  },
  {
    name: "New Listing",
    value: "createdAt",
  },
  {
    name: "Ending Soon",
    value: "auctionEnd",
  },
] as const;

export const filterByOptions = [
  {
    name: "Finished",
    value: "finished",
  },
  {
    name: "Ending Soon",
    value: "endingSoon",
  },
  {
    name: "Live Now",
    value: "live",
  },
] as const;

export type PageSizeOption = (typeof pageSizes)[number];
export type SortByOption = (typeof sortByOptions)[number]["value"];
export type FilterByOption = (typeof filterByOptions)[number]["value"];

type State = {
  pageNumber: number;
  pageSize: PageSizeOption;
  searchTerm: string;
  seller: string;
  winner: string;
  orderBy: SortByOption;
  filterBy: FilterByOption;
};

type Action = {
  setParams: (params: Partial<State>) => void;
  resetParams: () => void;
  isInitialState: () => boolean;
};

const initState: State = {
  pageNumber: 1,
  pageSize: 6,
  searchTerm: "",
  seller: "",
  winner: "",
  orderBy: "auctionEnd",
  filterBy: "live",
};
export const useParamsStore = create<State & Action>()(
  subscribeWithSelector((set, get) => ({
    ...initState,
    setParams: (params: Partial<State>) =>
      set((state) => {
        if (params.pageNumber) {
          return {
            ...state,
            pageNumber: params.pageNumber,
          };
        } else {
          return {
            ...params,
            pageNumber: 1,
          };
        }
      }),
    resetParams: () => set(initState),
    isInitialState: () => {
      const { filterBy, orderBy, searchTerm, pageSize } = get();
      const result =
        filterBy === initState.filterBy &&
        orderBy === initState.orderBy &&
        searchTerm === initState.searchTerm &&
        pageSize === initState.pageSize;
      return result;
    },
  }))
);
