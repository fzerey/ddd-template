namespace Fzerey.DDDStarter.Domain.Exceptions
{
    public class DomainException(string message, string code) : Exception(message)
    {
        public string Code { get; } = code;
    }

    public static class DomainErrorCodes
    {
        public const string INVALID_CUSTOMER_NAME = "301";
        public const string INVALID_ITEM_NAME = "302";
        public const string INVALID_PRICE = "303";
        public const string INVALID_QUANTITY = "304";
    }
}
