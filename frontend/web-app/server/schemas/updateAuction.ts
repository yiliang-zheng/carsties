import { z } from "zod";
export const updateAuctionPayloadSchema = z.object({
  id: z.string().uuid(),
  make: z.string().nullish(),
  model: z.string().nullish(),
  color: z.string().nullish(),
  year: z.coerce.number().int().nullish(),
  mileage: z.coerce.number().int().nonnegative().nullish(),
});

export type UpdateAuctionPayload = z.infer<typeof updateAuctionPayloadSchema>;
