using System;

namespace IT_Asset_Management_System.Common.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException(string message) : base(message)
        {
        }

        public InternalServerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
