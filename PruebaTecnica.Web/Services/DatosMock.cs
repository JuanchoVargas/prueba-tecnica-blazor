using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services;

/// <summary>
/// Dataset del mock embebido: los mismos 12 registros de PruebaTecnica.MockApi.
/// Compartido entre el endpoint REST embebido y el servicio in-process
/// para no triplicar los datos.
/// </summary>
public static class DatosMock
{
    public static IReadOnlyList<TipoMovimiento> TiposMovimiento { get; } =
    [
        new(29, "Ajuste al Inventario", false),
        new(51, "Avance Produccion", false),
        new(17, "Balance Inicial", false),
        new(11, "Entrada por Compra", true),
        new(12, "Salida por Venta", true),
        new(21, "Devolución de Cliente", true),
        new(22, "Devolución a Proveedor", false),
        new(31, "Traslado entre Bodegas", true),
        new(41, "Consumo Interno", true),
        new(45, "Salida por Donación", false),
        new(61, "Nota Crédito Inventario", true),
        new(72, "Reintegro de Producción", true)
    ];
}
