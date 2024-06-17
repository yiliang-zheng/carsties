import React, { useEffect, useMemo } from "react";
import { trpc } from "@/app/_trpc/client";
import { useParams } from "next/navigation";
import { useBidStore } from "@/app/_hooks/useBidStore";
import { useShallow } from "zustand/react/shallow";

import BidItem from "@/app/_components/BidItem/BidItem";
import Heading from "@/app/_components/Core/Heading";
import EmptyList from "@/app/_components/EmptyList/EmptyList";
import BidForm from "@/app/_components/BidForm/BidForm";

const BidList = () => {
  const params = useParams<{ id: string }>();
  const bidsStore = useBidStore(
    useShallow((state) => ({
      bids: state.bids,
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
    if (!isLoading && !error) bidsStore.setBids(bids);
  }, [bids, isLoading]);

  const currentHighestBid = useMemo<number | null>(() => {
    if (!bids) return null;
    const result = bids.reduce((prev, current) => {
      if (current.amount > prev) return current.amount;
      return prev;
    }, 0);
    return result;
  }, [bidsStore.bids]);

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
        {bidsStore.bids.length === 0 && (
          <EmptyList
            title="No bids for this item"
            subtitle="Please feel free to make a bid"
            showReset={false}
          />
        )}
        {bidsStore.bids.length > 0 && (
          <>
            {bidsStore.bids.map((bid, idx) => (
              <BidItem key={idx} bid={bid} />
            ))}
          </>
        )}
      </div>

      <div className="px-2 pb-2 text-gray-500">
        <BidForm auctionId={params.id} currentHighBid={currentHighestBid} />
      </div>
    </div>
  );
};

BidList.displayName = "BidList";
export default BidList;
