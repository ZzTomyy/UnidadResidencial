namespace UnidadResidencial.Web.Core
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Result { get; set; }

        public static Response<T> Failure(Exception ex, string message = "An error occurred while processing the request")
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string>
                {
                    ex.Message,
                }
            };
        }

        public static Response<T> Failure(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static Response<T> Success(T result, string message = "Operation completed successfully")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result,
            };
        }

        public static Response<T> Success(string message = "Operation completed successfully")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
            };
        }
    }
}