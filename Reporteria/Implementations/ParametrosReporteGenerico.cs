using Reporteria.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace Reporteria.Services
{
    /// <summary>
    /// Implementación genérica de IParametrosReporte que usa reflexión para mapear propiedades públicas de la entidad
    /// a un diccionario de parámetros para el reporte.
    /// </summary>
    /// <typeparam name="T">Tipo de la entidad</typeparam>
    public class ParametrosReporteGenerico<T> : IParametrosReporte<T>
    {
        public Dictionary<string, object> ObtenerParametrosReporte(T entity)
        {
            var parametros = new Dictionary<string, object>();

            if (entity == null)
                return parametros;

            // Obtener todas las propiedades públicas de la entidad
            PropertyInfo[] propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propiedad in propiedades)
            {
                // Puedes personalizar el nombre del parámetro aquí
                string nombreParametro = propiedad.Name.ToLower(); // ejemplo: nombre en minúsculas

                object valor = propiedad.GetValue(entity) ?? string.Empty;

                parametros[nombreParametro] = valor;
            }

            return parametros;
        }
    }
}
