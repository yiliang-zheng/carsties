"use client";
import React, { useCallback } from "react";
import Countdown, { zeroPad } from "react-countdown";
import { useParams, usePathname } from "next/navigation";
import { useBidStore } from "@/app/_hooks/useBidStore";

type Props = {
  auctionEnd: string;
};

type CountdownRenderProps = {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
};

const CountdownTimer = ({ auctionEnd }: Props) => {
  const { id } = useParams<{ id: string }>();
  const pathName = usePathname();
  const setAuctionOpen = useBidStore((state) => state.setAuctionOpen);
  const handleComplete = useCallback(() => {
    if (pathName.startsWith("/auctions/details")) {
      setAuctionOpen(id, false);
    }
  }, [setAuctionOpen, id]);
  const renderer = ({
    days,
    hours,
    minutes,
    seconds,
    completed,
  }: CountdownRenderProps) => {
    const commonStyle =
      "border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center";
    if (completed) {
      // Render a completed state
      return (
        <div className={`${commonStyle} bg-red-600`}>
          <span suppressHydrationWarning={true}>Auction Ended</span>
        </div>
      );
    }

    if (days <= 0 && hours <= 10) {
      return (
        <div className={`${commonStyle} bg-amber-600`}>
          <span suppressHydrationWarning={true}>
            {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:
            {zeroPad(seconds)}
          </span>
        </div>
      );
    }

    return (
      <div className={`${commonStyle} bg-green-600`}>
        <span suppressHydrationWarning={true}>
          {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
        </span>
      </div>
    );
  };

  return (
    <Countdown
      date={new Date(auctionEnd)}
      renderer={renderer}
      onComplete={handleComplete}
    />
  );
};

CountdownTimer.displayName = "CountdownTimer";
export default CountdownTimer;
