import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";
import { TRPCError } from "@trpc/server";
import { bidSchema } from "@/server/schemas/bid";

import type { Bid } from "@/server/schemas/bid";

type PlaceBidPayload = {
  amount: number;
  auctionId: string;
};
export const placeBid = async (
  payload: PlaceBidPayload,
  accessToken: string
): Promise<Bid> => {
  let data: Bid;
  try {
    data = await fetchWrapper.post<Bid, PlaceBidPayload>(
      "bids/create",
      payload,
      accessToken
    );
  } catch (error) {
    console.log(error);
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }

  const parsed = await bidSchema.safeParseAsync(data);
  if (!parsed.success) {
    console.log(parsed.error);
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "parse bid type failed",
    });
  }

  return parsed.data;
};
