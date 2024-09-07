import React, { useEffect, useMemo, useRef } from "react";
import { trpc } from "@/app/_trpc/client";
import { useParams } from "next/navigation";
import { useBidStore } from "@/app/_hooks/useBidStore";
import { useShallow } from "zustand/react/shallow";
import { useSession } from "next-auth/react";

import BidItem from "@/app/_components/BidItem/BidItem";
import Heading from "@/app/_components/Core/Heading";
import EmptyList from "@/app/_components/EmptyList/EmptyList";
import BidForm from "@/app/_components/BidForm/BidForm";

import type { Auction } from "@/server/schemas/auction";
import { Bid } from "@/server/schemas/bid";

type Props = {
  auction: Auction;
};

const BidList = ({ auction }: Props) => {
  const bidListRef = useRef<HTMLDivElement>(null);
  const { data: session } = useSession();
  const params = useParams<{ id: string }>();
  const bidsStore = useBidStore(
    useShallow((state) => ({
      bidsWithAuction: state.bids,
      setBids: state.setBids,
    }))
  );
  const {
    data: bids,
    isLoading,
    error,
  } = trpc.bids.getBidsByAuction.useQuery(
    { auctionId: params.id },
    {
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false,
    }
  );

  useEffect(() => {
    if (!isLoading && !error)
      bidsStore.setBids(auction.id, bids, auction.status === "Live");
  }, [bids, isLoading]);

  const auctionOpen = useMemo<boolean>(() => {
    const selected = bidsStore.bidsWithAuction.find(
      (p) => p.auctionId === params.id
    );
    return selected?.auctionOpen ?? false;
  }, [bidsStore.bidsWithAuction]);

  const selectedBids = useMemo<Bid[]>(() => {
    const selected = bidsStore.bidsWithAuction.find(
      (p) => p.auctionId === params.id
    );
    if (!selected || !selected.bids) return [] as Bid[];

    return selected.bids;
  }, [bidsStore.bidsWithAuction]);

  const currentHighestBid = useMemo<number | null>(() => {
    const result = selectedBids.reduce((prev, current) => {
      if (current.amount > prev) return current.amount;
      return prev;
    }, 0);
    return result;
  }, [selectedBids]);

  useEffect(() => {
    if (!!bidListRef.current) {
      bidListRef.current.scrollTo(0, 0);
    }
  }, [selectedBids]);

  if (isLoading) return <div>loading...</div>;
  if (!!error) return <div>Error Occurred: {error.message}</div>;

  return (
    <div className="rounded-lg shadow-md">
      <div className="py-2 px-4 bg-white">
        <div className="sticky top-0 bg-white p-2">
          <Heading title="Bids" />
        </div>
      </div>
      <div className="overflow-auto h-[400px] flex flex-col-reverse px-2">
        {selectedBids.length === 0 && (
          <EmptyList
            title="No bids for this item"
            subtitle="Please feel free to make a bid"
            showReset={false}
          />
        )}
        {selectedBids.length > 0 && (
          <div id="bidList" ref={bidListRef}>
            {selectedBids.map((bid, idx) => (
              <BidItem key={idx} bid={bid} />
            ))}
          </div>
        )}
      </div>

      <div className="px-2 pb-2 text-gray-500">
        {(!session || !session.user) && (
          <div className="flex items-center justify-center p-2 text-lg font-semibold">
            Please login to make a bid{" "}
          </div>
        )}
        {!!session &&
          !!session.user &&
          session.user.username === auction.seller && (
            <div className="flex items-center justify-center p-2 text-lg font-semibold">
              You cannot bid on your own auction
            </div>
          )}
        {!!session &&
          !!session.user &&
          session.user.username !== auction.seller &&
          auctionOpen && (
            <BidForm auctionId={params.id} currentHighBid={currentHighestBid} />
          )}
      </div>
    </div>
  );
};

BidList.displayName = "BidList";
export default BidList;
