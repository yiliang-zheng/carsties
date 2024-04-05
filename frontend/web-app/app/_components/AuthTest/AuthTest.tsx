"use client";

import React, { useMemo } from "react";
import { trpc } from "@/app/_trpc/client";
import Button from "@/app/_components/Core/Button";

export default function AuthTest() {
  const { data, mutate, isLoading, isError, error } =
    trpc.auctions.update.useMutation();

  function doUpdate() {
    mutate({
      id: "17042f47-1b82-4ee3-b786-715544379bf3",
      mileage: Math.floor(Math.random() * 100000) + 1,
    });
  }

  //   const shouldSignIn = useMemo(() => {
  //     const result = !isLoading && isError && error.data?.httpStatus === 400;
  //     return result;
  //   }, [isLoading, isError, error]);

  //   if (shouldSignIn) {
  //     signIn("id-server", { callbackUrl: "/session" });
  //     return null;
  //   }

  return (
    <div className="flex items-center gap-4">
      <Button outline isLoading={isLoading} onClick={doUpdate}>
        Test auth
      </Button>
      {!isLoading && !isError && (
        <div className="text-green-700">{JSON.stringify(data, null, 2)}</div>
      )}

      {!isLoading && isError && (
        <div className="text-red-700">
          {JSON.stringify(error.shape?.data, null, 2)}
        </div>
      )}
    </div>
  );
}
