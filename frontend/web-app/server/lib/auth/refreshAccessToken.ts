import type { Session } from "next-auth";
import type { JWT } from "next-auth/jwt";

type RefreshTokenResponse = {
  id_token?: string;
  access_token?: string;
  expires_in?: number;
  token_type?: string;
  refresh_token?: string;
  scope?: string;
};
export async function refreshAccessToken<T extends JWT | Session>(
  token: T
): Promise<T> {
  try {
    const url = process.env.IDENTITY_SERVER_URL_INTERNAL + "/connect/token";

    const payload = {
      client_id: process.env.IDENTITY_SERVER_CLIENT_ID!,
      client_secret: process.env.IDENTITY_SERVER_ClIENT_SECRET!,
      grant_type: "refresh_token",
      refresh_token: token.refreshToken,
    };

    console.log("payload: ", JSON.stringify(payload));
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      method: "POST",
      body: new URLSearchParams(payload),
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
    console.log("error for refreshing token: ", error);
    return {
      ...token,
      error: "RefreshAccessTokenError",
    };
  }
}
