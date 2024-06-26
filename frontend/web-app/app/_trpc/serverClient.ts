import { createCaller } from "@/server";
import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";
import { getSession } from "@/server/lib/auth/getSession";
import { getToken } from "next-auth/jwt";
import { cookies, headers } from "next/headers";
import type { NextApiRequest } from "next";

export const createServerClient = async () => {
  const user = await getCurrentUser();
  const session = await getSession();
  const mockRequest = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
      cookies()
        .getAll()
        .map((p) => [p.name, p.value])
    ),
  } as NextApiRequest;
  const token = await getToken({ req: mockRequest });
  const serverClient = createCaller({
    session,
    user,
    accessToken: token?.accessToken,
  });

  return serverClient;
};
