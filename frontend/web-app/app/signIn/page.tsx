import EmptyList from "@/app/_components/EmptyList/EmptyList";
export default function Page({
  searchParams,
}: {
  searchParams: { callbackUrl: string };
}) {
  return (
    <EmptyList
      title="You need to logged in to do the operation"
      subtitle="Please click below to sign in"
      showLogin
      showReset={false}
      callbackUrl={searchParams.callbackUrl}
    />
  );
}
