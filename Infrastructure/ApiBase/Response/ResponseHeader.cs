using System;

namespace ApiBase.Response
{
    public sealed class ResponseHeader
    {
        public ResponseHeader()
        {
            this.GlobalId = Guid.NewGuid();
        }

        public Guid GlobalId { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int ResponseCode { get; set; }

        public DateTime HostDateTime { get; set; }
    }
}