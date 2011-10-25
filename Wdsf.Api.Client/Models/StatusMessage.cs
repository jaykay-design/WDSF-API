namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("status", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.status")]
    public class StatusMessage
    {
        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("subcode")]
        public int SubCode { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("link")]
        public Link Link { get; set; }
    }
}
