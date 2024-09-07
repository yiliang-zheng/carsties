import { cookies } from "next/headers";

import SignIn from "@/app/_components/LoginButton/SignIn";

export default async function LoginButton() {
  const csrfToken = cookies().get("authjs.csrf-token")?.value ?? "";
  return <SignIn csrfToken={csrfToken} />;
}
