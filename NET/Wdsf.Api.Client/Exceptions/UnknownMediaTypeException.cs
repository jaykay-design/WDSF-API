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
        public UnknownMediaTypeException(string receivedType, string receivedContent) :
            base()
        {
            this.ReceivedType = receivedType;
            ReceivedContent = receivedContent;
        }


        public override string Message
        {
            get
            {
                return "An unknown media type was received: " + this.ReceivedType + ' ' + this.ReceivedContent;
            }
        }

        public string ReceivedType { get; set; }
        public string ReceivedContent { get; }
    }
}
