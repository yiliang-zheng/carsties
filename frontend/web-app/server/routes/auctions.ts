import { router, publicProcedure } from "@/server/trpc";
import { z } from "zod";
import { get } from "@/server/lib/auctions/get";
import { list } from "@/server/lib/auctions/list";

export const auctionsRoute = router({
  get: publicProcedure
    .input(
      z.object({
        id: z.string().uuid(),
      })
    )
    .query(async ({ input }) => await get(input.id)),
  list: publicProcedure
    .input(
      z.object({
        searchTerm: z.string().nullish(),
        pageSize: z.number().nullish(),
        pageNumber: z.number().nullish(),
        seller: z.string().nullish(),
        winner: z.string().nullish(),
        orderBy: z.string().nullish(),
        filterBy: z.string().nullish(),
      })
    )
    .query(
      async ({ input }) =>
        await list(
          input.searchTerm,
          input.pageSize ?? undefined,
          input.pageNumber ?? undefined,
          input.seller,
          input.winner,
          input.orderBy,
          input.filterBy
        )
    ),
});
