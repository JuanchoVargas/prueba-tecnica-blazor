using Microsoft.Extensions.Options;
using PruebaTecnica.Web.Components;
using PruebaTecnica.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuración de la API: la URL vive SOLO en appsettings.json.
builder.Services.Configure<ApiOptions>(
    builder.Configuration.GetSection(ApiOptions.Seccion));

// HttpClient tipado vía IHttpClientFactory (nunca new HttpClient() suelto).
builder.Services.AddHttpClient<ITipoMovimientoService, TipoMovimientoService>(
    (sp, client) =>
    {
        var opciones = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
        if (string.IsNullOrWhiteSpace(opciones.BaseUrl))
            throw new InvalidOperationException(
                "Falta configurar Api:BaseUrl en appsettings.json.");

        client.BaseAddress = new Uri(opciones.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(10);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
