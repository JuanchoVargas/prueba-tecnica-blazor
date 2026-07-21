using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services;

/// <summary>
/// Implementación in-process de ITipoMovimientoService para el demo desplegado
/// (flag Api:HabilitarMockInterno activo). Misma latencia simulada del mock
/// para que el estado de carga sea visible.
/// </summary>
public sealed class TipoMovimientoMockService : ITipoMovimientoService
{
    public async Task<Resultado<IReadOnlyList<TipoMovimiento>>> ObtenerAsync(
        CancellationToken ct = default)
    {
        await Task.Delay(Random.Shared.Next(300, 700), ct);
        return Resultado<IReadOnlyList<TipoMovimiento>>.Ok(DatosMock.TiposMovimiento);
    }
}
