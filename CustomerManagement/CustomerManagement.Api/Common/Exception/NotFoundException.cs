namespace CustomerManagement.Api.Common.Exception
{
    public class NotFoundException : CustomException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}