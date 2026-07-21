namespace PruebaTecnica.Web.Models;

/// <summary>
/// Representa un tipo de movimiento retornado por la API.
/// Estructura según el contrato definido en la prueba técnica.
/// </summary>
public sealed record TipoMovimiento(int Codigo, string Descripcion, bool VActiva);
