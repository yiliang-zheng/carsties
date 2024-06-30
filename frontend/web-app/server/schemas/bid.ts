import { z } from "zod";

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
  bidStatus: z.string().transform((val) => {
    const status = bidStatus.find((p) => p === val.toLowerCase());
    return status;
  }),
  auctionId: z.string().uuid(),
  auctionEnd: z.string().datetime({ offset: true }),
  seller: z.string().nullish(),
  reservePrice: z.number().nullish(),
  finished: z.boolean().nullish(),
});

export const bidArraySchema = z.array(bidSchema);

export type Bid = z.infer<typeof bidSchema>;
