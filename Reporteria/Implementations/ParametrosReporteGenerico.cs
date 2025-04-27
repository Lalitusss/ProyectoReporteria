using Reporteria.Interfaces;
using System.Reflection;

namespace Reporteria.Services
{
    public class ParametrosReporteGenerico<T> : IParametrosReporte<T>
    {
        public Dictionary<string, object> ObtenerParametrosReporte(T entity)
        {
            var parametros = new Dictionary<string, object>();

            if (entity == null)
                return parametros;

            PropertyInfo[] propiedades = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propiedad in propiedades)
            {
                string nombreParametro = propiedad.Name.ToLower();
                object valor = propiedad.GetValue(entity) ?? string.Empty;
                parametros[nombreParametro] = valor;
            }
            return parametros;
        }
    }
}