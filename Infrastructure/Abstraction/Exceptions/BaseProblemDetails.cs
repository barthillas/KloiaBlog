using System;

namespace Abstraction.Exceptions
{
    public class BaseProblemDetails
    {
        public Type Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string StackTrace { get; set; }
    }
}