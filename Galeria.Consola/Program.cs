using Galeria.Application.Importacion;
using Galeria.Application.Pintores;
using Galeria.Domain.Common;
using Galeria.Domain.Entities;
using Galeria.Infrastructure.Repositorios;
using Microsoft.Extensions.Logging;

// Adaptación para Visual Studio / Windows: sin esto la consola no muestra bien ✅ ❌ ni las tildes.
Console.OutputEncoding = System.Text.Encoding.UTF8;

using var loggerFactory = LoggerFactory.Create(b => b.AddSimpleConsole(o => o.SingleLine = true));
var logger = loggerFactory.CreateLogger("Galeria");

var repositorio = new PintorRepositorioEnMemoria();
var servicio = new RegistrarPintorServicio(repositorio);

try
{
    // 1) Demo: registrar pintores con Result
    RegistrarPintorDto[] pedidos =
    [
        new("Benito Quinquela Martín", "Argentina", 1890),
        new("",                        "Argentina", 1900),
        new("Xul Solar",               "Argentina", 2090),
        new("benito quinquela martín", "Argentina", 1890),
    ];

    foreach (var pedido in pedidos)
    {
        var resultado = servicio.Ejecutar(pedido);
        Console.WriteLine(resultado.Match(
            siExito: id => $"✅ Registrado: {pedido.Nombre} ({id})",
            siFallo: e => $"❌ {e.Mensaje} [{e.Codigo}]"));
    }

    // 2) Guiado: ClienteGaleria con Bind/Map
    var clientes = new[]
    {
        ClienteGaleria.Crear("Ana Gómez", "ana@galeria.com"),
        ClienteGaleria.Crear("",          "ana@galeria.com"),
        ClienteGaleria.Crear("Ana Gómez", "esto-no-es-un-mail"),
    };
    foreach (var c in clientes)
        Console.WriteLine(c.Match(
            siExito: cli => $"✅ Cliente {cli.Nombre} <{cli.Email}>",
            siFallo: e => $"❌ {e.Mensaje}"));

    // 3) Autónomo: importación CSV. Archivo inexistente → excepción, la atrapa el catch del borde.
    // Repositorio propio: si se reutiliza el de la demo, "Benito Quinquela Martín" (línea 1)
    // sale como duplicado porque ya se registró en el paso 1.
    // Adaptación para Visual Studio: pintores.csv se copia junto al .exe (ver Galeria.Consola.csproj).
    // VS ejecuta desde bin\..., no desde la raíz de la solución; por eso lo buscamos junto al ejecutable
    // y no dependemos del directorio de trabajo.
    var rutaCsv = Path.Combine(AppContext.BaseDirectory, "pintores.csv");
    var importador = new ImportadorPintores(new PintorRepositorioEnMemoria());
    var resultadoImport = importador.Importar(File.ReadLines(rutaCsv));

    Console.WriteLine($"\nImportados: {resultadoImport.Importados.Count}");
    foreach (var (linea, error) in resultadoImport.Errores)
        Console.WriteLine($"  Línea {linea}: {error.Mensaje}");

    // 4) Benchmark (opcional): dotnet run --project Galeria.Consola -c Release -- --benchmark
    if (args.Contains("--benchmark"))
        Benchmark.Ejecutar(cantidad: 100_000);
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Error inesperado en la aplicación");
    Console.WriteLine("Ocurrió un error inesperado. Contacte al administrador.");
    return 1;
}

return 0;
