# Radzen Blazor Examples

A small **.NET 10 Blazor Web App** built to try out [Radzen Blazor](https://blazor.radzen.com)
components — focused on the **Charts** and the newly released **`RadzenSpreadsheet`**.

🔗 **Live demo:** https://kobrien603.github.io/RadzenBlazor.Examples/

## What's in here

| Page | Component | Highlights |
|------|-----------|------------|
| **Home** (`/`) | — | Live badge showing the current render mode (Static → Server → WebAssembly) |
| **Charts** (`/charts`) | `RadzenChart` | Switch between line / area / column / bar / pie / donut, toggle smooth & markers, randomize data |
| **Spreadsheet** (`/spreadsheet`) | `RadzenSpreadsheet` | Excel-compatible grid with a live formula engine (`=SUM`, `=B5*C1`) and `.xlsx` / `.csv` import & export |

Theme: **Material Dark**. Radzen.Blazor **11.0.4**.

## Render modes

The app uses **global `InteractiveAuto`**, so every page is exercised in all three render
modes during a single load:

1. **Static SSR** — initial server-rendered HTML
2. **InteractiveServer** — interactive over SignalR while the WebAssembly runtime downloads
3. **InteractiveWebAssembly** — fully client-side once WASM is ready

The badge on the home page reflects the active mode in real time.

## Project layout

```
RadzenBlazor.Examples/          # Server host project (InteractiveAuto)
RadzenBlazor.Examples.Client/   # WebAssembly project — all demo pages, layout and nav live here
  Pages/                        # Home, Charts, Spreadsheet
  Layout/                       # MainLayout + NavMenu (shared by host and standalone builds)
  StandaloneRoot.razor          # Router used only for the standalone WASM (GitHub Pages) build
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
