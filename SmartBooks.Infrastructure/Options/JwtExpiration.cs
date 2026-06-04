namespace SmartBooks.Infrastructure.Options;

public class JwtExpiration
{

    public const string SectionName  = "JwtExpiration";

    public int Web { get; init; }
    public int Mobile { get; init; }
}
