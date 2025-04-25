using FastReport;
using FastReport.Export.PdfSimple;
using Reporteria.Interfaces;
using Reporteria.Utils;

namespace Reporteria.Services
{
    public class ReporteService<T> : IReporte<T>
    {
        private readonly IParametrosReporte<T> _parametrosMapper;

        public ReporteService(IParametrosReporte<T> parametrosMapper)
        {
            _parametrosMapper = parametrosMapper;
        }

        public Task<byte[]> Ejecutar(ReportRequest<T> request)
        {
            FastReport.Utils.Config.WebMode = true;

            using Report reporte = new Report();

            string nombreReporte = request.NombreReporte;

            if (string.IsNullOrWhiteSpace(nombreReporte))
                throw new ArgumentException("El nombre del reporte no puede estar vacío.", nameof(request.NombreReporte));

            string rutaReporte = Helper.ObtenerRutaReporte(nombreReporte);

            reporte.Load(rutaReporte);

            // Obtener parámetros desde el mapper
            var parametros = _parametrosMapper.ObtenerParametrosReporte(request.Entity);

            foreach (var parametro in parametros)
            {
                reporte.SetParameterValue(parametro.Key, parametro.Value);
            }

            if (reporte.Prepare())
            {
                using var ms = new MemoryStream();
                var pdfExport = new PDFSimpleExport
                {
                    ShowProgress = false,
                    Subject = "Reporte generado",
                    Title = nombreReporte
                };

                reporte.Export(pdfExport, ms);
                ms.Position = 0;

                return Task.FromResult(ms.ToArray());
            }
            else
            {
                return Task.FromResult(Array.Empty<byte>());
            }
        }
    }
}
