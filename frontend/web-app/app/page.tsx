import { createServerClient } from "@/app/_trpc/serverClient";
import AuctionList from "@/app/_components/AuctionList/AuctionList";

export default async function Home() {
  const serverClient = await createServerClient();
  const result = await serverClient.auctions.list({});
  return (
    <div className="w-full items-center justify-between font-mono text-sm lg:flex">
      <AuctionList initialAuctions={result} />
    </div>
  );
}
