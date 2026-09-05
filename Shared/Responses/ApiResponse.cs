namespace Shared.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public IReadOnlyList<string>? Errors { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, string message, T? data, IReadOnlyList<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }

    public static ApiResponse<T> Ok(T? data, string message = Constants.UserMessages.Generic.Loaded) =>
        new(true, message, data);

    public static ApiResponse<T> Fail(string message, IReadOnlyList<string>? errors = null) =>
        new(false, message, default, errors);
}

public class ApiResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public IReadOnlyList<string>? Errors { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, string message, IReadOnlyList<string>? errors = null)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }

    public static ApiResponse Ok(string message = Constants.UserMessages.Generic.Loaded) =>
        new(true, message);

    public static ApiResponse Fail(string message, IReadOnlyList<string>? errors = null) =>
        new(false, message, errors);
}
