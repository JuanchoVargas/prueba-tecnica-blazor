using Microsoft.Extensions.Options;
using PruebaTecnica.Web.Components;
using PruebaTecnica.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuración de la API: la URL vive SOLO en appsettings.json.
builder.Services.Configure<ApiOptions>(
    builder.Configuration.GetSection(ApiOptions.Seccion));

// En el hosting del demo el servidor no puede consumirse a sí mismo por su
// dominio público (restricción de loopback), así que con el flag activo la
// resolución de datos ocurre in-process a través de la MISMA interfaz; con la
// API real (flag off) se usa el cliente HTTP tipado, que es la implementación
// principal y la cubierta por tests.
var mockInterno = builder.Configuration.GetValue<bool>("Api:HabilitarMockInterno");

if (mockInterno)
{
    builder.Services.AddScoped<ITipoMovimientoService, TipoMovimientoMockService>();
}
else
{
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
}

var app = builder.Build();

// Endpoint mock embebido para el demo desplegado. Se activa solo por
// configuración; en local se usa PruebaTecnica.MockApi y con la API real
// este flag queda en false. La página NO consume este endpoint por HTTP
// (ver registro del servicio arriba): queda mapeado como evidencia pública
// del contrato REST.
if (mockInterno)
{
    app.MapGet("/api/tiposmovimiento", async () =>
    {
        await Task.Delay(Random.Shared.Next(300, 700));
        return DatosMock.TiposMovimiento;
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
