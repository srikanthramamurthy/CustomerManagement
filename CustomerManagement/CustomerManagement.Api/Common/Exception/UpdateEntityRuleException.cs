namespace CustomerManagement.Api.Common.Exception
{
    public class UpdateEntityRuleException : CustomException
    {
        public UpdateEntityRuleException()
        {
        }

        public UpdateEntityRuleException(string message) : base(message)
        {
        }

        public UpdateEntityRuleException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }
    }
}