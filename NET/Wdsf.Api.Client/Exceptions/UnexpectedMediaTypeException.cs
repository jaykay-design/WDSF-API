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
                return $"Type \"{this.ExpectedType.FullName}\" was expected but \"{this.ReceivedType.FullName}\" was received";
            }
        }

        public Type ExpectedType { get; set; }
        public Type ReceivedType { get; set; }
    }
}
