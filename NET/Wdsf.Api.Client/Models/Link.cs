namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;

    [XmlType("link")]
    public class Link
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        public string HRef { get; set; }

        [XmlAttribute("rel")]
        public string Rel { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
