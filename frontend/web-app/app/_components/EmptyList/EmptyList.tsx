"use client";

import { useParamsStore } from "@/app/_hooks/useParamsStore";
import Button from "@/app/_components/Core/Button";
import { signIn } from "next-auth/react";
import { useSearchParams } from "next/navigation";

type Props = {
  title?: string;
  subtitle?: string;
  showReset: boolean;
  showLogin?: boolean;
  callbackUrl?: string;
};

const EmptyList = ({
  title = "No matches for this filter.",
  subtitle = "Try changing or resetting the filter",
  showReset,
  showLogin = false,
}: Props) => {
  const reset = useParamsStore((state) => state.resetParams);
  const searchParams = useSearchParams();
  const callbackUrl = searchParams.get("callbackUrl");

  return (
    <div className="w-full h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg bg-white border rounded-md">
      <h3 className="text-3xl font-bold dark:text-white text-center">
        {title}
      </h3>
      <p className="my-6 text-lg text-center font-normal text-gray-500 lg:text-xl sm:px-16 xl:px-48 dark:text-gray-400">
        {subtitle}
      </p>
      {showReset && (
        <button
          type="button"
          className="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 "
          onClick={reset}
        >
          Reset Filter
        </button>
      )}
      {showLogin && !!callbackUrl && (
        <Button
          color="dark"
          outline
          onClick={() => signIn("id-server", { callbackUrl })}
        >
          Login
        </Button>
      )}
    </div>
  );
};

EmptyList.displayName = "EmptyList";
export default EmptyList;
