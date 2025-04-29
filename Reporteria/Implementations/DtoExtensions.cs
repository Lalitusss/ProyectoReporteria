public static class DtoExtensions
{
    public static Dictionary<string, string> ToDictionary<TDto>(this TDto dto)
        where TDto : class
    {
        return typeof(TDto)
            .GetProperties()
            .Where(p => p.CanRead)
            .ToDictionary(
                p => p.Name,
                p => p.GetValue(dto)?.ToString() ?? string.Empty
            );
    }
}



































































