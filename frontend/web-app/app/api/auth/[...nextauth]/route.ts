import NextAuth from "next-auth";
import type { NextAuthOptions } from "next-auth";
import DuendeIdentityServerProvider from "next-auth/providers/duende-identity-server6";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: "jwt",
  },
  providers: [
    DuendeIdentityServerProvider({
      id: "id-server",
      clientId: "nextApp",
      clientSecret: "secret",
      issuer: "http://localhost:5000",
      authorization: { params: { scope: "openid profile auctionSvc" } },
      idToken: true,
    }),
  ],
  callbacks: {
    jwt: async ({ token, profile }) => {
      if (!!profile) token.username = profile.username;

      return token;
    },
    session: async ({ session, token }) => {
      if (!!token) session.user.username = token.username;

      return session;
    },
  },
};

const handler = NextAuth(authOptions);
export { handler as GET, handler as POST };
