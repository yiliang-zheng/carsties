import { publicProcedure, router, createCallerFactory } from "@/server/trpc";
import { auctionsRoute } from "@/server/routes/auctions";

export const appRouter = router({
  getTodos: publicProcedure.query(async () => [10, 20, 30]),
  auctions: auctionsRoute,
});

export const createCaller = createCallerFactory(appRouter);
export type AppRouter = typeof appRouter;
