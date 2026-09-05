namespace Shared.Exceptions;

public sealed class BadRequestException : AppException
{
    public override int StatusCode => 400;

    public BadRequestException(string message, IReadOnlyList<string>? errors = null)
        : base(message, errors) { }
}

public sealed class NotFoundException : AppException
{
    public override int StatusCode => 404;

    public NotFoundException(string message)
        : base(message) { }
}

public sealed class UnauthorizedException : AppException
{
    public override int StatusCode => 401;

    public UnauthorizedException(string message)
        : base(message) { }
}

public sealed class ConflictException : AppException
{
    public override int StatusCode => 409;

    public ConflictException(string message, IReadOnlyList<string>? errors = null)
        : base(message, errors) { }
}
