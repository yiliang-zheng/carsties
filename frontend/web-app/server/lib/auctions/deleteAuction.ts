import { TRPCError } from "@trpc/server";

import { deleteAuctionPayloadSchema } from "@/server/schemas/deleteAuction";
import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";

import type { DeleteAuctionPayload } from "@/server/schemas/deleteAuction";

export const deleteAuction = async (
  payload: DeleteAuctionPayload,
  accessToken?: string
): Promise<void> => {
  const parseResult = await deleteAuctionPayloadSchema.safeParseAsync(payload);
  if (!parseResult.success) {
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "Invalid delete auction payload",
    });
  }

  if (!accessToken) {
    throw new TRPCError({
      code: "UNAUTHORIZED",
      message: "access token is required",
    });
  }

  try {
    await fetchWrapper.del(`auctions/${payload.id}`, accessToken);
  } catch (error) {
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }
};
