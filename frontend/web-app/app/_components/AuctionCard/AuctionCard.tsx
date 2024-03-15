import CountdownTimer from "@/app/_components/CountdownTimer/CountdownTimer";
import AuctionCardImage from "@/app/_components/AuctionCard/AuctionCardImage";
import type { Auction } from "@/server/schemas/auction";

type Props = {
  auction: Auction;
};

const AuctionCard = ({ auction }: Props) => {
  return (
    <div className="card bg-base-100 shadow-xl h-full group">
      <figure className="!items-start basis-2/3 relative h-full">
        <AuctionCardImage imageUrl={auction.imageUrl} title={auction.make} />
        <div className="absolute top-2 right-2">
          <CountdownTimer auctionEnd={auction.auctionEnd} />
        </div>
      </figure>
      <div className="card-body">
        <h2 className="card-title">
          {auction.make} {auction.model}
        </h2>
        <div className="card-actions justify-around stats no-scrollbar">
          <div className="stat px-2.5 py-0">
            <div className="stat-title">Year</div>
            <div className="stat-value text-primary text-sm">
              {auction.year}
            </div>
          </div>
          <div className="stat px-2.5 py-0">
            <div className="stat-title">Colour</div>
            <div className="stat-value text-secondary  text-sm">
              {auction.color}
            </div>
          </div>
          <div className="stat px-2.5 py-0">
            <div className="stat-title">Mileage</div>
            <div className="stat-value  text-sm">{auction.mileage}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

AuctionCard.displayName = "AuctionCard";
export default AuctionCard;
