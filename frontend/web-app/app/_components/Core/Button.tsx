import { cn } from "@/utils/cn";
import LoadingIcon from "@/app/_components/Icons/Loading";
import type { ButtonHTMLAttributes, ReactNode } from "react";

const colors = {
  dark: {
    primary: "text-white bg-gray-800 hover:bg-gray-900 focus:ring-gray-300 ",
    outline:
      "text-gray-900 hover:text-white border-2 border-gray-800 hover:bg-gray-900 focus:ring-gray-300",
  },
  light: {
    primary: "text-gray-900 bg-white hover:bg-gray-100 focus:ring-gray-100",
    outline:
      "text-blue-700 hover:text-white border-2 border-blue-700 hover:bg-blue-800 focus:ring-blue-300",
  },
  green: {
    primary: "text-white bg-green-700 hover:bg-green-800 focus:ring-green-300",
    outline:
      "text-green-700 hover:text-white border-2 border-green-700 hover:bg-green-800 focus:bg-green-300",
  },
  red: {
    primary: "text-white bg-red-700 hover:bg-red-800 focus:ring-red-300",
    outline:
      "text-red-700 hover:text-white border-2 border-red-700 hover:bg-red-800 focus:bg-red-300",
  },
  yellow: {
    primary:
      "text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-yellow-300",
    outline:
      "text-yellow-400 hover:text-white border-2 border-yellow-400 hover:bg-yellow-500 focus:bg-yellow-300",
  },
  purple: {
    primary:
      "text-white bg-purple-700 hover:bg-purple-800 focus:ring-purple-300",
    outline:
      "text-purple-700 hover:text-white border-2 border-purple-700 hover:bg-purple-800 focus:bg-purple-300",
  },
} as const;

type Props = {
  children: ReactNode;
  className?: string;
  outline?: boolean;
  color?: keyof typeof colors;
  type?: ButtonHTMLAttributes<HTMLButtonElement>["type"];
  isLoading?: boolean;
  onClick?: () => void;
};

const Button = ({
  children,
  className,
  outline = false,
  color = "dark",
  type = "button",
  isLoading = false,
  onClick,
}: Props) => {
  const commonClass =
    "focus:outline-none focus:ring-4 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2";

  const colorClass = outline ? colors[color].outline : colors[color].primary;

  return (
    <button
      type={type}
      className={cn(commonClass, colorClass, className)}
      onClick={onClick}
      disabled={isLoading}
    >
      {isLoading && (
        <span className="flex flex-row items-center">
          <LoadingIcon outline={outline} className="inline w-4 h-4 me-3" />{" "}
          Loading...
        </span>
      )}
      {!isLoading && children}
    </button>
  );
};

Button.displayName = "Button";
export default Button;
