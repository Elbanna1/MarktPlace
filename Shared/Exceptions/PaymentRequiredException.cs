namespace Shared.Exceptions;

public sealed class PaymentRequiredException : AppException
{
    public override int StatusCode => 402;

    public PaymentRequiredException(string message)
        : base(message) { }
}
