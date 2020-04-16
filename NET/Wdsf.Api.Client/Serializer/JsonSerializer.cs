
namespace Wdsf.Api.Client.Serializer
{
    using Interfaces;
    using System;
    using System.IO;
    using n = Newtonsoft.Json;

    internal class JsonSerializer : ISerializer
    {
        public object Deserialize(Type type, Stream data)
        {
            n.JsonSerializer serializer = new n.JsonSerializer();
            using (TextReader reader = new StreamReader(data))
            {
                return serializer.Deserialize(reader, type);
            }
        }

        public void Serialize(Type type, object data, Stream outStream)
        {
            n.JsonSerializer serializer = new n.JsonSerializer();
            using (TextWriter writer = new StreamWriter(outStream))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
