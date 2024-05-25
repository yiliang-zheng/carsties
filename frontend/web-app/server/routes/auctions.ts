import { router, publicProcedure, protectedProcedure } from "@/server/trpc";
import { z } from "zod";
import { get } from "@/server/lib/auctions/get";
import { list } from "@/server/lib/auctions/list";
import { create } from "@/server/lib/auctions/create";
import { update } from "@/server/lib/auctions/update";
import { deleteAuction } from "@/server/lib/auctions/deleteAuction";
import { createAuctionPayloadSchema } from "@/server/schemas/createAuction";
import { updateAuctionPayloadSchema } from "@/server/schemas/updateAuction";
import { deleteAuctionPayloadSchema } from "../schemas/deleteAuction";

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
    .query(async ({ input }) => {
      return await list(
        input.searchTerm,
        input.pageSize ?? undefined,
        input.pageNumber ?? undefined,
        input.seller,
        input.winner,
        input.orderBy,
        input.filterBy
      );
    }),
  create: protectedProcedure
    .input(createAuctionPayloadSchema)
    .mutation(
      async ({ input, ctx }) => await create(input, ctx.accessToken ?? "")
    ),
  update: protectedProcedure
    .input(updateAuctionPayloadSchema)
    .mutation(
      async ({ input, ctx }) => await update(input, ctx.accessToken ?? "")
    ),
  delete: protectedProcedure
    .input(deleteAuctionPayloadSchema)
    .mutation(
      async ({ input, ctx }) =>
        await deleteAuction(input, ctx.accessToken ?? "")
    ),
});
