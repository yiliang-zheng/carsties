import { z } from "zod";

const auctionStatus = z.enum(["Live", "Finished", "Reserve Not Met"]);

export const auctionSchema = z.object({
  id: z.string().uuid(),
  createdAt: z.string().datetime(),
  updatedAt: z.string().datetime().nullish(),
  auctionEnd: z.string().datetime(),
  seller: z.string(),
  winner: z.string().nullish(),
  make: z.string(),
  model: z.string(),
  year: z.number().int(),
  color: z.string(),
  mileage: z.number().int().nonnegative(),
  imageUrl: z.string().url(),
  status: auctionStatus,
  reservePrice: z.number().int(),
  soldAmount: z.number().int().positive().nullish(),
  currentHighBid: z.number().int().positive().nullish(),
});

export const arrayAuctionSchema = z.array(auctionSchema);

export type Auction = z.infer<typeof auctionSchema>;
