"use client";
import React, { useMemo } from "react";
import { useController } from "react-hook-form";
import { cn } from "@/utils/cn";

import Input from "@/app/_components/Core/Input";

import type { Color } from "@/app/_components/Core/Input";
import type { UseControllerProps } from "react-hook-form";

type Props = {
  label: string;
  type?: string;
  placeholder?: string;
  showLabel?: boolean;
  className?: string;
} & UseControllerProps;

const FormInput = (props: Props) => {
  const { showLabel, className, ...inputProps } = props;
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
    <div className={cn("mb-3", className)}>
      {props.showLabel && (
        <div className="mb-2 block">
          <label htmlFor={field.name}>{props.label}</label>
        </div>
      )}

      <Input
        placeholder={props.placeholder}
        type={props.type || "text"}
        color={color}
        helperText={fieldState?.error?.message}
        {...restField}
        {...inputProps}
      />
    </div>
  );
};

FormInput.displayName = "FormInput";
export default FormInput;
