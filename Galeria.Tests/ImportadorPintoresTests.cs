using Galeria.Application.Importacion;
using Galeria.Infrastructure.Repositorios;

namespace Galeria.Tests;

public class ImportadorPintoresTests
{
    [Fact]
    public void Importar_ConLineasMixtas_ImportaValidasYReportaErroresPorLinea()
    {
        var importador = new ImportadorPintores(new PintorRepositorioEnMemoria());
        string[] lineas =
        [
            "Xul Solar;Argentina;1887",
            "Antonio Berni;Argentina;dos mil",
            "Xul Solar;Argentina;1887",
        ];

        var resultado = importador.Importar(lineas);

        Assert.Single(resultado.Importados);
        Assert.Collection(resultado.Errores,
            e => { Assert.Equal(2, e.NumeroLinea); Assert.Equal("Importacion.AnioNoNumerico", e.Error.Codigo); },
            e => { Assert.Equal(3, e.NumeroLinea); Assert.Equal("Pintor.Duplicado", e.Error.Codigo); });
    }
}
