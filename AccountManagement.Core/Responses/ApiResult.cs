namespace AccountManagement.Domain.Responses;

public class ApiResult<T>
{
    private T? _result;
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public T? Result {
        get
        {
            if (Errors.Count > 0)
            {
                return default;
            }

            return _result;
        }
        private set
        {
            _result = value!;
        }
    }

    public ApiResult()
    {

    }

    public static ApiResult<T> Success(T result, int statusCode = 200)
    {
        return new ApiResult<T>
        {
            IsSuccess = true,
            StatusCode = statusCode,
            _result = result
        };
    }


    public static ApiResult<T> Failure(List<string> errors, int statusCode = 400)
    {
        return new ApiResult<T>
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Errors = errors
        };
    }
}