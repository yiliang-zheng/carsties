import React from "react";
import Link from "next/link";
import Image from "next/image";

import type { AuctionCreated } from "@/server/schemas/auctionCreated";

type Props = {
  auction: AuctionCreated;
};

const AuctionCreatedToast = ({ auction }: Props) => {
  return (
    <Link
      href={`/auctions/details/${auction.id}`}
      className="flex flex-col items-center"
    >
      <div className="flex flex-row items-center gap-2">
        <Image
          src={auction.imageUrl ?? ""}
          alt="image"
          height={80}
          width={80}
          className="rounded-lg w-auto h-auto"
        ></Image>
        <span>
          New Auction! {auction.make} {auction.model} has been added
        </span>
      </div>
    </Link>
  );
};

AuctionCreatedToast.displayName = "AuctionCreatedToast";
export default AuctionCreatedToast;
