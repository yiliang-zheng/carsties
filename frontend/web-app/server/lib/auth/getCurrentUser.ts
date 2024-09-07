import { auth } from "@/auth";

export type User = {
  name: string;
  email: string;
  username: string;
};
export const getCurrentUser = async (): Promise<User | null> => {
  try {
    const session = await auth();
    if (!session || !session.user) return null;

    const result: User = {
      name: session.user.name ?? "",
      email: session.user.email ?? "",
      username: session.user.username,
    };

    return result;
  } catch (error) {
    return null;
  }
};
