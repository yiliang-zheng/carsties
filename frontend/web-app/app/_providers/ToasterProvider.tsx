"use client";

import { Toaster } from "react-hot-toast";

const ToasterProvider = () => {
  return <Toaster position="top-right" />;
};

ToasterProvider.displayName = "ToasterProvider";
export default ToasterProvider;
