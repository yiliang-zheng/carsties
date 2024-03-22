import SearchInput from "@/app/_components/SearchInput/SearchInput";
import LoginButton from "@/app/_components/LoginButton/LoginButton";
import UserActions from "@/app/_components/UserActions/UserActions";
import Logo from "@/app/_components/Logo/Logo";

import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";

const Navbar = async () => {
  const user = await getCurrentUser();
  return (
    <header>
      <div className="flex justify-around items-center bg-base-300 shadow-md">
        <Logo />
        <div className="hidden lg:flex lg:basis-1/2">
          <SearchInput />
        </div>
        <div className="flex justify-end basis-1/2 lg:basis-1/4">
          {!!user ? <UserActions user={user} /> : <LoginButton />}
        </div>
      </div>
    </header>
  );
};
Navbar.displayName = "Navbar";

export default Navbar;
