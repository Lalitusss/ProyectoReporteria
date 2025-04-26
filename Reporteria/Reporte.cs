using FastReport;
using FastReport.Export.PdfSimple;
using Reporteria.Interfaces;
using Reporteria.Utils;
using System.Diagnostics;

namespace Reporteria.Services
{
    public class ReporteService<T> : IReporte<T>
    {
        private readonly IParametrosReporte<T> _parametrosMapper;
        private readonly Dictionary<string, byte[]> _templateCache = new(); // Cache simple en memoria

        public ReporteService(IParametrosReporte<T> parametrosMapper)
        {
            _parametrosMapper = parametrosMapper;
        }

        public Task<byte[]> Ejecutar(ReportRequest<T> request)
        {
            FastReport.Utils.Config.WebMode = true;

            if (string.IsNullOrWhiteSpace(request.NombreReporte))
                throw new ArgumentException("El nombre del reporte no puede estar vacío.", nameof(request.NombreReporte));

            string rutaReporte = Helper.ObtenerRutaReporte(request.NombreReporte);

            if (!_templateCache.TryGetValue(rutaReporte, out var templateBytes))
            {
                templateBytes = File.ReadAllBytes(rutaReporte);
                _templateCache[rutaReporte] = templateBytes;
            }

            using var ms = new MemoryStream(templateBytes);
            using var reporte = new Report();
            reporte.Load(ms);

            var parametros = _parametrosMapper.ObtenerParametrosReporte(request.Entity);
            foreach (var parametro in parametros)
                reporte.SetParameterValue(parametro.Key, parametro.Value);

            if (reporte.Prepare())
            {
                using var pdfStream = new MemoryStream();
                var pdfExport = new PDFSimpleExport
                {
                    ShowProgress = false,
                    Subject = "Reporte generado",
                    Title = request.NombreReporte
                };
                reporte.Export(pdfExport, pdfStream);
                return Task.FromResult(pdfStream.ToArray());
            }

            return Task.FromResult(Array.Empty<byte>());
        }
    }
}
