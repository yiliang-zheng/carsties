"use client";
import Button from "@/app/_components/Core/Button";
import Link from "next/link";
const UserActions = () => {
  return (
    <Button outline onClick={() => {}}>
      <Link href="/session">Session</Link>
    </Button>
  );
};

UserActions.displayName = "UserActions";
export default UserActions;
