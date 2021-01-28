using System.Text.Json;

namespace CustomerManagement.Api.Models
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}