namespace SmartBooks.Domain.Exceptions;

public class RuleBusinessException : Exception
{
    public string Code { get; }

    public RuleBusinessException()
        : base("A business rule was violated.")
    {
        Code = "BUSINESS_RULE_VIOLATION";
    }

    public RuleBusinessException(string message)
        : base(message)
    {
        Code = "BUSINESS_RULE_VIOLATION";
    }

    public RuleBusinessException(string message, string code)
        : base(message)
    {
        Code = code;
    }

    public RuleBusinessException(string message, Exception innerException)
        : base(message, innerException)
    {
        Code = "BUSINESS_RULE_VIOLATION";
    }

    public RuleBusinessException(string message, string code, Exception innerException)
        : base(message, innerException)
    {
        Code = code;
    }
}