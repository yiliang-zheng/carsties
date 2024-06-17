import { boolean, z } from "zod";

export const bidStatus = [
  "accepted",
  "accepted below reserve",
  "too low",
  "finished",
] as const;
export type BidStatus = (typeof bidStatus)[number];

export const bidSchema = z.object({
  id: z.string().uuid(),
  bidder: z.string(),
  bidDateTime: z.string().datetime({ offset: true }),
  amount: z.number().positive(),
  bidStatus: z.string(),
  auctionId: z.string().uuid(),
  auctionEnd: z.string().datetime({ offset: true }),
  seller: z.string().nullish(),
  reservePrice: z.number().nullish(),
  finished: boolean().nullish(),
});

export const bidArraySchema = z.array(bidSchema);

export type Bid = z.infer<typeof bidSchema>;
