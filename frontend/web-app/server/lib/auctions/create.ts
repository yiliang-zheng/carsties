import { TRPCError } from "@trpc/server";
import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";
import { createAuctionPayloadSchema } from "@/server/schemas/createAuction";
import { auctionSchema } from "@/server/schemas/auction";

import type { Auction } from "@/server/schemas/auction";
import type { CreateAuctionPayload } from "@/server/schemas/createAuction";

export const create = async (
  payload: CreateAuctionPayload,
  accessToken?: string
): Promise<Auction> => {
  const parseResult = await createAuctionPayloadSchema.safeParseAsync(payload);
  if (!parseResult.success) {
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "Invalid create auction payload",
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
    data = await fetchWrapper.post<Auction, CreateAuctionPayload>(
      "auctions",
      payload,
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
