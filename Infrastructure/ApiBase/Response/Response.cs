namespace ApiBase.Response
{
    public class Response<T>
    {
        public Response()
        {
            Header = new ResponseHeader();
            Body = default(T);
        }

        public Response(T body, bool isSuccess, int responseCode = 200)
        {
            Body = body;
            Header = new ResponseHeader { ResponseCode = responseCode, IsSuccess = isSuccess };
        }

        public ResponseHeader Header { get; set; }

        public T Body { get; set; }

        public static Response<TBody> Success<TBody>(TBody body) => new (body, true);


        public static Response<TBody> Fail<TBody>(TBody body, int responseCode) => new (body, false, responseCode);

    }
}