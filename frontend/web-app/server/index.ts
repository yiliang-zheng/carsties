import { publicProcedure, router, createCallerFactory } from "@/server/trpc";

export const appRouter = router({
  getTodos: publicProcedure.query(async () => [10, 20, 30]),
});

export const createCaller = createCallerFactory(appRouter);
export type AppRouter = typeof appRouter;
