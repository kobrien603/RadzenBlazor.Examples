# Radzen Blazor Examples

A small **.NET 10 Blazor Web App** built to try out [Radzen Blazor](https://blazor.radzen.com)
components — focused on the **Charts** and the newly released **`RadzenSpreadsheet`**.

🔗 **Live demo:** https://kobrien603.github.io/RadzenBlazor.Examples/

## What's in here

| Page | Component | Highlights |
|------|-----------|------------|
| **Home** (`/`) | — | Live badge showing the current render mode (Static → Server → WebAssembly), plus cards linking to every demo |
| **Charts** (`/charts`) | `RadzenChart` | Switch between line / area / column / bar / pie / donut, toggle smooth & markers, randomize data |
| **Spreadsheet** (`/spreadsheet`) | `RadzenSpreadsheet` | Excel-compatible grid with a live formula engine (`=SUM`, `=B5*C1`) and `.xlsx` / `.csv` import & export |
| **Spreadsheet chart** (`/spreadsheet-chart`) | `RadzenSpreadsheet` + `SheetChart` | A native **in-sheet** chart plotting two trace columns; updates as you edit. Switch chart type / point count. Single shared value axis (model limitation) |
| **Spreadsheet-driven chart** (`/spreadsheet-live-chart`) | `RadzenSpreadsheet` → `RadzenChart` | The sheet holds the data; an **external** dual-axis chart reads the cells and redraws live — so the right trace gets its own axis and scale |
| **Dual-axis tester** (`/dual-axis-tester`) | `RadzenChart` (v11 multi-axis) | Two value axes (left + right); a green/red badge tells you whether their ticks line up, with auto-set steps or manual + "what it should be" hints |

A header **light/dark toggle** (`RadzenAppearanceToggle`) switches between Material and Material
Dark and is remembered across reloads. Radzen.Blazor **11.0.4**, .NET 10.

## Render modes

The app uses **global `InteractiveAuto`**, so every page is exercised in all three render
modes during a single load:

1. **Static SSR** — initial server-rendered HTML
2. **InteractiveServer** — interactive over SignalR while the WebAssembly runtime downloads
3. **InteractiveWebAssembly** — fully client-side once WASM is ready

The badge on the home page reflects the active mode in real time.

## Theming

The light/dark choice is managed by Radzen's `ThemeService` and persisted with
`AddRadzenCookieThemeService()` (registered in both projects). Instead of a hardcoded
stylesheet, an interactive `<RadzenTheme>` renders the theme so the toggle can swap it at
runtime — in `App.razor` for the hosted app and in `StandaloneRoot.razor` for the standalone
WASM build. On the hosted app the cookie is read during prerender, so there is no theme flash
on first paint.

## Project layout

```
RadzenBlazor.Examples/          # Server host project (InteractiveAuto)
RadzenBlazor.Examples.Client/   # WebAssembly project — all demo pages, layout and nav live here
  Pages/                        # Home, Charts, Spreadsheet, SpreadsheetChart,
                                #   SpreadsheetLiveChart, DualAxisTesterPage
  Components/                   # DualAxisChart (reusable two value-axis chart),
                                #   DualAxisTester (alignment controls, uses DualAxisChart)
  Charts/AxisAlignment.cs       # Pure, testable tick-alignment math
  Layout/                       # MainLayout (header theme toggle) + NavMenu
  StandaloneRoot.razor          # Root used only for the standalone WASM (GitHub Pages) build
.github/workflows/deploy.yml    # Publishes the WASM build to GitHub Pages
```

All interactive pages are authored in the **`.Client`** project. This lets the exact same
components run under the hosted app (Server + Auto + WASM) **and** as a standalone WebAssembly
site on GitHub Pages — which is static and can only serve the WASM output.

## Run locally

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project RadzenBlazor.Examples
```

Then open the printed `https://localhost:...` URL. First paint is server-rendered; the render
badge flips to **WebAssembly** once the client runtime loads.

## Build the static site (what GitHub Pages serves)

```bash
dotnet publish RadzenBlazor.Examples.Client -c Release -p:Standalone=true -o release
# serve release/wwwroot with any static file server
```

The `-p:Standalone=true` flag compiles the `STANDALONE` define, which registers the Client's
own root components and uses `wwwroot/index.html` as the host page.

## Deployment

Pushes to `main` trigger `.github/workflows/deploy.yml`, which publishes the standalone
WebAssembly build, rewrites the `<base href>` to the repository sub-path, adds a `404.html`
SPA fallback and a `.nojekyll` marker, then deploys to GitHub Pages.

## License

This sample is provided as-is for demonstration. Radzen.Blazor is MIT licensed.
