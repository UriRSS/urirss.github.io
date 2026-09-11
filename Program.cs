using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MyPortfolio;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

var host = builder.Build();

var js = host.Services.GetRequiredService<IJSRuntime>();

string? savedCulture = null;

try
{
    savedCulture = await js.InvokeAsync<string?>(
        "blazorCulture.get");
}
catch
{
    // Если localStorage недоступен,
    // используем культуру браузера.
}

var culture = GetUserCulture(savedCulture);

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

await host.RunAsync();


static CultureInfo GetUserCulture(string? savedCulture)
{
    if (!string.IsNullOrWhiteSpace(savedCulture))
    {
        if (savedCulture.StartsWith("ru",
                StringComparison.OrdinalIgnoreCase))
        {
            return new CultureInfo("ru");
        }

        if (savedCulture.StartsWith("ky",
                StringComparison.OrdinalIgnoreCase))
        {
            return new CultureInfo("ky");
        }

        if (savedCulture.StartsWith("en",
                StringComparison.OrdinalIgnoreCase))
        {
            return new CultureInfo("en");
        }
    }

    var browserCulture = CultureInfo.CurrentCulture
        .TwoLetterISOLanguageName;

    return browserCulture switch
    {
        "ru" => new CultureInfo("ru"),
        "ky" => new CultureInfo("ky"),
        _ => new CultureInfo("en")
    };
}