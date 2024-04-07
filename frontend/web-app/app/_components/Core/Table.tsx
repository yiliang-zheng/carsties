import { cn } from "@/utils/cn";
import type { ReactElement, ReactNode } from "react";

type TableHeadCellProps = {
  scope: "col" | "row";
} & TableCellProps;

type TableCellProps = {
  children: ReactNode;
  className?: string;
};

type TableRowProps = {
  children: ReactElement<TableHeadCellProps>[] | ReactElement<TableCellProps>[];
  className?: string;
};

type TableBodyProps = {
  children: ReactElement<TableRowProps>[];
  className?: string;
};

type TableHeadProps = {
  children: ReactElement<TableRowProps>;
  className?: string;
};

type TableProps = {
  children: ReactElement<TableHeadProps> | ReactElement<TableRowProps>;
  className?: string;
};

const TableHead = ({ children, className }: TableHeadProps) => (
  <thead
    className={cn(
      "text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400",
      className
    )}
  >
    {children}
  </thead>
);

const TableBody = ({ children, className }: TableBodyProps) => (
  <tbody className={className}>{children}</tbody>
);

const TableRow = ({ children, className }: TableRowProps) => (
  <tr
    className={cn(
      "odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700",
      className
    )}
  >
    {children}
  </tr>
);

const TableHeadCell = ({ scope, children, className }: TableHeadCellProps) => (
  <th scope={scope} className={cn("px-6 py-4", className)}>
    {children}
  </th>
);

const TableCell = ({ children, className }: TableCellProps) => (
  <td className={cn("px-6 py-4", className)}>{children}</td>
);

const Table = ({ children, className }: TableProps) => {
  return (
    <table
      className={cn(
        "w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400",
        className
      )}
    >
      {children}
    </table>
  );
};

Table.Head = TableHead;
Table.Body = TableBody;
Table.Row = TableRow;
Table.HeadCell = TableHeadCell;
Table.Cell = TableCell;

Table.displayName = "Table";
export default Table;
