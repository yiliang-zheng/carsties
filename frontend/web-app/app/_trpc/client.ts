import { createTRPCReact } from "@trpc/react-query";

import type { AppRouter } from "@/server";

export const trpc = createTRPCReact<AppRouter>();

export function getBaseUrl() {
  if (typeof window !== "undefined")
    // browser should use relative path
    return "";

  if (process.env.SERVER_URL) return `https://${process.env.SERVER_URL}`;

  // assume localhost
  return `http://localhost:${process.env.PORT ?? 3000}`;
}
