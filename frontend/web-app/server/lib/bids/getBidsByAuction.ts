import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";
import { bidArraySchema } from "@/server/schemas/bid";
import { TRPCError } from "@trpc/server";

import type { Bid } from "@/server/schemas/bid";

export const getBidsByAuction = async (auctionId: string): Promise<Bid[]> => {
  let data: Bid[];
  try {
    data = await fetchWrapper.get<Bid[]>(`bids/${auctionId}`);
  } catch (error) {
    console.log(error);
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }

  const parsed = await bidArraySchema.safeParseAsync(data);
  if (!parsed.success) {
    console.log(parsed.error);
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "parse bid array type failed",
    });
  }

  return parsed.data;
};
