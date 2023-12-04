using System.Runtime.CompilerServices;

namespace my_app_backend.Domain.SeedWork.Models
{
    public class Result<T>
    {
        public bool IsSuccessful { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Result<T> Ok(T data)
        {
            return new Result<T>()
            {
                IsSuccessful = true,
                Data = data
            };
        }

        public static Result<T> Error(string errorCode, string message)
        {
            return new Result<T>()
            {
                IsSuccessful = false,
                ErrorCode = errorCode,
                Message = message
            };
        }

        public static Result<T> Error(string message)
        {
            return new Result<T>()
            {
                IsSuccessful = false,
                ErrorCode = string.Empty,
                Message = message
            };
        }

        public ApiResponse<T> ToApiResponse()
        {
            return new ApiResponse<T>
            {
                IsSuccessful = this.IsSuccessful,
                Data = this.Data,
                ErrorCode = this.ErrorCode,
                Message = this.Message
            };
        }
    }

    public class Result : Result<string>
    {
        public static Result Ok()
        {
            return new Result
            {
                IsSuccessful = true
            };
        }

        public static Result Error(string message)
        {
            return new Result
            {
                IsSuccessful = false,
                Message = message
            };
        }
    }
}
