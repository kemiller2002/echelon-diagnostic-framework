# EDF site browser conformance (F#)

`EdfSite.BrowserTests` drives the built site in Chromium through
Microsoft.Playwright and Deque axe-core. It is the browser half of the site
checks; `tests/forma-site.test.mjs` covers markup structure.

For each page (`index`, `learn`, `examples`, `evidence`, `start`) at 320, 375,
430, 768 and 1280px it checks:

| Check | Rule |
|---|---|
| Page overflow | no horizontal document scroll |
| Containment | Forma cards, callouts, badges, buttons, nav links, facts, index rows, CTAs, steps and quotes hold their content; no text box crosses a container edge |
| Targets | 24px minimum (WCAG 2.2 2.5.8, inline text links exempt); 44px for navigation links and buttons (Forma) |
| Navigation | all five primary links visible inside the viewport at every width |
| Keyboard | the first Tab stop is the skip link; every stop paints a visible focus indicator inside the viewport |
| axe | no WCAG 2.0/2.1/2.2 A and AA violations |
| Long content | at 320, 375 and 1280px, long URLs, identifiers, labels and headings injected into real content stay contained |
| 200% text | at 320px, text enlarged to 200% (WCAG 1.4.4) stays contained |

Contexts use `prefers-reduced-motion: reduce`, so focus scrolling is
immediate rather than Forma's smooth scroll and focus position is measured
after it settles.

It writes full-page screenshots and `report.json` to `--out` and exits 1 on
any finding.

## Run locally

Requires the .NET 10 SDK, the pinned Forma assets installed
(`bash install.sh --lock forma.lock` from
`kemiller2002/forma/actions/install-presentation` at the pinned tag), and a
static server for `static-site/`:

```bash
cp public/og.png static-site/og.png
python3 -m http.server 8765 --directory static-site &
dotnet build -c Release tests/browser
pwsh tests/browser/bin/Release/net10.0/playwright.ps1 install chromium   # once
dotnet run -c Release --no-build --project tests/browser -- --base-url http://127.0.0.1:8765 --out artifacts/browser
```

`EDF_CHROMIUM_EXECUTABLE` points the run at an existing Chromium instead of a
Playwright-managed one. CI runs the same command (`.github/workflows/quality.yml`)
and uploads the screenshots and report as the `browser-evidence` artifact.
