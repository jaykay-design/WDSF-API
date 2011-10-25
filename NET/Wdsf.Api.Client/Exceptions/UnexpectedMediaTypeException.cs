namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class UnexpectedMediaTypeException : Exception
    {
        public UnexpectedMediaTypeException(Type expectedType, Type receivedType) :
            base()
        {
            this.ReceivedType = receivedType;
            this.ExpectedType = expectedType;
        }

        public override string Message
        {
            get
            {
                return string.Format("Type \"{0}\" was expected but \"{1}\" was received", this.ExpectedType.FullName, this.ReceivedType.FullName);
            }
        }

        public Type ExpectedType { get; set; }
        public Type ReceivedType { get; set; }
    }
}
