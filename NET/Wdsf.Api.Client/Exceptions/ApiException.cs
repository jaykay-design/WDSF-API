namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class ApiException : Exception
    {
        public ApiException(Exception innerException)
            : base("The API call failed. See inner exception for further details.", innerException)
        {

        }
    }
}
