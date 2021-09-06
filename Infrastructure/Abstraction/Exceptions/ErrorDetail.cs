namespace Abstraction.Exceptions
{
    public sealed class ErrorDetail
    {
        public ErrorDetail(string code, string message, string fieldName)
        {
            this.Code = code;
            this.Message = message;
            this.FieldName = fieldName;
        }

        public string Code { get; }

        public string Message { get; }

        public string FieldName { get; }
    }
}