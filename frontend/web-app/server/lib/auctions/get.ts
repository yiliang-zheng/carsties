import { auctionSchema } from "@/server/schemas/auction";
import type { Auction } from "@/server/schemas/auction";

export const get = async (id: string): Promise<Auction> => {
  const response = await fetch(`http://localhost:6001/auctions/${id}`, {
    method: "GET",
  });
  if (!response.ok) throw new Error(response.statusText);

  const data = await response.json();
  const parsed = await auctionSchema.safeParseAsync(data);
  if (!parsed.success) throw new Error("parse auction type failed");

  return parsed.data;
};
