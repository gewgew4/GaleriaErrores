using Galeria.Domain.Common;

namespace Galeria.Application.Importacion;

public static class ImportacionErrores
{
    public static Error FormatoInvalido(int columnas) =>
        new("Importacion.FormatoInvalido", $"Se esperaban 3 columnas y se encontraron {columnas}.");

    public static Error AnioNoNumerico(string valor) =>
        new("Importacion.AnioNoNumerico", $"El año '{valor}' no es un número.");
}
