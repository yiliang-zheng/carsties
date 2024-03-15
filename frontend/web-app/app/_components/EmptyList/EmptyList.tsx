import { useParamsStore } from "@/app/_hooks/useParamsStore";

type Props = {
  title?: string;
  subtitle?: string;
  showReset: boolean;
};

const EmptyList = ({
  title = "No matches for this filter.",
  subtitle = "Try changing or resetting the filter",
  showReset,
}: Props) => {
  const reset = useParamsStore((state) => state.resetParams);

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
    </div>
  );
};

EmptyList.displayName = "EmptyList";
export default EmptyList;
