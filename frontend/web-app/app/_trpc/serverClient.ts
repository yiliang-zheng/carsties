import { createCaller } from "@/server";
import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";
import { getSession } from "@/server/lib/auth/getSession";

export const createServerClient = async () => {
  const user = await getCurrentUser();
  const session = await getSession();
  const serverClient = createCaller({
    session,
    user,
    accessToken: session?.accessToken,
  });

  return serverClient;
};
