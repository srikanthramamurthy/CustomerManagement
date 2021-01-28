namespace CustomerManagement.Api.Common.Exception
{
    public class UniqueEntityRuleException : CustomException
    {
        public UniqueEntityRuleException()
        {
        }

        public UniqueEntityRuleException(string message) : base(message)
        {
        }

        public UniqueEntityRuleException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }
    }
}