"use client";
import { trpc } from "@/app/_trpc/client";
import Heading from "@/app/_components/Core/Heading";
import CountdownTimer from "@/app/_components/CountdownTimer/CountdownTimer";
import AuctionCardImage from "@/app/_components/AuctionCard/AuctionCardImage";
import DetailedSpecs from "@/app/_components/DetailsSpec/DetailsSpec";
import EditButton from "@/app/_components/EditButton/EditButton";
import DeleteButton from "@/app/_components/DeleteButton/DeleteButton";
import BidList from "@/app/_components/BidList/BidList";

import type { Auction } from "@/server/schemas/auction";

type Props = {
  auction: Auction;
  id: string;
};
const AuctionDetail = ({ auction, id }: Props) => {
  const { data } = trpc.auctions.get.useQuery(
    { id },
    {
      initialData: auction,
      refetchOnMount: false,
      refetchOnWindowFocus: false,
      refetchOnReconnect: false,
    }
  );

  return (
    <div>
      <div className="flex justify-between">
        <div className="flex justify-center items-center gap-3">
          <Heading title={`${data.make} ${data.model}`} />
          <EditButton id={id} seller={data.seller} />
          <DeleteButton id={id} seller={data.seller} />
        </div>

        <div className="flex gap-3">
          <h3 className="text-2xl font-semibold">Time remaining: </h3>
          <CountdownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-6 mt-3">
        <div className="w-full bg-gray-200 aspect-h-10 aspect-w-16 rounded-lg overflow-hidden">
          <AuctionCardImage imageUrl={data.imageUrl} title={data.model} />
        </div>
        <BidList auction={auction} />
      </div>

      <div className="mt-3 grid grid-cols-1 rounded-lg">
        <DetailedSpecs auction={data} />
      </div>
    </div>
  );
};

AuctionDetail.displayName = "AuctionDetail";
export default AuctionDetail;
