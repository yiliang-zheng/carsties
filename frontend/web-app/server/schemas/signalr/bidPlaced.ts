import { z } from "zod";

export const bidPlacedSchema = z.object({
  id: z.string().uuid(),
  bidder: z.string(),
  bidTime: z.string().datetime({ offset: true }),
  amount: z.number().positive(),
  bidStatus: z.string(),
  auctionId: z.string().uuid(),
});

export type BidPlaced = z.infer<typeof bidPlacedSchema>;
