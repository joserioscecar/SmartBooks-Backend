namespace SmartBooks.Application.Interfaces;

public interface IClientContext
{
    string ClientType { get; }
    bool IsMobile { get; }
    bool IsWeb { get; }
}
