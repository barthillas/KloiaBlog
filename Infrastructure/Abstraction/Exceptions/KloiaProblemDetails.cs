using System.Text.Json.Serialization;

namespace Abstraction.Exceptions
{
    public class KloiaProblemDetails
    {
        [JsonConstructor]
        public KloiaProblemDetails()
        {
            Type = GetType().Name;
        }

        public KloiaProblemDetails(string errorCode, string message)
        {
            Type = GetType().Name;
        }

        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string StackTrace { get; set; }
    }
}