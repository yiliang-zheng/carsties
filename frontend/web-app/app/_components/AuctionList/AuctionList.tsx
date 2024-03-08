"use client";
import React, { useMemo } from "react";
import { trpc } from "@/app/_trpc/client";

import AuctionCard from "@/app/_components/AuctionCard/AuctionCard";

import type { Auction } from "@/server/schemas/auction";

type Props = {
  initialAuctions: Auction[] | undefined;
};

const AuctionList = ({ initialAuctions }: Props) => {
  const { data, isLoading, isError } = trpc.auctions.list.useQuery(undefined, {
    initialData: initialAuctions,
    refetchOnMount: false,
    refetchOnReconnect: false,
  });

  const auctions = useMemo(() => {
    if (!data) return [] as Auction[];

    return data;
  }, [data, isLoading]);
  return (
    <div className="grid grid-cols-1 gap-6 auto-rows-max  lg:grid-cols-3">
      {auctions.map((auction, idx) => (
        <div key={idx} className="w-full">
          <AuctionCard auction={auction} />
        </div>
      ))}
    </div>
  );
};

AuctionList.displayName = "AuctionList";
export default AuctionList;
