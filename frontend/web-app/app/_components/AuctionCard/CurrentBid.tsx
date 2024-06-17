import React, { useMemo } from "react";

import { cn } from "@/utils/cn";

type Props = {
  amount?: number | null;
  reservePrice: number;
};

const CurrentBid = ({ amount, reservePrice }: Props) => {
  const text = useMemo(() => {
    if (!amount) return "No bids";
    return `$${amount}`;
  }, [amount]);

  return (
    <div
      className={cn(
        "border-2 border-white text-white py-1 px-2 rounded-lg flex flex-row justify-center",
        {
          "bg-green-600": !!amount && amount > reservePrice,
        },
        {
          "bg-amber-600": !!amount && amount <= reservePrice,
        },
        {
          "bg-red-600": !amount,
        }
      )}
    >
      {text}
    </div>
  );
};

CurrentBid.displayName = "CurrentBid";
export default CurrentBid;
