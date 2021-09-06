using System;

namespace Abstraction.Exceptions
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase(string message)
            : base(message)
        {
        }

        public ExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}