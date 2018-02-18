namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string method, Uri uri)
            : base(string.Format("Access to {0} {1} was denied.", method, uri))
        {
        }
    }
}