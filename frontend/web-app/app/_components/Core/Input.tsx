import { cn } from "@/utils/cn";
import type { InputHTMLAttributes } from "react";

export const colors = ["default", "success", "failure"] as const;
export type Color = (typeof colors)[number];
type Props = {
  helperText?: string;
  color?: Color;
} & InputHTMLAttributes<HTMLInputElement>;
const Input = (props: Props) => {
  const { color, helperText, ...inputProps } = props;
  return (
    <>
      <input
        type={inputProps.type}
        className={cn(
          "border text-sm rounded-lg block w-full p-2.5",
          {
            "bg-gray-50 border-gray-300 text-gray-900 focus:ring-blue-500 focus:border-blue-500":
              color === "default",
          },
          {
            "bg-green-50 border-green-500 text-green-900 placeholder-green-700 focus:ring-green-500 focus:border-green-500":
              color === "success",
          },
          {
            "bg-red-50 border-red-500 text-red-900 placeholder-red-700 focus:ring-red-500 focus:border-red-500":
              color === "failure",
          }
        )}
        {...inputProps}
      />

      {!!helperText && (
        <p
          className={cn(
            "mt-2 text-sm",
            { "text-gray-500": color === "default" },
            { "text-green-600": color === "success" },
            { "text-red-600": color === "failure" }
          )}
        >
          {helperText}
        </p>
      )}
    </>
  );
};

Input.displayName = "Input";
export default Input;
