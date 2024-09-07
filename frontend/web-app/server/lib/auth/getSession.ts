import { auth } from "@/auth";

export const getSession = async () => {
  const result = await auth();
  return result;
};
