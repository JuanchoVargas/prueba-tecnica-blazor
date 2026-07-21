using Microsoft.Extensions.Options;
using PruebaTecnica.Web.Components;
using PruebaTecnica.Web.Models;
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

// Endpoint mock embebido para el demo desplegado. Se activa solo por
// configuración; en local se usa PruebaTecnica.MockApi y con la API real
// este flag queda en false.
if (app.Configuration.GetValue<bool>("Api:HabilitarMockInterno"))
{
    app.MapGet("/api/tiposmovimiento", async () =>
    {
        await Task.Delay(Random.Shared.Next(300, 700));

        return new[]
        {
            new TipoMovimiento(29, "Ajuste al Inventario", false),
            new TipoMovimiento(51, "Avance Produccion", false),
            new TipoMovimiento(17, "Balance Inicial", false),
            new TipoMovimiento(11, "Entrada por Compra", true),
            new TipoMovimiento(12, "Salida por Venta", true),
            new TipoMovimiento(21, "Devolución de Cliente", true),
            new TipoMovimiento(22, "Devolución a Proveedor", false),
            new TipoMovimiento(31, "Traslado entre Bodegas", true),
            new TipoMovimiento(41, "Consumo Interno", true),
            new TipoMovimiento(45, "Salida por Donación", false),
            new TipoMovimiento(61, "Nota Crédito Inventario", true),
            new TipoMovimiento(72, "Reintegro de Producción", true)
        };
    });
}

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
