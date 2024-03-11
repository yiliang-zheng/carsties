import { cn } from "@/utils/cn";
import type { IconProps } from "@/app/_components/Icons/types/props";
const DownArrowIcon = ({ width = 3, height = 3, className }: IconProps) => {
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
        d="m1 1 5.326 5.7a.909.909 0 0 0 1.348 0L13 1"
      />
    </svg>
  );
};

DownArrowIcon.displayName = "DownArrowIcon";
export default DownArrowIcon;
