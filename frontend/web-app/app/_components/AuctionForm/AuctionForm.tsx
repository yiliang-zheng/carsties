"use client";
import React, { useCallback, useEffect, useMemo } from "react";
import { usePathname, useRouter, useParams } from "next/navigation";
import { trpc } from "@/app/_trpc/client";
import { useForm } from "react-hook-form";

import toast from "react-hot-toast";
import Button from "@/app/_components/Core/Button";
import FormInput from "@/app/_components/FormInput/FormInput";
import FormDateInput from "@/app/_components/FormDateInput/FormDateInput";

import type { FieldValues } from "react-hook-form";
import type { CreateAuctionPayload } from "@/server/schemas/createAuction";
import type { Auction } from "@/server/schemas/auction";
import type { UpdateAuctionPayload } from "@/server/schemas/updateAuction";

const formMode = ["create", "edit"] as const;
type FormMode = (typeof formMode)[number];

type Props = {
  auction?: Auction;
};

const AuctionForm = ({ auction }: Props) => {
  const { control, handleSubmit, setFocus, reset } = useForm({
    mode: "onTouched",
  });

  const pathname = usePathname();
  const params = useParams<{ id: string }>();
  const router = useRouter();
  const trpcUtils = trpc.useUtils();

  const {
    mutate: createMutate,
    isLoading: createLoading,
    error: createError,
  } = trpc.auctions.create.useMutation();

  const {
    mutate: updateMutate,
    isLoading: updateLoading,
    error: updateError,
  } = trpc.auctions.update.useMutation();

  const currentMode = useMemo<FormMode>(() => {
    //pathname is /auctions/create and auction prop is null
    if (!auction && pathname === "/auctions/create") return "create";
    return "edit";
  }, [auction, pathname]);

  const submitCreate = async (payload: CreateAuctionPayload) => {
    await createMutate(payload, {
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
  };

  const submitUpdate = async (payload: UpdateAuctionPayload) => {
    await updateMutate(payload, {
      onSuccess: ({ id, make, model, color, imageUrl, year, mileage }) => {
        trpcUtils.auctions.get.invalidate({ id: id });
        toast.success("Auction updated", {
          icon: "👏",
        });
        reset({
          make,
          model,
          color,
          imageUrl,
          year,
          mileage,
        });
        router.push(`/auctions/details/${id}`, {
          scroll: true,
        });
      },
    });
  };

  const submit = async (data: FieldValues) => {
    try {
      if (currentMode === "create") {
        const createPayload: CreateAuctionPayload = {
          make: data["make"],
          model: data["model"],
          color: data["color"],
          imageUrl: data["imageUrl"],
          year: data["year"],
          mileage: data["mileage"],
          reservePrice: data["reservePrice"],
          auctionEnd: data["auctionEnd"],
        };
        await submitCreate(createPayload);
      } else {
        const updatePayload: UpdateAuctionPayload = {
          id: auction?.id ?? "",
          color: data["color"],
          make: data["make"],
          model: data["model"],
          mileage: data["mileage"],
          year: data["year"],
        };

        await submitUpdate(updatePayload);
      }
    } catch (error) {
      console.log(error);
    }
  };

  const cancelForm = useCallback(() => {
    switch (currentMode) {
      case "create":
        reset();
        break;
      case "edit":
        router.push(`/auctions/details/${params.id}`, {
          scroll: true,
        });
        break;
      default:
        break;
    }
  }, [currentMode]);

  useEffect(() => {
    setFocus("make");
  }, [setFocus]);

  useEffect(() => {
    if (!!auction) {
      reset({
        make: auction.make,
        model: auction.model,
        color: auction.color,
        imageUrl: auction.imageUrl,
        year: auction.year,
        mileage: auction.mileage,
      });
    }
  }, [auction]);

  const submitResult = useMemo(() => {
    switch (currentMode) {
      case "create":
        return {
          isLoading: createLoading,
          error: createError,
        };
      case "edit":
        return {
          isLoading: updateLoading,
          error: updateError,
        };
      default:
        return {
          isLoading: false,
          error: null,
        };
    }
  }, [createLoading, createError, updateLoading, updateError]);

  return (
    <>
      {!submitResult.isLoading && !!submitResult.error && (
        <div role="alert" className="alert alert-error">
          <span>{JSON.stringify(submitResult.error.data, null, 2)}</span>
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
        {currentMode === "create" && (
          <>
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
          </>
        )}

        <div className="flex justify-between">
          <Button type="button" onClick={cancelForm} outline color="dark">
            Cancel
          </Button>
          <Button type="submit" color="dark" isLoading={submitResult.isLoading}>
            Submit
          </Button>
        </div>
      </form>
    </>
  );
};

AuctionForm.displayName = "AuctionForm";
export default AuctionForm;
