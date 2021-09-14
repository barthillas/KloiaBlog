using System.Collections.Generic;

namespace Abstraction.Exceptions
{
    public class ValidationProblemDetails : BaseProblemDetails
    {
        public ValidationProblemDetails(ValidationException exception)
        {
            Type = this.GetType();
            this.ErrorDetails = exception.ErrorDetails;
            this.Message = exception.Message;
        }

        public List<ErrorDetail> ErrorDetails { get; set; }
    }
}