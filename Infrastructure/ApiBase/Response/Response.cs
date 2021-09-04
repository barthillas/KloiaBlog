namespace ApiBase.Response
{
    public class Response<T>
    {
        public Response()
        {
            this.Header = new ResponseHeader();
            this.Body = default(T);
        }

        public Response(T body, bool isSuccess, int responseCode = 0)
        {
            this.Body = body;
            this.Header = new ResponseHeader { ResponseCode = responseCode, IsSuccess = isSuccess };
        }

        public ResponseHeader Header { get; set; }

        public T Body { get; set; }

        public static Response<TBody> Success<TBody>(TBody body) => new Response<TBody>(body, true);


        public static Response<TBody> Fail<TBody>(TBody body, int responseCode) => new Response<TBody>(body, false, responseCode);

    }
}