"use client";
import { signOut } from "next-auth/react";
import Button from "@/app/_components/Core/Button";
import Dropdown from "@/app/_components/Core/Dropdown";
import Link from "next/link";
import { useRouter, usePathname } from "next/navigation";
import { useParamsStore } from "@/app/_hooks/useParamsStore";

import type { User } from "@/server/lib/auth/getCurrentUser";

type Props = {
  user: User;
};
const UserActions = ({ user }: Props) => {
  const setParams = useParamsStore((state) => state.setParams);
  const router = useRouter();
  const pathname = usePathname();
  const DropDownButton = (
    <Button outline onClick={() => {}}>
      Welcome {user.name}
    </Button>
  );

  const setWinner = () => {
    setParams({ winner: user.username, seller: undefined });
    if (pathname !== "/") router.push("/");
  };

  const setSeller = () => {
    setParams({ winner: undefined, seller: user.username });
    if (pathname !== "/") router.push("/");
  };
  return (
    <Dropdown button={DropDownButton}>
      <Dropdown.Item key="my-auctions">
        <a
          href="#"
          onClick={(e) => {
            e.preventDefault();
            setSeller();
          }}
        >
          My Auctions
        </a>
      </Dropdown.Item>
      <Dropdown.Item key="auctions-won">
        <a
          href="#"
          onClick={(e) => {
            e.preventDefault();
            setWinner();
          }}
        >
          Auctions Won
        </a>
      </Dropdown.Item>
      <Dropdown.Item key="sell-my-car">
        <Link href="/auctions/create">Sell My Car</Link>
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
