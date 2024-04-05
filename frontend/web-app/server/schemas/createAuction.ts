import { z } from "zod";
export const createAuctionPayloadSchema = z.object({
  make: z.string(),
  model: z.string(),
  color: z.string(),
  year: z.coerce.number().int(),
  mileage: z.coerce.number().int().nonnegative(),
  reservePrice: z.coerce.number().int().positive(),
  imageUrl: z.string().url(),
  auctionEnd: z.date(),
});

export type CreateAuctionPayload = z.infer<typeof createAuctionPayloadSchema>;
