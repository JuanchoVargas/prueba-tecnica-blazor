// Mock temporal de la API REST de la prueba técnica.
// Reproduce exactamente la estructura JSON del correo del reclutador.
// Cuando llegue la URL real, basta con actualizar Api:BaseUrl y
// Api:MovimientosEndpoint en PruebaTecnica.Web/appsettings.json.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/tiposmovimiento", () => new[]
{
    new TipoMovimientoDto(29, "Ajuste al Inventario", false),
    new TipoMovimientoDto(51, "Avance Produccion", false),
    new TipoMovimientoDto(17, "Balance Inicial", false)
});

app.Run();

internal sealed record TipoMovimientoDto(int Codigo, string Descripcion, bool VActiva);
