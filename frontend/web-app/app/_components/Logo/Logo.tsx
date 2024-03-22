"use client";
import Image from "next/image";
import logo from "@/app/_assets/Carsties.svg";

import { usePathname, useRouter } from "next/navigation";
import { useParamsStore } from "@/app/_hooks/useParamsStore";

const Logo = () => {
  const router = useRouter();
  const pathname = usePathname();
  const reset = useParamsStore((state) => state.resetParams);
  const doReset = () => {
    if (pathname !== "/") router.push("/");
    reset();
  };
  return (
    <div className="p-1 cursor-pointer" onClick={doReset}>
      <Image src={logo} priority alt="logo" title="logo" />
    </div>
  );
};

Logo.displayName = "Logo";
export default Logo;
