namespace PruebaTecnica.Web.Models;

/// <summary>
/// Envoltorio de resultado para operaciones que pueden fallar.
/// Evita propagar excepciones crudas hacia los componentes de UI.
/// </summary>
public sealed record Resultado<T>(bool Exito, T? Datos, string? Error)
{
    public static Resultado<T> Ok(T datos) => new(true, datos, null);
    public static Resultado<T> Fallo(string error) => new(false, default, error);
}
