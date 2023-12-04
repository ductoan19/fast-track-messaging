namespace my_app_backend.Domain.SeedWork.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>()
            {
                IsSuccessful = true,
                Data = data
            };
        }

        public static ApiResponse<T> Error(string errorCode, string message)
        {
            return new ApiResponse<T>()
            {
                IsSuccessful = false,
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static ApiResponse<T> Error(string message)
        {
            return new ApiResponse<T>()
            {
                IsSuccessful = false,
                ErrorCode = string.Empty,
                Message = message
            };
        }
    }

    public class ApiResponse : ApiResponse<string>
    {
        public static ApiResponse<string> Ok()
        {
            return Ok(string.Empty);
        }
    }
}
