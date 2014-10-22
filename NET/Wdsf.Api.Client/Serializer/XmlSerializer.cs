namespace Wdsf.Api.Client.Serializer
{
    using System;
    using System.IO;
    using Interfaces;
    using xml = System.Xml.Serialization;

    internal class XmlSerializer : ISerializer
    {
        public object Deserialize(Type type, Stream data)
        {
            xml.XmlSerializer serializer = new xml.XmlSerializer(type);
            return serializer.Deserialize(data);
        }

        public void Serialize(Type type, object data, Stream outStream)
        {
            xml.XmlSerializer serializer = new xml.XmlSerializer(type);
            xml.XmlSerializerNamespaces ns = new xml.XmlSerializerNamespaces();
            ns.Add("xlink", "http://www.w3.org/1999/xlink");

            serializer.Serialize(outStream, data, ns);
        }
    }
}
