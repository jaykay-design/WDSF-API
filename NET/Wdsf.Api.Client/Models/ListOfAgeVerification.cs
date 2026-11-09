namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("age_verifications", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("age_verifications", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.age.verifications", IsCollection = true)]
    [JsonArray("age_verifications")]
    public class ListOfAgeVerification : List<AgeVerification>
    {
    }
}
