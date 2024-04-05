import { withAuth } from "next-auth/middleware";

export default withAuth(
  function middleware(req) {
    console.log(req.nextauth.token);
  },
  {
    callbacks: {
      authorized: ({ token }) => {
        return !!token && !token.error;
      },
    },
    pages: {
      signIn: "/signIn",
    },
  }
);
export const config = { matcher: ["/session"] };
