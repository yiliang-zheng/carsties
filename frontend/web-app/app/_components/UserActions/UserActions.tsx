"use client";
import { signOut } from "next-auth/react";
import Button from "@/app/_components/Core/Button";
import Dropdown from "@/app/_components/Core/Dropdown";
import Link from "next/link";

import type { User } from "@/server/lib/auth/getCurrentUser";

type Props = {
  user: User;
};
const UserActions = ({ user }: Props) => {
  const DropDownButton = (
    <Button outline onClick={() => {}}>
      Welcome {user.name}
    </Button>
  );
  return (
    <Dropdown button={DropDownButton}>
      <Dropdown.Item key="my-auctions">
        <Link href="/">My Auctions</Link>{" "}
      </Dropdown.Item>
      <Dropdown.Item key="auctions-won">
        <Link href="/">Auctions Won</Link>
      </Dropdown.Item>
      <Dropdown.Item key="sell-my-car">
        <Link href="/">Sell My Car</Link>
      </Dropdown.Item>
      <Dropdown.Item key="session">
        <Link href="/session">Session</Link>
      </Dropdown.Item>
      <div className="divider" key="divider" />
      <Dropdown.Item key="sign-out">
        <a
          href="#"
          onClick={(e) => {
            e.preventDefault();
            signOut({ callbackUrl: "/" });
          }}
        >
          Sign Out
        </a>
      </Dropdown.Item>
    </Dropdown>
  );
};

UserActions.displayName = "UserActions";
export default UserActions;
