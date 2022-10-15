namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string method, Uri uri)
            : base($"Access to {method} {uri} was denied.")
        {
        }
    }
}