import React from "react";

import Link from "next/link";
import Image from "next/image";

import { numberWithCommas } from "@/utils/number/number";

import type { AuctionFinished } from "@/server/schemas/auctionFinished";
import type { Auction } from "@/server/schemas/auction";

type Props = {
  finishedAuction: AuctionFinished;
  detail: Auction;
};
const AuctionFinishedToast = ({ finishedAuction, detail }: Props) => {
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
};

AuctionFinishedToast.displayName = "AuctionFinishedToast";
export default AuctionFinishedToast;
