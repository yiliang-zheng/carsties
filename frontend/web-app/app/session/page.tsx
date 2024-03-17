import { getSession } from "@/server/lib/auth/getSession";
export default async function Session() {
  const session = await getSession();
  return (
    <div>
      <h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
        Session Dashboard
      </h1>
      <div className="bg-blue-200 border-2 border-blue-500">
        <h3 className="text-lg">Session Data</h3>
        <pre>{JSON.stringify(session, null, 2)}</pre>
      </div>
    </div>
  );
}
