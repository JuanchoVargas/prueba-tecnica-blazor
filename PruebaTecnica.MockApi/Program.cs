// Mock temporal de la API REST de la prueba técnica.
// Reproduce exactamente la estructura JSON del correo del reclutador.
// Cuando llegue la URL real, basta con actualizar Api:BaseUrl y
// Api:MovimientosEndpoint en PruebaTecnica.Web/appsettings.json.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// El dataset extiende el ejemplo del correo (códigos 29, 51 y 17, intactos) con
// la MISMA estructura, para demostrar filtros, orden y paginación.
app.MapGet("/api/tiposmovimiento", async () =>
{
    // Latencia simulada para que el estado de carga sea visible en demos.
    await Task.Delay(Random.Shared.Next(300, 700));

    return new[]
    {
        new TipoMovimientoDto(29, "Ajuste al Inventario", false),
        new TipoMovimientoDto(51, "Avance Produccion", false),
        new TipoMovimientoDto(17, "Balance Inicial", false),
        new TipoMovimientoDto(11, "Entrada por Compra", true),
        new TipoMovimientoDto(12, "Salida por Venta", true),
        new TipoMovimientoDto(21, "Devolución de Cliente", true),
        new TipoMovimientoDto(22, "Devolución a Proveedor", false),
        new TipoMovimientoDto(31, "Traslado entre Bodegas", true),
        new TipoMovimientoDto(41, "Consumo Interno", true),
        new TipoMovimientoDto(45, "Salida por Donación", false),
        new TipoMovimientoDto(61, "Nota Crédito Inventario", true),
        new TipoMovimientoDto(72, "Reintegro de Producción", true)
    };
});

app.Run();

internal sealed record TipoMovimientoDto(int Codigo, string Descripcion, bool VActiva);
