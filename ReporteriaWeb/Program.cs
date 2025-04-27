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
    string afiliado, string cuil,
    string du, string titular,
    IReporte<CredencialOS> reporteService) =>
{
    var request = new ReportRequest<CredencialOS>
    {
        Entity = new CredencialOS
        {
            Afiliado = afiliado,
            Cuil = cuil,
            DU = du,
            Titular = titular
        },
        NombreReporte = "CredencialOS"
    };

    byte[] pdfBytes = await reporteService.Ejecutar(request);

    if (pdfBytes == null || pdfBytes.Length == 0)
        return Results.NotFound();

    return Results.File(pdfBytes, "application/pdf", "CredencialOS.pdf");
});
app.MapGet("/download-recibo", async (
      string numero,
      string fecha,
      string recibido,
      string domicilio,
      string cp,
      string localidad,
      string cantidad,
      string cuit,
      string telefono,
      string emp,
      string tipodepago,
      string periodo,
      string ubicacion,
    IReporte<ReciboDto> reporteService) =>
{
    var request = new ReportRequest<ReciboDto>
    {
        Entity = new ReciboDto
        {
            Numero = numero,
            Fecha = fecha,
            Recibido = recibido,
            Domicilio = domicilio,
            CP = cp,
            Localidad = localidad,
            Cantidad = cantidad,
            CUIT = cuit,
            Telefono = telefono,
            EMP = emp,
            TipoDePago = tipodepago,
            Periodo = periodo,
            Ubicacion = ubicacion
        },
        NombreReporte = "Recibo"
    };

    byte[] pdfBytes = await reporteService.Ejecutar(request);

    if (pdfBytes == null || pdfBytes.Length == 0)
        return Results.NotFound();

    return Results.File(pdfBytes, "application/pdf", "Recibo.pdf");
});


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
