import type { NextConfig } from "next";

const isGitHubPages = process.env.GITHUB_PAGES === "true";
const repositoryName = process.env.GITHUB_REPOSITORY?.split("/")[1] ?? "";
const isUserSite = repositoryName.endsWith(".github.io");
const pagesBasePath = isGitHubPages && !isUserSite ? `/${repositoryName}` : "";

const nextConfig: NextConfig = {
  ...(isGitHubPages
    ? {
        output: "export",
        basePath: pagesBasePath,
        assetPrefix: pagesBasePath,
        trailingSlash: true,
        images: { unoptimized: true },
        experimental: { useTypeScriptCli: true },
      }
    : {}),
};
export default nextConfig;
