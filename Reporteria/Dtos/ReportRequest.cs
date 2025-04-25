public class ReportRequest<T>
{
    public T Entity { get; set; }
    public List<T> Values { get; set; } = new List<T>();
    public string NombreReporte { get; set; }
}
