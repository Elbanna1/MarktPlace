namespace Shared.Exceptions;

public sealed class ForbiddenException : AppException
{
    public override int StatusCode => 403;

    public ForbiddenException(string message)
        : base(message) { }
}
