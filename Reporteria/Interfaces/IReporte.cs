using Reporteria.Services;

namespace Reporteria.Interfaces
{
    public interface IReporte<T>
    {
        Task<byte[]> Ejecutar(ReportRequest<T> request);
    }
}
