using Galeria.Domain.Common;
using Galeria.Domain.Entities;
using Galeria.Domain.Errors;
using Galeria.Domain.Interfaces;

namespace Galeria.Application.Importacion;

public sealed record ErrorDeLinea(int NumeroLinea, Error Error);

public sealed record ResultadoImportacion(
    IReadOnlyList<Pintor> Importados,
    IReadOnlyList<ErrorDeLinea> Errores);

public sealed class ImportadorPintores(IPintorRepositorio repositorio)
{
    /// <summary>
    /// Recibe líneas (no un path): Application no conoce el sistema de archivos.
    /// Acumula errores por línea en lugar de cortar en el primero.
    /// </summary>
    public ResultadoImportacion Importar(IEnumerable<string> lineas)
    {
        var importados = new List<Pintor>();
        var errores = new List<ErrorDeLinea>();
        var numeroLinea = 0;

        foreach (var linea in lineas)
        {
            numeroLinea++;

            var resultado = ParsearLinea(linea)
                .Bind(VerificarNoDuplicado)
                .Bind(d => Pintor.Crear(d.Nombre, d.Nacionalidad, d.Anio));

            if (resultado.EsFallo)
            {
                errores.Add(new ErrorDeLinea(numeroLinea, resultado.Error));
                continue;
            }

            repositorio.Agregar(resultado.Valor);
            importados.Add(resultado.Valor);
        }

        return new ResultadoImportacion(importados, errores);
    }

    private static Result<(string Nombre, string Nacionalidad, int Anio)> ParsearLinea(string linea)
    {
        var partes = linea.Split(';');
        if (partes.Length != 3)
            return ImportacionErrores.FormatoInvalido(partes.Length);

        if (!int.TryParse(partes[2].Trim(), out var anio))
            return ImportacionErrores.AnioNoNumerico(partes[2].Trim());

        return (partes[0].Trim(), partes[1].Trim(), anio);
    }

    private Result<(string Nombre, string Nacionalidad, int Anio)> VerificarNoDuplicado(
        (string Nombre, string Nacionalidad, int Anio) datos)
    {
        if (repositorio.ExisteConNombre(datos.Nombre))
            return PintorErrores.Duplicado(datos.Nombre);
        return datos;
    }
}
