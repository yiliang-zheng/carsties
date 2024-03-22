import type { ReactNode, PropsWithChildren, ReactElement } from "react";

import { cn } from "@/utils/cn";

const positions = ["top", "bottom", "left", "right"] as const;
type Position = (typeof positions)[number];

type DropdownItemProps = {};

type DropdownProps = {
  position?: Position;
  className?: string;
  button: ReactNode;
  children: ReactElement<DropdownItemProps>[] | ReactElement<DropdownItemProps>;
};

const DropdownItem = ({ children }: PropsWithChildren<DropdownItemProps>) => (
  <li>{children}</li>
);

const Dropdown = ({
  position = "bottom",
  className,
  button,
  children,
}: DropdownProps) => {
  return (
    <div
      className={cn(
        "dropdown dropdown-end",
        { "dropdown-top": position === "top" },
        { "dropdown-bottom": position === "bottom" },
        { "dropdown-left": position === "left" },
        { "dropdown-right": position === "right" }
      )}
    >
      {button}
      <ul
        tabIndex={0}
        className={cn(
          "dropdown-content z-[1] menu p-2 shadow bg-base-100 rounded-box w-52",
          className
        )}
      >
        {children}
      </ul>
    </div>
  );
};

Dropdown.displayName = "Dropdown";
Dropdown.Item = DropdownItem;
export default Dropdown;
