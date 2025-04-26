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

            var sw = Stopwatch.StartNew();

            // Carga del reporte
            reporte.Load(rutaReporte);
            sw.Stop();
            Debug.WriteLine($"[FastReport] Carga del reporte: {sw.ElapsedMilliseconds} ms");

            // Obtener parámetros
            sw.Restart();
            var parametros = _parametrosMapper.ObtenerParametrosReporte(request.Entity);
            sw.Stop();
            Debug.WriteLine($"[FastReport] Obtención de parámetros: {sw.ElapsedMilliseconds} ms");

            // Asignar parámetros
            sw.Restart();
            foreach (var parametro in parametros)
            {
                reporte.SetParameterValue(parametro.Key, parametro.Value);
            }
            sw.Stop();
            Debug.WriteLine($"[FastReport] Asignación de parámetros: {sw.ElapsedMilliseconds} ms");

            // Preparar reporte
            sw.Restart();
            bool preparado = reporte.Prepare();
            sw.Stop();
            Debug.WriteLine($"[FastReport] Preparación del reporte: {sw.ElapsedMilliseconds} ms");

            if (preparado)
            {
                // Exportar a PDF
                sw.Restart();
                using var ms = new MemoryStream();
                var pdfExport = new PDFSimpleExport
                {
                    ShowProgress = false,
                    Subject = "Reporte generado",
                    Title = nombreReporte
                };

                reporte.Export(pdfExport, ms);
                ms.Position = 0;
                sw.Stop();
                Debug.WriteLine($"[FastReport] Exportación a PDF: {sw.ElapsedMilliseconds} ms");

                return Task.FromResult(ms.ToArray());
            }
            else
            {
                return Task.FromResult(Array.Empty<byte>());
            }
        }


    }
}
