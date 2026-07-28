import type { Metadata } from "next";
import "./globals.css";

const title = "EDF — Understand the system before changing it";
const description = "The Echelon Diagnostic Framework is a structured grammar for understanding how systems produce outcomes and where intervention has leverage.";
const siteUrl = process.env.NEXT_PUBLIC_SITE_URL ?? "https://echelon-diagnostic-framework.kemiller2002.chatgpt.site";
const metadataBase = new URL(siteUrl.endsWith("/") ? siteUrl : `${siteUrl}/`);

export const metadata: Metadata = {
  metadataBase,
  title,
  description,
  openGraph: { title, description, type: "website", images: [{ url: "og.png", width: 1200, height: 630, alt: "Understand the system. Then change it." }] },
  twitter: { card: "summary_large_image", title, description, images: ["og.png"] },
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
