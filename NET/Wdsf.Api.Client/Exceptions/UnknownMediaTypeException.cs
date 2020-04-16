namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class UnknownMediaTypeException : Exception
    {
        public UnknownMediaTypeException(string receivedType) :
            base()
        {
            this.ReceivedType = receivedType;
        }

        public override string Message
        {
            get
            {
                return "An unknown media type was received: " + this.ReceivedType;
            }
        }

        public string ReceivedType { get; set; }
    }
}
