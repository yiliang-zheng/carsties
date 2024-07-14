import { z } from "zod";
import { auctionStatus } from "@/server/schemas/auction";

export const auctionCreatedSchema = z.object({
  id: z.string().uuid(),
  createdAt: z.string().datetime(),
  updatedAt: z.string().datetime().nullish(),
  auctionEnd: z.string().datetime(),
  seller: z.string().nullish(),
  winner: z.string().nullish(),
  make: z.string(),
  model: z.string(),
  year: z.number(),
  color: z.string(),
  mileage: z.number().nullish(),
  imageUrl: z.string().url(),
  status: auctionStatus,
  reservePrice: z.number().nullish(),
  soldAmount: z.number().nullish(),
  currentHighBid: z.number().nullish(),
});

export type AuctionCreated = z.infer<typeof auctionCreatedSchema>;
