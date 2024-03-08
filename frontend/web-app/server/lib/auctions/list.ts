import { arrayAuctionSchema } from "@/server/schemas/auction";
import type { Auction } from "@/server/schemas/auction";

export const list = async (): Promise<Auction[]> => {
  const response = await fetch("http://localhost:6001/auctions/", {
    method: "GET",
  });
  if (!response.ok) throw new Error(response.statusText);

  const data = await response.json();
  const parsed = await arrayAuctionSchema.safeParseAsync(data);
  if (!parsed.success) throw new Error("parse auction type failed");

  return parsed.data;
};
