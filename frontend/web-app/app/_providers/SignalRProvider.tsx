"use client";
import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useAuctionStore } from "@/app/_hooks/useAuctionStore";
import { useBidStore } from "@/app/_hooks/useBidStore";

import type { ReactNode } from "react";
import type { HubConnection } from "@microsoft/signalr";
import type { Bid } from "@/server/schemas/bid";

type Props = {
  children: ReactNode;
};

const SignalRProvider = ({ children }: Props) => {
  const [connection, setConnection] = useState<HubConnection | null>();
  const setCurrentHighPrice = useAuctionStore((state) => state.setCurrentPrice);
  const addBid = useBidStore((state) => state.addBid);

  useEffect(() => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:6001/notifications")
      .withAutomaticReconnect()
      .build();

    setConnection(hubConnection);
  }, []);

  useEffect(() => {
    if (!!connection) {
      connection
        .start()
        .then(() => {
          connection.on("ReceiveBidPlacedNotification", (bid: Bid) => {
            if (
              bid.bidStatus === "accepted" ||
              bid.bidStatus === "accepted below reserve"
            ) {
              setCurrentHighPrice(bid.auctionId, bid.amount);
            }
            addBid(bid);
          });
        })
        .catch((error) => console.log(error));
    }

    return () => {
      connection?.stop();
    };
  }, [connection, addBid, setCurrentHighPrice]);
  return <>{children}</>;
};

SignalRProvider.displayName = "SignalRProvider";
export default SignalRProvider;
