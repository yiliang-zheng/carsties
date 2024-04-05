import { TRPCError } from "@trpc/server";
import { auctionSchema } from "@/server/schemas/auction";
import { updateAuctionPayloadSchema } from "@/server/schemas/updateAuction";
import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";

import type { Auction } from "@/server/schemas/auction";
import type { UpdateAuctionPayload } from "@/server/schemas/updateAuction";

export const update = async (
  payload: UpdateAuctionPayload,
  accessToken?: string
): Promise<Auction> => {
  const parseResult = await updateAuctionPayloadSchema.safeParseAsync(payload);
  if (!parseResult.success) {
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "Invalid update auction payload",
    });
  }

  if (!accessToken) {
    throw new TRPCError({
      code: "UNAUTHORIZED",
      message: "access token is required",
    });
  }

  let data: Auction;
  try {
    const { id, ...updatePayload } = payload;
    data = await fetchWrapper.put<Auction, Partial<UpdateAuctionPayload>>(
      `auctions/${payload.id}`,
      updatePayload,
      accessToken
    );
  } catch (error) {
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }

  const parsed = await auctionSchema.safeParseAsync(data);

  if (!parsed.success) {
    console.log(parsed.error);
    throw new Error("parse auction type failed");
  }

  return parsed.data;
};
