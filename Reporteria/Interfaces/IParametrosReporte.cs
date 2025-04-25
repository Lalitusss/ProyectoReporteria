namespace Reporteria.Interfaces
{
    public interface IParametrosReporte<T>
    {
        Dictionary<string, object> ObtenerParametrosReporte(T entity);
    }
}