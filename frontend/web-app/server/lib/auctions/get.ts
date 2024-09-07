import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";
import { auctionSchema } from "@/server/schemas/auction";
import { TRPCError } from "@trpc/server";

import type { Auction } from "@/server/schemas/auction";

export const get = async (id: string): Promise<Auction> => {
  let data: Auction;
  try {
    data = await fetchWrapper.get<Auction>(`auctions/${id}`);
    console.log(data);
  } catch (error) {
    console.log(error);
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }

  const parsed = await auctionSchema.safeParseAsync(data);
  if (!parsed.success)
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "parse auction type failed",
    });

  return parsed.data;
};
