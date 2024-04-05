import { CallbacksOptions } from "next-auth";
import type { JWT } from "next-auth/jwt";

type RefreshTokenResponse = {
  id_token?: string;
  access_token?: string;
  expires_in?: number;
  token_type?: string;
  refresh_token?: string;
  scope?: string;
};
export async function refreshAccessToken(token: JWT): Promise<JWT> {
  try {
    const url = process.env.IDENTITY_SERVER_URL + "/connect/token";

    const payload = new URLSearchParams({
      client_id: process.env.IDENTITY_SERVER_CLIENT_ID ?? "",
      client_secret: process.env.IDENTITY_SERVER_CLIENT_SECRET ?? "",
      grant_type: "refresh_token",
      refresh_token: token.refreshToken,
    });

    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      method: "POST",
      body: payload,
    });

    if (!response.ok) throw new Error(response.statusText);

    const refreshedToken = (await response.json()) as RefreshTokenResponse;
    return {
      ...token,
      accessToken: refreshedToken.access_token ?? "",
      accessTokenExpires: Date.now() + (refreshedToken.expires_in ?? 0) * 1000,
      refreshToken: refreshedToken.refresh_token ?? token.refreshToken,
    };
  } catch (error) {
    return {
      ...token,
      error: "RefreshAccessTokenError",
    };
  }
}
