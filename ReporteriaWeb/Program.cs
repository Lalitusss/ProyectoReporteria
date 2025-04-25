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

app.MapGet("/download-credencial", async (
    string nya, string dni, string numcredencial,
    IReporte<CredencialDto> reporteService) =>
{
    var request = new ReportRequest<CredencialDto>
    {
        Entity = new CredencialDto
        {
            NyA = nya,
            DNI = dni,
            NumCredencial = numcredencial
        },
        NombreReporte = "Credencial.frx"
    };

    byte[] pdfBytes = await reporteService.Ejecutar(request);

    if (pdfBytes == null || pdfBytes.Length == 0)
        return Results.NotFound();

    return Results.File(pdfBytes, "application/pdf", "Credencial.pdf");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
