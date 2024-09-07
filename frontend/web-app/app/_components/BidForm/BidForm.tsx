import React from "react";
import { trpc } from "@/app/_trpc/client";
import { useForm } from "react-hook-form";
import { useBidStore } from "@/app/_hooks/useBidStore";

import FormInput from "@/app/_components/FormInput/FormInput";
import Button from "@/app/_components/Core/Button";
import Alert from "@/app/_components/Core/Alert";

import type { FieldValues } from "react-hook-form";

type Props = {
  auctionId: string;
  currentHighBid?: number | null;
};
const BidForm = ({ auctionId, currentHighBid }: Props) => {
  const { addBid } = useBidStore((state) => ({
    addBid: state.addBid,
  }));
  const {
    mutate: placeBidMutate,
    isLoading,
    error,
    reset: mutationReset,
  } = trpc.bids.placeBid.useMutation();

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm({ mode: "onChange" });

  const onSubmit = async (data: FieldValues) => {
    await placeBidMutate(
      {
        auctionId,
        amount: Number(data["bidAmount"]),
      },
      {
        onSuccess: (bid) => {
          addBid(bid);
          reset();
        },
      }
    );
  };

  return (
    <>
      {!!error && <Alert type="error" title={error.message} className="m-2" />}
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex flex-row gap-2 justify-between items-start border-2 rounded-lg p-2"
      >
        <FormInput
          type="number"
          name="bidAmount"
          control={control}
          label="Bid amount"
          placeholder={`Enter your bid (minimum bid is $${
            currentHighBid ?? 0 + 1
          })`}
          className="grow"
          rules={{ required: "bid amount is required" }}
        />

        <div className="flex justify-between">
          <Button
            type="button"
            onClick={() => {
              reset();
              mutationReset();
            }}
            outline
            color="dark"
          >
            Cancel
          </Button>
          <Button type="submit" color="dark" isLoading={isLoading}>
            Submit
          </Button>
        </div>
      </form>
    </>
  );
};

BidForm.displayName = "BidForm";
export default BidForm;
