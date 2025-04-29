using System.ComponentModel.DataAnnotations;

public class CredencialOSDto
{
    [Required]
    public string? Afiliado { get; set; }
    public string? Cuil { get; set; }
    public string? DU { get; set; }
    public string? Titular { get; set; }
}

