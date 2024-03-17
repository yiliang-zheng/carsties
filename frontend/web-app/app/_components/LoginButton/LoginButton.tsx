"use client";

import Button from "@/app/_components/Core/Button";
import { signIn } from "next-auth/react";
const LoginButton = () => {
  return (
    <Button
      outline
      children="Login"
      color="dark"
      onClick={() => signIn("id-server", { callbackUrl: "/" })}
    />
  );
};

LoginButton.displayName = "LoginButton";
export default LoginButton;
