"use client";

import { useRouter } from "next/navigation";
import { useSession } from "next-auth/react";
import { useMemo } from "react";
import Button from "../Core/Button";

type Props = {
  seller: string;
  id: string;
};

const EditButton = ({ id, seller }: Props) => {
  const router = useRouter();
  const { data: session } = useSession();

  const showButton = useMemo(() => {
    if (!session || !seller || session.user.username !== seller) return false;
    return true;
  }, [session, seller]);

  if (!showButton) return null;

  return (
    <Button
      outline
      type="button"
      onClick={() => router.push(`/auctions/update/${id}`)}
    >
      Update Auction
    </Button>
  );
};

EditButton.displayName = "EditButton";
export default EditButton;
