namespace Wdsf.Api.Client.Exceptions
{
    using System;

    public class BusyException:Exception
    {
        public BusyException() :
            base("The REST client ist busy communicating with the API")
        {
        }
    }
}
