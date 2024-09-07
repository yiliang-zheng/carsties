import React from "react";

import { trpc } from "@/app/_trpc/client";

import Link from "next/link";
import Image from "next/image";

import { numberWithCommas } from "@/utils/number/number";

import type { AuctionFinished } from "@/server/schemas/auctionFinished";
import type { Auction } from "@/server/schemas/auction";

type Props = {
  finishedAuction: AuctionFinished;
};
const AuctionFinishedToast = ({ finishedAuction }: Props) => {
  const {
    data: detail,
    isLoading,
    error,
  } = trpc.auctions.get.useQuery({ id: finishedAuction.auctionId });
  if (!detail && !!isLoading) return "Loading...";

  if (!!detail && !isLoading) {
    return (
      <Link
        href={`/auctions/details/${detail.id}`}
        className="flex flex-col items-center"
      >
        <div className="flex flex-row items-center gap-2">
          <Image
            src={detail.imageUrl}
            alt="image"
            height={80}
            width={80}
            className="rounded-lg w-auto h-auto"
          ></Image>
          <div className="flex flex-col">
            <span>
              Auction for {detail.make} {detail.model} has finished
            </span>
            {finishedAuction.itemSold && !!finishedAuction.amount ? (
              <p>
                Congrats to {finishedAuction.winner} who won the auction for $
                {numberWithCommas(finishedAuction.amount)}
              </p>
            ) : (
              <p>The item did not sell</p>
            )}
          </div>
        </div>
      </Link>
    );
  }

  return (
    <div className="flex flex-col items-center justify-center gap-2">
      <p className="font-bold text-base">Error Occurred</p>
      <p className="text-sm">{error.message}</p>
    </div>
  );
};

AuctionFinishedToast.displayName = "AuctionFinishedToast";
export default AuctionFinishedToast;
