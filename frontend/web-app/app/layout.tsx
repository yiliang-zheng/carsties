import type { Metadata } from "next";
import "./globals.css";
import TRPCReactProvider from "@/app/_trpc/Provider";
import SessionProvider from "@/app/_providers/SessionProvider";
import ToasterProvider from "@/app/_providers/ToasterProvider";
import SignalRProvider from "@/app/_providers/SignalRProvider";

import Navbar from "@/app/_components/Navbar/Navbar";

export const metadata: Metadata = {
  title: "Carsties Auction",
  description: "Carsties auction platform. Get your car sold faster.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <TRPCReactProvider>
          <SessionProvider>
            <ToasterProvider />
            <Navbar />
            <SignalRProvider>
              <main className="container mx-auto px-5 pt-10">{children}</main>
            </SignalRProvider>
          </SessionProvider>
        </TRPCReactProvider>
      </body>
    </html>
  );
}
