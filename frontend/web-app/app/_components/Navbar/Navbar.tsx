import Image from "next/image";
import logo from "@/app/_assets/Carsties.svg";
import SearchInput from "@/app/_components/SearchInput/SearchInput";
import LoginButton from "@/app/_components/LoginButton/LoginButton";
import UserActions from "@/app/_components/UserActions/UserActions";

import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";
type Props = {
  title: string;
};
const Navbar = async ({ title }: Props) => {
  const user = await getCurrentUser();
  return (
    <header>
      <div className="flex justify-around items-center bg-base-300 shadow-md">
        <div>
          <a className="p-1">
            <Image src={logo} priority alt="logo" title="logo" />
          </a>
        </div>
        <div className="hidden lg:flex lg:basis-1/2">
          <SearchInput />
        </div>
        <div className="flex justify-end basis-1/2 lg:basis-1/4">
          {user ? <UserActions /> : <LoginButton />}
        </div>
      </div>
    </header>
  );
};
Navbar.displayName = "Navbar";

export default Navbar;
