import { z } from "zod";

export const deleteAuctionPayloadSchema = z.object({
  id: z.string().uuid(),
});

export type DeleteAuctionPayload = z.infer<typeof deleteAuctionPayloadSchema>;
