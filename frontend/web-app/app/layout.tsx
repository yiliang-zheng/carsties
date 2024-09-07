import type { Metadata } from "next";
import "./globals.css";
import TRPCReactProvider from "@/app/_trpc/Provider";
import SessionProvider from "@/app/_providers/SessionProvider";
import ToasterProvider from "@/app/_providers/ToasterProvider";
import SignalRProvider from "@/app/_providers/SignalRProvider";

import Navbar from "@/app/_components/Navbar/Navbar";

import { getCurrentUser } from "@/server/lib/auth/getCurrentUser";

export const metadata: Metadata = {
  title: "Carsties Auction",
  description: "Carsties auction platform. Get your car sold faster.",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const user = await getCurrentUser();
  const notificationUrl = process.env.SIGNALR_URL!;
  return (
    <html lang="en">
      <body>
        <TRPCReactProvider>
          <SessionProvider>
            <ToasterProvider />
            <Navbar />
            <SignalRProvider user={user} notificationUrl={notificationUrl}>
              <main className="container mx-auto px-5 pt-10">{children}</main>
            </SignalRProvider>
          </SessionProvider>
        </TRPCReactProvider>
      </body>
    </html>
  );
}
