using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PruebaTecnica.Web.Models;

namespace PruebaTecnica.Web.Services;

/// <summary>
/// Consume el endpoint de tipos de movimiento.
/// Todos los errores se capturan aquí y se traducen a un Resultado tipado:
/// los componentes de UI nunca reciben excepciones crudas.
/// </summary>
public sealed class TipoMovimientoService(HttpClient http, IOptions<ApiOptions> opciones)
    : ITipoMovimientoService
{
    private readonly string _endpoint = opciones.Value.MovimientosEndpoint;

    public async Task<Resultado<IReadOnlyList<TipoMovimiento>>> ObtenerAsync(
        CancellationToken ct = default)
    {
        try
        {
            var datos = await http.GetFromJsonAsync<List<TipoMovimiento>>(_endpoint, ct);
            return Resultado<IReadOnlyList<TipoMovimiento>>.Ok(datos ?? []);
        }
        catch (HttpRequestException ex)
        {
            return Resultado<IReadOnlyList<TipoMovimiento>>.Fallo(
                $"No fue posible conectar con la API ({(int?)ex.StatusCode ?? 0}). " +
                "Verifique que el servicio esté disponible.");
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            return Resultado<IReadOnlyList<TipoMovimiento>>.Fallo(
                "La API no respondió dentro del tiempo esperado (timeout).");
        }
        catch (JsonException)
        {
            return Resultado<IReadOnlyList<TipoMovimiento>>.Fallo(
                "La respuesta de la API no tiene el formato esperado.");
        }
    }
}
