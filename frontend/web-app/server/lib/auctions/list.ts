import { pagedAuctionSchema } from "@/server/schemas/auction";

import type { PagedAuction } from "@/server/schemas/auction";

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
  const response = await fetch(`http://localhost:6001/search?query=${query}`, {
    method: "GET",
  });
  if (!response.ok) throw new Error(response.statusText);

  const data = await response.json();

  const parsed = await pagedAuctionSchema.safeParseAsync(data);

  if (!parsed.success) {
    console.log(parsed.error);
    throw new Error("parse auction type failed");
  }

  return parsed.data;
};
