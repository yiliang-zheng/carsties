"use client";
import { useForm } from "react-hook-form";
import Button from "@/app/_components/Core/Button";
import FormInput from "@/app/_components/FormInput/FormInput";

import type { FieldValues } from "react-hook-form";

const AuctionForm = () => {
  const { control, handleSubmit } = useForm();
  const submit = (data: FieldValues) => {
    console.log(data);
  };
  return (
    <form className="flex flex-col mt-3" onSubmit={handleSubmit(submit)}>
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
      <div className="flex justify-between">
        <Button type="reset" outline color="dark">
          Cancel
        </Button>
        <Button type="submit" color="dark">
          Submit
        </Button>
      </div>
    </form>
  );
};

AuctionForm.displayName = "AuctionForm";
export default AuctionForm;
