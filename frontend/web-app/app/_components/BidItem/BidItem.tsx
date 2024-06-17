import React, { useCallback } from "react";
import { cn } from "@/utils/cn";
import { toZonedTime, formatInTimeZone } from "date-fns-tz";

import type { Bid, BidStatus } from "@/server/schemas/bid";

type Props = {
  bid: Bid;
};

const BidItem = ({ bid }: Props) => {
  const getBidInfo = useCallback<
    () => { bgColor: string; text: string }
  >(() => {
    const bidStatus = bid.bidStatus.toLowerCase() as BidStatus;
    switch (bidStatus) {
      case "accepted":
        return {
          bgColor: "bg-green-200",
          text: "Bid accepted",
        };
      case "accepted below reserve":
        return {
          bgColor: "bg-amber-500",
          text: "Reserve not met",
        };
      case "too low":
        return {
          bgColor: "bg-red-200",
          text: "Bid was too low",
        };
      case "finished":
      default:
        return {
          bgColor: "bg-red-200",
          text: "Bid placed after auction finished",
        };
    }
  }, [bid]);

  const formatBidDateTime = useCallback(() => {
    if (!bid || !bid.bidDateTime) return "";
    const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const result = formatInTimeZone(
      bid.bidDateTime,
      timezone,
      "dd/MM/yyyy HH:mm:ss"
    );
    return result;
  }, [bid]);

  return (
    <div
      className={cn(
        "border-gray-300 border-2 px-3 py-2 rounded-lg flex flex-row justify-between items-center mb-2",
        getBidInfo().bgColor
      )}
    >
      <div className="flex flex-col">
        <span>Bidder: {bid.bidder}</span>
        <span className="text-gray-700 text-sm">
          Time: {formatBidDateTime()}
        </span>
      </div>
      <div className="flex flex-col text-right">
        <div className="text-xl font-semibold">${bid.amount}</div>
        <div className="flex flex-row items-center">
          <span>{getBidInfo().text}</span>
        </div>
      </div>
    </div>
  );
};

BidItem.displayName = "BidItem";
export default BidItem;
