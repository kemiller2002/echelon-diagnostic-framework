# Echelon Diagnostic Framework

## GitHub Pages

The workflow in `.github/workflows/pages.yml` builds and publishes a static
version of the site whenever `main` is pushed. It can also be run manually from
the repository's **Actions** tab.

In the GitHub repository, open **Settings → Pages** and set **Source** to
**GitHub Actions** once. The published project URL will be:

`https://kemiller2002.github.io/Echelon-diagnostic-framework/`

To verify the static export locally:

```sh
GITHUB_REPOSITORY=kemiller2002/Echelon-diagnostic-framework \
NEXT_PUBLIC_SITE_URL=https://kemiller2002.github.io/Echelon-diagnostic-framework/ \
npm run build:pages
```
