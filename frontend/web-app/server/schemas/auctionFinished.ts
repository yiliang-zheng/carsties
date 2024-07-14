import { z } from "zod";

export const auctionFinishedSchema = z.object({
  auctionId: z.string().uuid(),
  itemSold: z.boolean(),
  winner: z.string().nullish(),
  seller: z.string(),
  amount: z.number().nullish(),
});

export type AuctionFinished = z.infer<typeof auctionFinishedSchema>;
