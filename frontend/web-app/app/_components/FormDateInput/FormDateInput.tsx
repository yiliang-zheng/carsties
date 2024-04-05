"use client";
import React, { useMemo } from "react";
import { cn } from "@/utils/cn";
import { useController } from "react-hook-form";
import DatePicker from "react-datepicker";

import type { Color } from "@/app/_components/Core/Input";
import type { UseControllerProps } from "react-hook-form";
import type { ReactDatePickerProps } from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

type Props = {
  label: string;
  type?: string;
  placeholder?: string;
  showLabel?: boolean;
} & UseControllerProps &
  Partial<ReactDatePickerProps>;

const FormDateInput = (props: Props) => {
  const { showLabel, ...inputProps } = props;
  const { field, fieldState } = useController({
    name: props.name,
    control: props.control,
    disabled: props.disabled,
    rules: props.rules,
    shouldUnregister: props.shouldUnregister,
    defaultValue: "",
  });

  const { ref, ...restField } = field;

  const color = useMemo<Color>(() => {
    if (fieldState.isDirty && !fieldState.error) return "success";
    if (!!fieldState.error) return "failure";

    return "default";
  }, [field, fieldState]);

  return (
    <div className="mb-3 block">
      {props.showLabel && (
        <div className="mb-2 block">
          <label htmlFor={field.name}>{props.label}</label>
        </div>
      )}
      <DatePicker
        {...inputProps}
        {...restField}
        onChange={(date) => field.onChange(date)}
        selected={field.value}
        placeholderText={props.placeholder}
        className={cn("border text-sm rounded-lg block w-full p-2.5", {
          "bg-red-50 border-red-500 text-red-900 placeholder-red-700 focus:ring-red-500 focus:border-red-500":
            color === "failure",
          "bg-green-50 border-green-500 text-green-900 placeholder-green-700 focus:ring-green-500 focus:border-green-500":
            color === "success",
          "bg-gray-50 border-gray-300 text-gray-900 focus:ring-blue-500 focus:border-blue-500":
            color === "default",
        })}
      />
      {!!fieldState.error && (
        <p className="mt-2 text-sm text-red-600">{fieldState.error.message}</p>
      )}
    </div>
  );
};

FormDateInput.displayName = "FormDateInput";
export default FormDateInput;
