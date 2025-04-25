using System.Reflection;

namespace Reporteria.Utils
{
    public static class Helper
    {
        private const string ExtensionReporte = ".frx";

        public static string ObtenerRutaReporte(string nombreReporte)
        {
            if (string.IsNullOrEmpty(nombreReporte))
                throw new ArgumentException("El nombre del reporte no puede estar vacío.", nameof(nombreReporte));

            string nombreConExtension = Path.HasExtension(nombreReporte)
                ? nombreReporte
                : nombreReporte + ExtensionReporte;

            string rutaEnsamblado = Assembly.GetExecutingAssembly().Location;
            string directorioEnsamblado = Path.GetDirectoryName(rutaEnsamblado)
                ?? throw new DirectoryNotFoundException("No se pudo obtener el directorio del ensamblado.");

            return Path.Combine(directorioEnsamblado, "Reportes", nombreConExtension);
        }
    }
}
