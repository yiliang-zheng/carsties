import { cn } from "@/utils/cn";
import type { IconProps } from "@/app/_components/Icons/types/props";

const CloseIcon = ({ width = 3, height = 3, className }: IconProps) => {
  return (
    <svg
      className={cn(
        "text-gray-800 dark:text-white",
        "w-[1rem]",
        "h-[1rem]",
        className
      )}
      aria-hidden="true"
      xmlns="http://www.w3.org/2000/svg"
      fill="currentColor"
      viewBox="0 0 24 24"
    >
      <path
        fillRule="evenodd"
        d="M2 12a10 10 0 1 1 20 0 10 10 0 0 1-20 0Zm7.7-3.7a1 1 0 0 0-1.4 1.4l2.3 2.3-2.3 2.3a1 1 0 1 0 1.4 1.4l2.3-2.3 2.3 2.3a1 1 0 0 0 1.4-1.4L13.4 12l2.3-2.3a1 1 0 0 0-1.4-1.4L12 10.6 9.7 8.3Z"
        clipRule="evenodd"
      />
    </svg>
  );
};

CloseIcon.displayName = "CloseIcon";
export default CloseIcon;
