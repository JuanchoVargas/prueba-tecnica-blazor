# Prueba Técnica — Blazor Web App (.NET Developer)

## Contexto
Prueba técnica para vacante .NET. Blazor Web App que consume una API REST y muestra
los datos en una grilla. Entrega: lunes 21 julio 2026, 5 p.m.
La URL de la API real está pendiente del reclutador (ya se le solicitó por correo);
mientras tanto se trabaja contra PruebaTecnica.MockApi, que replica el JSON del ejemplo.

## Estado actual
La solución YA está scaffoldeada y funcional. Antes de modificar algo, lee la
estructura existente. No regeneres archivos desde cero.

## Estructura
- `PruebaTecnica.Web` — Blazor Web App (.NET 8), Interactive Server
  - `Models/TipoMovimiento.cs` — record del contrato de la API
  - `Models/Resultado.cs` — envoltorio de resultado (nunca excepciones crudas a la UI)
  - `Services/` — ApiOptions + ITipoMovimientoService + implementación (HttpClient tipado)
  - `Components/Pages/Movimientos.razor` — página principal con QuickGrid
  - `Components/Shared/EstadoBadge.razor` — badge Activa/Inactiva
  - `wwwroot/app.css` — estilos propios, sin frameworks CSS
- `PruebaTecnica.MockApi` — Minimal API mock en http://localhost:5200
- `PruebaTecnica.Tests` — xUnit, servicio testeado con HttpMessageHandler falso

## Comandos
```
dotnet restore
dotnet run --project PruebaTecnica.MockApi     # terminal 1 → :5200
dotnet run --project PruebaTecnica.Web         # terminal 2 → :5100
dotnet test
```

## Reglas (NO negociables)
- La URL de la API vive SOLO en `appsettings.json` (`Api:BaseUrl` + `Api:MovimientosEndpoint`).
  NUNCA hardcodeada en código. Cuando llegue la URL real, el cambio es solo de config.
- HttpClient tipado vía IHttpClientFactory. Prohibido `new HttpClient()` en producción.
- El servicio captura HttpRequestException, TaskCanceledException (timeout) y JsonException
  y retorna `Resultado<T>`. Los componentes nunca manejan excepciones de red.
- La página mantiene los 3 estados: cargando / error con reintento / lista vacía.
- Solo QuickGrid como componente de grilla. Sin librerías de UI de terceros.
- Sin secretos ni credenciales en el repo. Sin sobre-ingeniería (nada de MediatR, CQRS,
  capas especulativas).
- Commits atómicos en español: `feat|fix|test|docs|chore: descripción corta`.
- Si algo no compila, arregla la causa mínima; no reescribas archivos completos.

## Pendientes
- [ ] Verificar compilación y correr tests (primera vez en esta máquina)
- [ ] Probar los 3 estados de UI (apagar el mock para provocar el error)
- [ ] Al llegar la URL real: actualizar appsettings.json y validar contra datos reales
- [ ] Completar README: sección "Uso de herramientas de IA" y captura de pantalla
- [ ] (Opcional) Deploy en vivo + link en el correo de entrega
