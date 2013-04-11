namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("rank", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.rank")]
    public class Ranking
    {
        [XmlElement("link")]
        public Link[] Links { get; set; }

        [XmlElement("name")]
        public string Couple { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("rank")]
        public int Rank { get; set; }
        
        [XmlElement("points")]
        public int Points { get; set; }

        [XmlElement("manMin")]
        public string ManMin { get; set; }

        [XmlElement("womanMin")]
        public string WomanMin { get; set; }

        [XmlElement("coupleId")]
        public string Id { get; set; }
    }
}
