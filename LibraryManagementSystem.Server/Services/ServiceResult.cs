namespace LibraryManagementSystem.Server.Services;

public class ServiceResult
{
    public bool Success { get; private init; }
    public string? ErrorMessage { get; private init; }
    public ServiceErrorType ErrorType { get; private init; }

    public static ServiceResult Ok() => new() { Success = true };
    public static ServiceResult NotFound(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.NotFound };
    public static ServiceResult Conflict(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.Conflict };
    public static ServiceResult BadRequest(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.BadRequest };
}

public class ServiceResult<T>
{
    public bool Success { get; private init; }
    public T? Data { get; private init; }
    public string? ErrorMessage { get; private init; }
    public ServiceErrorType ErrorType { get; private init; }

    public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
    public static ServiceResult<T> NotFound(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.NotFound };
    public static ServiceResult<T> Conflict(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.Conflict };
    public static ServiceResult<T> BadRequest(string message) => new() { ErrorMessage = message, ErrorType = ServiceErrorType.BadRequest };
}

public enum ServiceErrorType
{
    None,
    NotFound,
    Conflict,
    BadRequest
}
