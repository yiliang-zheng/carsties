import React from "react";
import { signIn } from "@/auth";

import Button from "@/app/_components/Core/Button";

type Props = {
  csrfToken: string;
};

const SignIn = ({ csrfToken }: Props) => (
  <form
    action={async () => {
      "use server";
      await signIn("id-server", { callbackUrl: "/" }, { prompt: "login" });
    }}
  >
    <input type="hidden" name="csrfToken" value={csrfToken} />
    <Button outline color="dark" type="submit">
      Login
    </Button>
  </form>
);

SignIn.displayName = "SignIn";
export default SignIn;
