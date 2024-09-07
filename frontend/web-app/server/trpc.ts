import { initTRPC, TRPCError } from "@trpc/server";
import superjson from "superjson";
import { getSession } from "@/server/lib/auth/getSession";
import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";
import { auth } from "@/auth";

import type { Session } from "next-auth";
import type { NextRequest } from "next/server";
import { refreshAccessToken } from "./lib/auth/refreshAccessToken";

type CreateContextOptions = {
  session: Session | null | undefined;
  user:
    | {
        name: string | null;
        email: string | null;
      }
    | null
    | undefined;
  accessToken: string | null | undefined;
};

const createInnerTRPCContext = (opts: CreateContextOptions) => {
  return {
    session: opts.session,
    user: opts.user,
    accessToken: opts.accessToken,
  };
};

export const createTRPCContext = async (opts: { req: NextRequest }) => {
  let session = await getSession();
  //access token expired, rotate access token by refresh token
  if (!!session && session.accessTokenExpires < new Date().getTime()) {
    session = await refreshAccessToken(session);
  }

  return createInnerTRPCContext({
    session,
    user: { name: session?.user.name ?? "", email: session?.user.email ?? "" },
    accessToken: session?.accessToken,
  });
};

const t = initTRPC
  .context<Awaited<ReturnType<typeof createTRPCContext>>>()
  .create({
    transformer: superjson,
    errorFormatter({ shape }) {
      return shape;
    },
  });

const enforceUserIsAuthed = t.middleware(({ ctx, next }) => {
  if (!ctx.session || !ctx.session.user) {
    throw new TRPCError({ code: "UNAUTHORIZED" });
  }
  return next({
    ctx: {
      // infers the `session` as non-nullable
      session: { ...ctx.session, user: ctx.session.user },
    },
  });
});
export const router = t.router;
export const publicProcedure = t.procedure;
export const protectedProcedure = t.procedure.use(enforceUserIsAuthed);
export const createCallerFactory = t.createCallerFactory;
