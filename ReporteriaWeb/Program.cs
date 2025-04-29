using Microsoft.AspNetCore.Mvc;
using Reporteria.Interfaces;
using Reporteria.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddFastReport();

// Registra tus servicios ANTES de builder.Build()
builder.Services.AddScoped(typeof(IParametrosReporte<>), typeof(ParametrosReporteGenerico<>));
builder.Services.AddScoped(typeof(IReporte<>), typeof(ReporteService<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configuración de reportes
var reportesConfig = new Dictionary<string, Type>
{
    { "CredencialOS", typeof(CredencialOSDto) },
    { "Recibo", typeof(ReciboDto) },
    // Agrega nuevos reportes aquí
};

app.MapGet("/download-report/{nombreReporte}", async (
    string nombreReporte,
    HttpContext context, 
    [FromServices] IServiceProvider serviceProvider) =>
{
    if (!reportesConfig.TryGetValue(nombreReporte, out var dtoType))
        return Results.NotFound("Reporte no encontrado");

    try
    {
        var parameters = context.Request.Query
            .ToDictionary(k => k.Key, v => v.Value.ToString());

        var reporteServiceType = typeof(IReporte<>).MakeGenericType(dtoType);
        var reporteService = serviceProvider.GetRequiredService(reporteServiceType);

        var method = typeof(ReportHandler).GetMethod(nameof(ReportHandler.GenerateReport));
        var genericMethod = method.MakeGenericMethod(dtoType);

        var task = (Task<byte[]?>)genericMethod.Invoke(null,
            new object[] { parameters, nombreReporte, reporteService });

        var pdfBytes = await task;

        return pdfBytes == null || pdfBytes.Length == 0
            ? Results.NotFound("Documento no generado")
            : Results.File(pdfBytes, "application/pdf", $"{nombreReporte}.pdf");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error generando reporte: {ex.Message}");
    }
});


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
