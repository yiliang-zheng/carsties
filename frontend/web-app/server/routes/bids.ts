import { router, publicProcedure, protectedProcedure } from "@/server/trpc";
import { z } from "zod";
import { getBidsByAuction } from "@/server/lib/bids/getBidsByAuction";
import { placeBid } from "@/server/lib/bids/placeBid";

export const bidsRouter = router({
  getBidsByAuction: publicProcedure
    .input(z.object({ auctionId: z.string().uuid() }))
    .query(async ({ input }) => await getBidsByAuction(input.auctionId)),
  placeBid: protectedProcedure
    .input(
      z.object({
        amount: z.number().positive(),
        auctionId: z.string().uuid(),
      })
    )
    .mutation(
      async ({ input, ctx }) => await placeBid(input, ctx.accessToken ?? "")
    ),
});
