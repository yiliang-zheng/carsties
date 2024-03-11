import { cn } from "@/utils/cn";
import type { IconProps } from "@/app/_components/Icons/types/props";

const UpArrowIcon = ({ width = 3, height = 3, className }: IconProps) => {
  return (
    <svg
      className={cn(
        "text-gray-800 dark:text-white",
        `w-${width}`,
        `h-${height}`,
        className
      )}
      aria-hidden="true"
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 14 8"
    >
      <path
        stroke="currentColor"
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeWidth="2"
        d="M13 7 7.674 1.3a.91.91 0 0 0-1.348 0L1 7"
      />
    </svg>
  );
};

UpArrowIcon.displayName = "UpArrowIcon";
export default UpArrowIcon;
