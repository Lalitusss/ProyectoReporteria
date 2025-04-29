using System.ComponentModel.DataAnnotations;

public class ReciboDto
{
    [Required]
    public string? Numero { get; set; }
    public string? Fecha { get; set; }
    public string? Recibido { get; set; }
    public string? Domicilio { get; set; }
    public string? CP { get; set; }
    public string? Localidad { get; set; }
    public string? Cantidad { get; set; }
    public string? CUIT { get; set; }
    public string? Telefono { get; set; }
    public string? EMP { get; set; }
    public string? TipoDePago { get; set; }
    public string? Periodo { get; set; }
    public string? Ubicacion { get; set; }
}

