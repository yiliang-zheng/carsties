import React, { useMemo } from "react";
import { useRouter } from "next/navigation";
import { useSession } from "next-auth/react";
import { trpc } from "@/app/_trpc/client";

import toast from "react-hot-toast";
import Button from "@/app/_components/Core/Button";

type Props = {
  id: string;
  seller: string;
};

const DeleteButton = ({ id, seller }: Props) => {
  const router = useRouter();
  const { data: session } = useSession();
  const { mutateAsync, isLoading } = trpc.auctions.delete.useMutation();

  const showButton = useMemo(() => {
    if (
      !session ||
      !seller ||
      !session.user ||
      session.user.username !== seller
    )
      return false;

    return true;
  }, [seller, session]);

  const handleClick = async () => {
    await mutateAsync(
      { id },
      {
        onError: (error) => {
          toast.error(`Error occurred! ${error.message}`, { icon: "🥲" });
        },
        onSuccess: () => {
          router.push("/");
        },
      }
    );
  };
  if (!showButton) return null;
  return (
    <Button color="red" isLoading={isLoading} onClick={handleClick}>
      Delete Auction
    </Button>
  );
};

DeleteButton.displayName = "DeleteButton";
export default DeleteButton;
