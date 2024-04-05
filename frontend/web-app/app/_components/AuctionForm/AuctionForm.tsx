"use client";
import React from "react";
import { useRouter } from "next/navigation";
import { trpc } from "@/app/_trpc/client";
import { useForm } from "react-hook-form";
import toast from "react-hot-toast";
import Button from "@/app/_components/Core/Button";
import FormInput from "@/app/_components/FormInput/FormInput";
import FormDateInput from "@/app/_components/FormDateInput/FormDateInput";

import type { FieldValues } from "react-hook-form";
import type { CreateAuctionPayload } from "@/server/schemas/createAuction";

const AuctionForm = () => {
  const { control, handleSubmit, reset } = useForm({
    mode: "onTouched",
  });

  const router = useRouter();

  const { mutate, isLoading, error } = trpc.auctions.create.useMutation();
  const submit = async (data: FieldValues) => {
    try {
      const payload: CreateAuctionPayload = {
        make: data["make"],
        model: data["model"],
        color: data["color"],
        imageUrl: data["imageUrl"],
        year: data["year"],
        mileage: data["mileage"],
        reservePrice: data["reservePrice"],
        auctionEnd: data["auctionEnd"],
      };
      await mutate(payload, {
        onSuccess: (insertedAuction) => {
          toast.success("Auction created", {
            icon: "👏",
          });
          reset();
          router.push(`/auctions/details/${insertedAuction?.id}`, {
            scroll: true,
          });
        },
      });
    } catch (error) {
      console.log(error);
    }
  };
  return (
    <>
      {!isLoading && !!error && (
        <div role="alert" className="alert alert-error">
          <span>{JSON.stringify(error.data, null, 2)}</span>
        </div>
      )}
      <form
        className="flex flex-col mt-3"
        onSubmit={handleSubmit(submit)}
        onReset={reset}
      >
        <FormInput
          label="Make"
          name="make"
          showLabel
          control={control}
          rules={{ required: "Make is required." }}
        />
        <FormInput
          label="Model"
          name="model"
          showLabel
          control={control}
          rules={{ required: "Model is required" }}
        />
        <FormInput
          label="Color"
          name="color"
          showLabel
          control={control}
          rules={{ required: "Color is required" }}
        />
        <div className="grid grid-cols-2 gap-3">
          <FormInput
            label="Year"
            name="year"
            showLabel
            control={control}
            type="number"
            rules={{ required: "Year is required" }}
          />

          <FormInput
            label="Mileage"
            name="mileage"
            showLabel
            control={control}
            type="number"
            rules={{ required: "Mileage is required" }}
          />
        </div>
        <FormInput
          label="Image URL"
          name="imageUrl"
          showLabel
          control={control}
          type="text"
          rules={{ required: "Image URL is required" }}
        />
        <div className="grid grid-cols-2 gap-3">
          <FormInput
            label="Reserve Price"
            name="reservePrice"
            showLabel
            control={control}
            type="number"
            rules={{ required: "Reserve price is required" }}
          />

          <FormDateInput
            label="Auction End Date"
            name="auctionEnd"
            showLabel
            control={control}
            dateFormat="dd MMMM yyyy h:mm a"
            showTimeSelect
            rules={{ required: "Auction end is required" }}
          />
        </div>
        <div className="flex justify-between">
          <Button type="reset" outline color="dark">
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

AuctionForm.displayName = "AuctionForm";
export default AuctionForm;
