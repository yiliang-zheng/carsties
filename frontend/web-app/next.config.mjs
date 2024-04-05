/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: "https",
        hostname: "**.pixabay.com",
        port: "",
      },
      {
        protocol: "https",
        hostname: "pixabay.com",
        port: "",
      },
      {
        protocol: "https",
        hostname: "**.windows.net",
        port: "",
      },
      {
        protocol: "https",
        hostname: "**.drive.com.au",
        port: "",
      },
      {
        protocol: "https",
        hostname: "**.norev.com",
        port: "",
      },
    ],
  },
};
export default nextConfig;
