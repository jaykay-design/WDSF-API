namespace Wdsf.Api.Client.Serializer
{
    using Interfaces;
    using System;

    internal static class SerializerFactory
    {
        internal static ISerializer GetSerializer(ContentTypes format)
        {
            switch (format)
            {
                case ContentTypes.Json: { return new JsonSerializer(); }
                case ContentTypes.Xml: { return new XmlSerializer(); }
                default: { throw new Exception("ApiDataFormat invalid."); }
            }

        }
    }
}
