import NextAuth from "next-auth";
import DuendeIdentityServerProvider from "next-auth/providers/duende-identity-server6";
import { refreshAccessToken } from "@/server/lib/auth/refreshAccessToken";

import type { NextAuthOptions } from "next-auth";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: "jwt",
  },
  providers: [
    DuendeIdentityServerProvider({
      id: "id-server",
      clientId: "nextApp",
      clientSecret: "secret",
      issuer: process.env.IDENTITY_SERVER_URL,
      authorization: {
        params: { scope: "openid profile auctionSvc offline_access" },
      },
      idToken: true,
    }),
  ],
  callbacks: {
    jwt: async ({ token, profile, account }) => {
      if (!!profile) token.username = profile.username;

      //initial sign in
      if (!!account && !!account.access_token && !!account.expires_at) {
        token.accessToken = account.access_token;
        token.accessTokenExpires = account.expires_at;
        token.refreshToken = account.refresh_token ?? "";
      }

      //return current token if access_token not expired
      if (Date.now() < token.accessTokenExpires) {
        return token;
      }

      //access token expired, get new access token from refresh token
      const refreshedToken = await refreshAccessToken(token);
      return refreshedToken;
    },
    session: async ({ session, token }) => {
      if (!!token) {
        session.user.username = token.username;
        session.error = token.error;
      }

      return session;
    },
  },
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
