import { getSession } from "@/server/lib/auth/getSession";
import { getToken } from "next-auth/jwt";
import { cookies, headers } from "next/headers";

import AuthTest from "@/app/_components/AuthTest/AuthTest";

import type { NextApiRequest } from "next";

export default async function Session() {
  const session = await getSession();
  const mockRequest = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
      cookies()
        .getAll()
        .map((p) => [p.name, p.value])
    ),
  } as NextApiRequest;
  const token = await getToken({ req: mockRequest });

  return (
    <div>
      <h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
        Session Dashboard
      </h1>
      <div className="bg-blue-200 border-2 border-blue-500">
        <h3 className="text-lg">Session Data</h3>
        <pre>{JSON.stringify(session, null, 2)}</pre>
      </div>
      <div className="mt-4">
        <AuthTest />
      </div>
      <div className="bg-green-200 border-2 border-blue-500 mt-4">
        <h3 className="text-lg">Token data</h3>
        <pre className="overflow-auto">{JSON.stringify(token, null, 2)}</pre>
      </div>
    </div>
  );
}
