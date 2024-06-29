import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useAuctionStore } from "@/app/_hooks/useAuctionStore";
import { useBidStore } from "@/app/_hooks/useBidStore";

import type { ReactNode } from "react";
import type { HubConnection } from "@microsoft/signalr";
import type { BidPlaced } from "@/server/schemas/signalr/bidPlaced";

type Props = {
  children: ReactNode;
};

const SignalRProvider = ({ children }: Props) => {
  const [connection, setConnection] = useState<HubConnection | null>();
  const setCurrentHighPrice = useAuctionStore((state) => state.setCurrentPrice);
  const addBid = useBidStore((state) => state.addBid);

  useEffect(() => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl("")
      .withAutomaticReconnect()
      .build();

    setConnection(hubConnection);
  }, []);

  useEffect(() => {
    if (!!connection) {
      connection.start().then(() => {
        connection.on("ReceiveBidPlacedNotification", (bid: BidPlaced) => {
          if (bid.bidStatus.toLowerCase() === "accepted") {
          }
        });
      });
    }
  }, [connection]);
  return <>{children}</>;
};
