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
        hostname: "**.windows.net",
        port: "",
      },
      {
        protocol: "https",
        hostname: "**.drive.com.au",
        port: "",
      },
    ],
  },
};
export default nextConfig;
