import React, { useMemo } from "react";
import { cn } from "@/utils/cn";

const alertTypes = [
  {
    type: "success",
    header: "Success!",
    className: "text-green-800 bg-green-50 border-green-800",
  },
  {
    type: "warning",
    header: "Warning!",
    className: "text-yellow-800 bg-yellow-50 border-yellow-800",
  },
  {
    type: "error",
    header: "Error!",
    className: "text-red-800 bg-red-50 border-red-800",
  },
  {
    type: "info",
    header: "Info!",
    className: "text-blue-800 bg-blue-50 border-blue-800",
  },
  {
    type: "default",
    header: "Please note!",
    className: "text-gray-800 bg-gray-50 border-gray-800",
  },
] as const;

type AlertType = (typeof alertTypes)[number]["type"];

type Props = {
  type: AlertType;
  title: string;
  className?: string;
};
const Alert = ({ title, className, type = "default" }: Props) => {
  const header = useMemo(() => {
    const result = alertTypes.find((p) => p.type === type);
    if (!result) return "";
    return result.header;
  }, [type]);

  const textColor = useMemo(() => {
    const result = alertTypes.find((p) => p.type === type);
    if (!result) return "";
    return result.className;
  }, [type]);
  return (
    <div
      className={cn("p-4 text-sm rounded-lg border-2", textColor, className)}
      role="alert"
    >
      <span className="font-medium">{header}</span> {title}
    </div>
  );
};

Alert.displayName = "Alert";
export default Alert;
