import { z } from "zod";

export function createPagedResultSchema<T extends z.ZodTypeAny>(items: T) {
  return z.object({
    results: z.array(items),
    totalCount: z.number().int(),
    pageCount: z.number().int(),
  });
}
