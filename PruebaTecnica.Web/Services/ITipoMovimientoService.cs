using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services;

public interface ITipoMovimientoService
{
    Task<Resultado<IReadOnlyList<TipoMovimiento>>> ObtenerAsync(CancellationToken ct = default);
}
