import { create } from "zustand";
import { subscribeWithSelector } from "zustand/middleware";

import type { Auction, PagedAuction } from "@/server/schemas/auction";

type State = {
  auctions: Auction[];
  totalCount: number;
  pageCount: number;
};

type Action = {
  setData: (data: PagedAuction) => void;
  setCurrentPrice: (auctionId: string, price: number) => void;
};

const initState: State = {
  auctions: [],
  totalCount: 0,
  pageCount: 0,
};

export const useAuctionStore = create<State & Action>()(
  subscribeWithSelector((set, get) => ({
    ...initState,
    setData: (data) =>
      set(() => ({
        auctions: data.results,
        totalCount: data.totalCount,
        pageCount: data.pageCount,
      })),
    setCurrentPrice: (auctionId, price) =>
      set((state) => ({
        ...state,
        auctions: state.auctions.map((auction) => {
          if (auction.id !== auctionId) return auction;
          return {
            ...auction,
            currentHighBid: price,
          };
        }),
      })),
  }))
);
