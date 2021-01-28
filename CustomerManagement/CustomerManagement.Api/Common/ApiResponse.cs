using System.Collections.Generic;
using System.Linq;

namespace CustomerManagement.Api.Common
{
    public class ApiResponse
    {
        public static ApiResponse<T> Create<T>(T body)
        {
            return Create(body, false);
        }

        public static ApiResponse<T> Create<T>(T body, bool hasErrors)
        {
            return new ApiResponse<T>(body, hasErrors);
        }

        public static ApiResponse<T> Create<T>(T body, string errorMessage)
        {
            return Create(body, new List<string> {errorMessage});
        }

        public static ApiResponse<T> Create<T>(T body, List<string> errors)
        {
            return new ApiResponse<T>(body, errors);
        }

        public static ApiResponse<object> Error(string errorMessage)
        {
            return Error(new List<string> {errorMessage});
        }

        public static ApiResponse<object> Error(List<string> errors)
        {
            return new ApiResponse<object>(null, errors);
        }
    }

    public class ApiResponse<T>
    {
        public ApiResponse(T body, bool hasErrors)
        {
            Body = body;
            Errors = new List<string>();
            HasErrors = hasErrors;
        }

        public ApiResponse(T body, List<string> errors)
        {
            Body = body;
            Errors = errors;
            HasErrors = errors?.Any() ?? false;
        }

        public T Body { get; }
        public IEnumerable<string> Errors { get; }
        public bool HasErrors { get; }
    }
}