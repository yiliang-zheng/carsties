import { create } from "zustand";
import { subscribeWithSelector } from "zustand/middleware";
import { devtools } from "zustand/middleware";

import type { Bid } from "@/server/schemas/bid";

type State = {
  bids: Bid[];
};

type Action = {
  setBids: (bids: Bid[]) => void;
  addBid: (bid: Bid) => void;
};

const initState: State = {
  bids: [],
};

export const useBidStore = create<State & Action>()(
  devtools(
    subscribeWithSelector((set) => ({
      ...initState,
      setBids: (bids) => set(() => ({ bids })),
      addBid: (bid) =>
        set((state) => {
          const exists = state.bids.find((p) => p.id === bid.id);
          if (!exists) return { bids: [bid, ...state.bids] };
          return { bids: [...state.bids] };
        }),
    })),
    {
      name: "bidStore",
    }
  )
);
