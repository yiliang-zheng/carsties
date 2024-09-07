"use client";
import React, { useState, useEffect, useCallback } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useAuctionStore } from "@/app/_hooks/useAuctionStore";
import { useBidStore } from "@/app/_hooks/useBidStore";

import AuctionCreatedToast from "@/app/_components/AuctionCreatedToast/AuctionCreatedToast";
import AuctionFinishedToast from "@/app/_components/AuctionFinishedToast/AuctionFinishedToast";

import { toast } from "react-hot-toast";

import type { ReactNode } from "react";
import type { HubConnection } from "@microsoft/signalr";
import type { Bid } from "@/server/schemas/bid";
import type { AuctionCreated } from "@/server/schemas/auctionCreated";
import type { AuctionFinished } from "@/server/schemas/auctionFinished";
import type { User } from "@/server/lib/auth/getCurrentUser";

const signalREvents = {
  ReceiveBidPlacedNotification: "ReceiveBidPlacedNotification",
  ReceiveAuctionCreatedNotification: "ReceiveAuctionCreatedNotification",
  ReceiveAuctionFinishedNotification: "ReceiveAuctionFinishedNotification",
} as const;

type Props = {
  children: ReactNode;
  user: User | null | undefined;
  notificationUrl: string;
};

const SignalRProvider = ({ children, user, notificationUrl }: Props) => {
  const [connection, setConnection] = useState<HubConnection | null>();
  const setCurrentHighPrice = useAuctionStore((state) => state.setCurrentPrice);
  const addBid = useBidStore((state) => state.addBid);

  const handleBidPlaced = useCallback<(bid: Bid) => void>(
    (bid: Bid) => {
      if (
        !!bid.bidStatus &&
        ["accepted", "accepted below reserve"].includes(
          bid.bidStatus.toLowerCase()
        )
      ) {
        setCurrentHighPrice(bid.auctionId, bid.amount);
      }
      addBid(bid);
    },
    [setCurrentHighPrice, addBid]
  );

  const handleAuctionCreated = useCallback<(auction: AuctionCreated) => void>(
    (auction: AuctionCreated) => {
      if (user?.username !== auction.seller) {
        toast(<AuctionCreatedToast auction={auction} />, { duration: 10000 });
      }
    },
    [user]
  );

  const handleAuctionFinished = useCallback<
    (finishedAuction: AuctionFinished) => void
  >((finishedAuction: AuctionFinished) => {
    toast(<AuctionFinishedToast finishedAuction={finishedAuction} />, {
      duration: 10000,
    });
  }, []);

  useEffect(() => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl(notificationUrl)
      .withAutomaticReconnect()
      .build();

    setConnection(hubConnection);
  }, [notificationUrl]);

  useEffect(() => {
    if (!!connection) {
      connection
        .start()
        .then(() => {
          connection.on(
            signalREvents.ReceiveBidPlacedNotification,
            handleBidPlaced
          );

          connection.on(
            signalREvents.ReceiveAuctionCreatedNotification,
            handleAuctionCreated
          );

          connection.on(
            signalREvents.ReceiveAuctionFinishedNotification,
            handleAuctionFinished
          );
        })
        .catch((error) => console.log(error));
    }

    return () => {
      connection?.off(
        signalREvents.ReceiveBidPlacedNotification,
        handleBidPlaced
      );
      connection?.off(
        signalREvents.ReceiveAuctionCreatedNotification,
        handleAuctionCreated
      );
      connection?.off(
        signalREvents.ReceiveAuctionFinishedNotification,
        handleAuctionFinished
      );
      connection?.stop();
    };
  }, [
    connection,
    addBid,
    setCurrentHighPrice,
    handleAuctionCreated,
    handleBidPlaced,
    handleAuctionFinished,
  ]);
  return <>{children}</>;
};

SignalRProvider.displayName = "SignalRProvider";
export default SignalRProvider;
