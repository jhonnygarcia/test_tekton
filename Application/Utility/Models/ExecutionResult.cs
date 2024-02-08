namespace Application.Utility.Models
{
    public enum ExceptionType
    {
        None,
        NotFoundException,
        BadRequestException,
        BusinessException,
        PrivilegesException,
        ValidationErrosException
    }
    public class ExecutionResult<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }
        public ExceptionType ErrorType { get; }
        public Exception Exception { get; }

        public ExecutionResult(bool success, string message, T data, ExceptionType errorType, Exception exception)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorType = errorType;
            Exception = exception;
        }
    }
    public class ExecutionResult
    {
        public bool Success { get; }
        public string Message { get; }
        public ExceptionType ErrorType { get; }
        public Exception Exception { get; }

        public ExecutionResult(bool success, string message, ExceptionType errorType, Exception exception)
        {
            Success = success;
            Message = message;
            ErrorType = errorType;
            Exception = exception;
        }
    }
}
