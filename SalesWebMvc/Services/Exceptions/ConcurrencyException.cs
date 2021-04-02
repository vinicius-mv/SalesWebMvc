using System;

namespace SalesWebMvc.Services.Exceptions
{
    public class ConcurrencyException : ApplicationException
    {
        public ConcurrencyException() : base()
        {
        }

        public ConcurrencyException(string message) : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
