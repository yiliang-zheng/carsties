import AuctionDetail from "@/app/_components/AuctionDetail/AuctionDetail";
import { serverClient } from "@/app/_trpc/serverClient";
import { Auction } from "@/server/schemas/auction";

type Props = {
  params: { id: string };
};

export default async function Details({ params }: Props) {
  //const data = await serverClient.auctions.get({ id: params.id });
  const data = await serverClient.auctions.list({});
  console.log(data);

  return (
    <AuctionDetail
      auction={{} as Auction}
      id="9559be6f-b481-4126-9dda-85375758fdcd"
    />
  );
}
