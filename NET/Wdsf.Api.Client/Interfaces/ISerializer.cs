namespace Wdsf.Api.Client.Interfaces
{
    using System;
    using System.IO;

    internal interface ISerializer
    {
        void Serialize(Type type, object data, Stream outStream);
        object Deserialize(Type type, Stream data);
    }
}
