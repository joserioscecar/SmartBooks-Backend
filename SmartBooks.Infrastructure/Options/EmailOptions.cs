namespace SmartBooks.Infrastructure.Options;

public class EmailOptions
{
    public const string SectionName = "Email";

    public string SmtpHost { get; init; } = string.Empty;
    public int Port { get; init; }
    public string User { get; init; } = string.Empty;
    public string Pass { get; init; } = string.Empty;
    public string FromName { get; init; } = string.Empty;
    public string From { get; init; } = string.Empty;
}
