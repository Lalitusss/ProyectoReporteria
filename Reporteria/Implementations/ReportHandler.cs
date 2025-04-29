using Reporteria.Interfaces;

public static class ReportHandler
{
    public static async Task<byte[]?> GenerateReport<TDto>(
        IDictionary<string, string> parameters,
        string reportName,
        IReporte<TDto> reporteService) where TDto : new()
    {
        var dto = new TDto();
        var dtoType = typeof(TDto);

        foreach (var param in parameters)
        {
            var prop = dtoType.GetProperty(param.Key,
                System.Reflection.BindingFlags.IgnoreCase |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                try
                {
                    var convertedValue = Convert.ChangeType(param.Value, prop.PropertyType);
                    prop.SetValue(dto, convertedValue);
                }
                catch
                {
                    // Maneja errores de conversión
                }
            }
        }

        var request = new ReportRequest<TDto> { Entity = dto, NombreReporte = reportName };
        return await reporteService.Ejecutar(request);
    }
}
