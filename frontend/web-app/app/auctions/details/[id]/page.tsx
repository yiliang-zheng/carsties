import AuctionDetail from "@/app/_components/AuctionDetail/AuctionDetail";
import { createServerClient } from "@/app/_trpc/serverClient";

type Props = {
  params: { id: string };
};

export default async function Details({ params }: Props) {
  const serverClient = await createServerClient();
  const data = await serverClient.auctions.get({ id: params.id });

  return <AuctionDetail auction={data} id={params.id} />;
}
