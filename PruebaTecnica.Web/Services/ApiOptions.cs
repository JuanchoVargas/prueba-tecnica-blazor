namespace PruebaTecnica.Web.Services;

/// <summary>
/// Configuración de la API externa. Se enlaza desde appsettings.json (sección "Api").
/// </summary>
public sealed class ApiOptions
{
    public const string Seccion = "Api";

    /// <summary>URL base de la API (mock local mientras se confirma la URL real).</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Ruta relativa del endpoint de tipos de movimiento.</summary>
    public string MovimientosEndpoint { get; set; } = string.Empty;
}
