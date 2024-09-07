import { create } from "zustand";
import { subscribeWithSelector } from "zustand/middleware";
import { devtools } from "zustand/middleware";

import type { Bid } from "@/server/schemas/bid";

type State = {
  bids: { auctionId: string; bids: Bid[]; auctionOpen: boolean }[];
};

type Action = {
  setBids: (auctionId: string, bids: Bid[], auctionOpen: boolean) => void;
  addBid: (bid: Bid) => void;
  setAuctionOpen: (auctionId: string, auctionOpen: boolean) => void;
};

const initState: State = {
  bids: [],
};

export const useBidStore = create<State & Action>()(
  devtools(
    subscribeWithSelector((set) => ({
      ...initState,
      setBids: (auctionId, bids, auctionOpen) =>
        set((state) => {
          const exists = state.bids.findIndex((p) => p.auctionId === auctionId);
          if (exists > -1) {
            return {
              bids: state.bids.toSpliced(exists, 1, {
                auctionId,
                bids,
                auctionOpen,
              }),
            };
          } else {
            return { bids: [...state.bids, { auctionId, bids, auctionOpen }] };
          }
        }),
      addBid: (bid) =>
        set((state) => {
          const existAuction = state.bids.find(
            (p) => p.auctionId === bid.auctionId
          );
          //auction not found
          if (!existAuction) {
            return {
              bids: [
                ...state.bids,
                { auctionId: bid.auctionId, bids: [bid], auctionOpen: true },
              ],
            };
          }

          //auction found
          const existBid = existAuction.bids.find((p) => p.id === bid.id);
          //bid not exist in the found auction
          //add to {auction, bids:[...existAuction.bids, newBid]}
          if (!existBid) {
            const existAuctionIdx = state.bids.indexOf(existAuction);
            return {
              bids: state.bids.toSpliced(existAuctionIdx, 1, {
                auctionId: existAuction.auctionId,
                bids: [...existAuction.bids, bid],
                auctionOpen: existAuction.auctionOpen,
              }),
            };
          }

          //auction found, but same bid already exist
          //no further action, not allow adding duplicate bid
          return { bids: [...state.bids] };
        }),
      setAuctionOpen(auctionId, auctionOpen) {
        set((state) => {
          const existAuction = state.bids.find(
            (p) => p.auctionId === auctionId
          );
          //auction not exist
          //do nothing
          if (!existAuction) return { bids: [...state.bids] };

          const idx = state.bids.indexOf(existAuction);
          return {
            bids: state.bids.toSpliced(idx, 1, {
              ...existAuction,
              auctionOpen,
            }),
          };
        });
      },
    })),
    {
      name: "bidStore",
    }
  )
);
