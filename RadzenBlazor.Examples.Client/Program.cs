using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Radzen services (dialog, notification, tooltip, context menu).
builder.Services.AddRadzenComponents();

#if STANDALONE
// Standalone WebAssembly publish (GitHub Pages): register our own root components.
// In the hosted InteractiveAuto app these are supplied by the server host page instead.
builder.RootComponents.Add<RadzenBlazor.Examples.Client.StandaloneRoot>("#app");
builder.RootComponents.Add<Microsoft.AspNetCore.Components.Web.HeadOutlet>("head::after");
#endif

await builder.Build().RunAsync();
