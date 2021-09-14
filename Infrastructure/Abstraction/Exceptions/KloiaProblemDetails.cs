using System;

namespace Abstraction.Exceptions
{
    public class KloiaProblemDetails : BaseProblemDetails
    {
        public KloiaProblemDetails(Exception ex)
        {
            Type = this.GetType();
            StackTrace = ex.StackTrace;
            Message = ex.Message;
        }
    }
}