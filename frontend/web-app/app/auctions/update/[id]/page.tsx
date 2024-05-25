import React from "react";
import { createServerClient } from "@/app/_trpc/serverClient";

import AuctionForm from "@/app/_components/AuctionForm/AuctionForm";
import Heading from "@/app/_components/Core/Heading";

type Props = {
  params: {
    id: string;
  };
};
export default async function Page({ params }: Props) {
  const serverClient = await createServerClient();
  const data = await serverClient.auctions.get({ id: params.id });
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading
        title="Update your auction"
        subtitle="Please update the details of your car"
      />
      <AuctionForm auction={data} />
    </div>
  );
}
