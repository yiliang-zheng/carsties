import Image from "next/image";
import React, { useState, useMemo } from "react";

type Props = {
  imageUrl: string;
  title: string;
};
const AuctionCardImage = ({ imageUrl, title }: Props) => {
  const [loading, setLoading] = useState(true);
  const imageLoadingClass = useMemo(() => {
    if (loading) return "grayscale blur-2xl scale-110";
    return "grayscale-0 blur-0 scale-100";
  }, [loading]);
  return (
    <Image
      src={imageUrl}
      alt={title}
      width={0}
      height={0}
      priority
      sizes="100vw"
      className={`object-cover h-auto w-auto group-hover:opacity-75 duration-700 ease-in-out ${imageLoadingClass}`}
      onLoad={(_) => setLoading(false)}
    />
  );
};

AuctionCardImage.displayName = "AuctionCardImage";
export default AuctionCardImage;
