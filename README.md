# GaleriaErrores

Solución de la clase "¿Excepción o Result?" (T123-22, UAI). Consola en .NET 9 con Clean Architecture liviana: `Galeria.Domain`, `Galeria.Application`, `Galeria.Infrastructure`, `Galeria.Consola` (Presentation) y `Galeria.Tests` (xUnit + Moq).

## Abrir en Visual Studio

1. Requisitos: Visual Studio 2022 17.12 o superior (o Visual Studio 2026) con la carga de trabajo "Desarrollo de escritorio con .NET" y el SDK de .NET 9.
2. Abrir `GaleriaErrores.sln`.
3. `Galeria.Consola` es el proyecto de inicio. Si no figurara en negrita, clic derecho sobre el proyecto y "Establecer como proyecto de inicio".
4. Presionar **F5** (o Ctrl+F5 para que la consola quede abierta). El archivo `pintores.csv` se copia solo junto al ejecutable.

## Benchmark

1. En la barra de herramientas, elegir el perfil de ejecución **Benchmark** (junto al botón de inicio).
2. Cambiar la configuración de **Debug** a **Release**: en Debug los números se distorsionan.
3. Ejecutar con **Ctrl+F5** (sin depurador, para no sumar costo a cada `throw`).

Por línea de comandos: `dotnet run --project Galeria.Consola -c Release -- --benchmark`

## Tests

- Visual Studio: menú Prueba > Explorador de pruebas > Ejecutar todas las pruebas.
- Consola: `dotnet test` desde la carpeta de la solución.

## Versión

Los cinco `.csproj` apuntan a `net9.0`. Si la PC también tiene el SDK de .NET 10 no hace falta cambiar nada: compila igual para .NET 9. Hace falta acceso a NuGet para restaurar los paquetes (`Microsoft.Extensions.Logging.Console` 9.x y `Moq`).
