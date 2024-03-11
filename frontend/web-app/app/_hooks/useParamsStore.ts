import { create } from "zustand";

type State = {
  pageNumber: number;
  pageSize: number;
  searchTerm: string;
  seller: string;
  winner: string;
  orderBy: string;
  filterBy: string;
};

type Action = {
  setParams: (params: Partial<State>) => void;
  resetParams: () => void;
};

const initState: State = {
  pageNumber: 1,
  pageSize: 4,
  searchTerm: "",
  seller: "",
  winner: "",
  orderBy: "",
  filterBy: "",
};
export const useParamsStore = create<State & Action>((set) => ({
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
}));
