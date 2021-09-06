using System;
using System.Collections.Generic;

namespace Abstraction.Exceptions
{
    public class ValidationException : ExceptionBase
    {
        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidationException(string message, IEnumerable<ErrorDetail> errors)
            : base(message)
        {
            if (errors != null) ErrorDetails.AddRange(errors);
        }

        public List<ErrorDetail> ErrorDetails { get; } = new();
    }
}