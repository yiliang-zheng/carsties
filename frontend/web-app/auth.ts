import NextAuth from "next-auth";
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6";
import { refreshAccessToken } from "@/server/lib/auth/refreshAccessToken";

import type { OIDCConfig } from "next-auth/providers";
import type { Profile, NextAuthConfig } from "next-auth";

const authOptions: NextAuthConfig = {
  session: {
    strategy: "jwt",
  },
  providers: [
    DuendeIDS6Provider({
      id: "id-server",
      clientId: process.env.IDENTITY_SERVER_CLIENT_ID,
      clientSecret: process.env.IDENTITY_SERVER_ClIENT_SECRET,
      issuer: process.env.IDENTITY_SERVER_URL,
      authorization: {
        url: `${process.env.IDENTITY_SERVER_URL}/connect/authorize`,
        params: { scope: "openid profile auctionSvc offline_access" },
      },
      token: {
        url: `${process.env.IDENTITY_SERVER_URL_INTERNAL}/connect/token`,
      },
      userinfo: {
        url: `${process.env.IDENTITY_SERVER_URL_INTERNAL}/connect/userinfo`,
      },
      idToken: true,
    } as OIDCConfig<Omit<Profile, "username">>),
  ],
  callbacks: {
    redirect: async ({ url, baseUrl }) => {
      return url.startsWith(baseUrl) ? url : baseUrl;
    },
    authorized: async ({ auth }) => {
      return !!auth && !auth.error;
    },
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
        session.accessToken = token.accessToken;
        session.accessTokenExpires = token.accessTokenExpires;
        session.refreshToken = token.refreshToken;
      }
      return session;
    },
  },
  pages: {
    signIn: "/signIn",
  },
  trustHost: true,
};

export const { auth, handlers, signIn, signOut } = NextAuth(authOptions);
