namespace CustomerManagement.Api.Common.Exception
{
    public class BadRequestException : CustomException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}