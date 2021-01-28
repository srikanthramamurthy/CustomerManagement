using System;

namespace CustomerManagement.Api.Common.Exception
{
    public abstract class CustomException : SystemException
    {
        protected CustomException()
        {
        }

        protected CustomException(string message) : base(message)
        {
        }

        protected CustomException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}