"use server";

import { signIn } from "@/auth";

export async function login() {
  await signIn("id-server", { callbackUrl: "/" }, { prompt: "login" });
}
