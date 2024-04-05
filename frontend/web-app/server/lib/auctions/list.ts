import { fetchWrapper, getErrorMessage } from "@/server/lib/fetchWrapper";
import { pagedAuctionSchema } from "@/server/schemas/auction";

import type { PagedAuction } from "@/server/schemas/auction";
import { TRPCError } from "@trpc/server";

export const list = async (
  searchTerm: string | null | undefined,
  pageSize: number = 6,
  pageNumber: number = 1,
  seller: string | null | undefined,
  winner: string | null | undefined,
  orderBy: string | null | undefined,
  filterBy: string | null | undefined
): Promise<PagedAuction> => {
  const query = JSON.stringify({
    searchTerm,
    pageSize,
    pageNumber,
    seller,
    winner,
    orderBy,
    filterBy,
  });

  let data: PagedAuction;
  try {
    data = await fetchWrapper.get(`search?query=${query}`);
  } catch (error) {
    console.log(error);
    throw new TRPCError({
      code: "BAD_REQUEST",
      message: getErrorMessage(error),
    });
  }

  const parsed = await pagedAuctionSchema.safeParseAsync(data);

  if (!parsed.success) {
    console.log(parsed.error);
    throw new TRPCError({
      code: "PARSE_ERROR",
      message: "parse auction type failed",
    });
  }

  return parsed.data;
};
