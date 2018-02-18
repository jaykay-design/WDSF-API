namespace Wdsf.Api.Client.Exceptions
{
    using System;
    using Wdsf.Api.Client.Models;

    public class RestException : Exception
    {

        public RestException(StatusMessage message)
            : base(message.Message)
        {
            this.Code = message.Code;
            this.SubCode = message.SubCode;
        }

        public RestException(string message, int code, int subcode)
            : base(message)
        {
            this.Code = code;
            this.SubCode = subcode;
        }

        public int Code { get; set; }
        public int SubCode { get; set; }
    }
}
