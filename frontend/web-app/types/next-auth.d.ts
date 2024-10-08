import { DefaultSession, Profile } from "next-auth";
import { DefaultJWT } from "next-auth/jwt";

declare module "next-auth" {
  interface Session {
    user: {
      username: string;
    } & DefaultSession["user"];
    error?: string;
    accessToken: string;
    accessTokenExpires: number;
    refreshToken: string;
  }

  interface Profile {
    username: string;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    user: {
      username: string;
    } & DefaultJWT;
    username: string;
    accessToken: string;
    accessTokenExpires: number;
    refreshToken: string;
    error?: string;
  }
}
