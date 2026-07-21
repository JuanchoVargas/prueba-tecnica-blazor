using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using PruebaTecnica.Web.Services;
using Xunit;

namespace PruebaTecnica.Tests;

public sealed class TipoMovimientoServiceTests
{
    private const string JsonValido = """
        [
          { "Codigo": 29, "Descripcion": "Ajuste al Inventario", "VActiva": false },
          { "Codigo": 51, "Descripcion": "Avance Produccion", "VActiva": false },
          { "Codigo": 17, "Descripcion": "Balance Inicial", "VActiva": true }
        ]
        """;

    [Fact]
    public async Task ObtenerAsync_ConRespuestaValida_RetornaExitoConDatos()
    {
        var servicio = CrearServicio(HttpStatusCode.OK, JsonValido);

        var resultado = await servicio.ObtenerAsync();

        Assert.True(resultado.Exito);
        Assert.NotNull(resultado.Datos);
        Assert.Equal(3, resultado.Datos!.Count);
        Assert.Equal(29, resultado.Datos[0].Codigo);
        Assert.Equal("Ajuste al Inventario", resultado.Datos[0].Descripcion);
        Assert.True(resultado.Datos[2].VActiva);
    }

    [Fact]
    public async Task ObtenerAsync_ConListaVacia_RetornaExitoSinRegistros()
    {
        var servicio = CrearServicio(HttpStatusCode.OK, "[]");

        var resultado = await servicio.ObtenerAsync();

        Assert.True(resultado.Exito);
        Assert.Empty(resultado.Datos!);
    }

    [Fact]
    public async Task ObtenerAsync_ConErrorHttp_RetornaFalloConMensaje()
    {
        var servicio = CrearServicio(HttpStatusCode.InternalServerError, "");

        var resultado = await servicio.ObtenerAsync();

        Assert.False(resultado.Exito);
        Assert.Null(resultado.Datos);
        Assert.Contains("500", resultado.Error);
    }

    [Fact]
    public async Task ObtenerAsync_ConJsonInvalido_RetornaFalloDeFormato()
    {
        var servicio = CrearServicio(HttpStatusCode.OK, "{ esto no es un arreglo válido");

        var resultado = await servicio.ObtenerAsync();

        Assert.False(resultado.Exito);
        Assert.Contains("formato", resultado.Error, StringComparison.OrdinalIgnoreCase);
    }

    // ---- Infraestructura de prueba ----

    private static TipoMovimientoService CrearServicio(HttpStatusCode codigo, string cuerpo)
    {
        var handler = new HandlerFalso(codigo, cuerpo);
        var http = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost:5200")
        };
        var opciones = Options.Create(new ApiOptions
        {
            BaseUrl = "http://localhost:5200",
            MovimientosEndpoint = "api/tiposmovimiento"
        });

        return new TipoMovimientoService(http, opciones);
    }

    /// <summary>
    /// HttpMessageHandler falso: simula respuestas de la API sin red.
    /// </summary>
    private sealed class HandlerFalso(HttpStatusCode codigo, string cuerpo) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var respuesta = new HttpResponseMessage(codigo)
            {
                Content = new StringContent(cuerpo, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(respuesta);
        }
    }
}
