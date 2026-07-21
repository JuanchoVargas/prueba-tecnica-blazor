# Prueba Técnica — Blazor Web App

Aplicación en **Blazor Web App (.NET 8)** que consume una API REST y despliega los
tipos de movimiento en una grilla interactiva con ordenamiento y filtro de búsqueda.

## Cómo ejecutar

Requisitos: .NET SDK 8.0+

```bash
dotnet restore

# Terminal 1 — API mock (mientras se confirma la URL de la API real)
dotnet run --project PruebaTecnica.MockApi     # http://localhost:5200

# Terminal 2 — Aplicación web
dotnet run --project PruebaTecnica.Web         # http://localhost:5100

# Tests
dotnet test
```

## Nota sobre la API

El correo de la prueba menciona "la siguiente API REST", pero la URL del endpoint
no venía incluida en el mensaje. Se solicitó al equipo reclutador el mismo día
(domingo 20 de julio) y, para no detener el desarrollo, se construyó
`PruebaTecnica.MockApi`: un Minimal API que replica exactamente la estructura JSON
del ejemplo del correo.

**Conectar la API real requiere únicamente editar `PruebaTecnica.Web/appsettings.json`:**

```json
"Api": {
  "BaseUrl": "https://url-real-de-la-api.com",
  "MovimientosEndpoint": "ruta/del/endpoint"
}
```

Ninguna línea de código cambia: la URL nunca está hardcodeada.

## Decisiones técnicas

| Decisión | Razón |
|---|---|
| Blazor Web App (.NET 8), Interactive Server | Template actual del framework; render server evita complejidad de CORS/payload innecesaria para este alcance |
| QuickGrid | Componente de grilla oficial de Microsoft: sin dependencias de terceros ni licencias |
| `HttpClient` tipado + `IHttpClientFactory` | Patrón correcto de inyección; evita agotamiento de sockets del `new HttpClient()` manual |
| `Resultado<T>` en el servicio | La UI nunca recibe excepciones crudas; errores de red, timeout y formato se traducen a mensajes claros |
| Estados de UI: cargando / error+reintento / vacío | Comportamiento robusto ante fallas de la API, no solo el camino feliz |
| `record` para el modelo | Inmutabilidad idiomática de C# moderno |
| Tests con `HttpMessageHandler` falso | El servicio se prueba sin red: éxito, lista vacía, error HTTP y JSON inválido |
| Sin capas extra (MediatR, CQRS, etc.) | El alcance es una pantalla: la arquitectura mínima correcta es criterio, no falta de conocimiento |

## Uso de herramientas de IA

*(Completar antes de la entrega: descripción del workflow con Claude / Claude Code —
CLAUDE.md como contrato de convenciones del proyecto, generación asistida con revisión
humana de cada cambio, tests como red de seguridad.)*

## Estructura

```
PruebaTecnica.Web/       Blazor Web App (UI + servicio + modelo)
PruebaTecnica.MockApi/   Minimal API mock (temporal)
PruebaTecnica.Tests/     xUnit
```
