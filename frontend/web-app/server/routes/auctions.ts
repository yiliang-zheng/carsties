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
  list: publicProcedure.query(async () => await list()),
});
