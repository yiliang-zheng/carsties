import { publicProcedure, router, createCallerFactory } from "@/server/trpc";
import { auctionsRoute } from "@/server/routes/auctions";
import { bidsRouter } from "@/server/routes/bids";

export const appRouter = router({
  getTodos: publicProcedure.query(async () => [10, 20, 30]),
  auctions: auctionsRoute,
  bids: bidsRouter,
});

export const createCaller = createCallerFactory(appRouter);
export type AppRouter = typeof appRouter;
