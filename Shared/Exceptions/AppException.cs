namespace Shared.Exceptions;

public abstract class AppException : Exception
{
    public abstract int StatusCode { get; }

    public IReadOnlyList<string>? Errors { get; }

    protected AppException(string message, IReadOnlyList<string>? errors = null)
        : base(message)
    {
        Errors = errors;
    }
}
