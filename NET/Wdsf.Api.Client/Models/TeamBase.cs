namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;

    public class TeamBase
    {
        [XmlElement("link")]
        public virtual Link[] Links { get; set; }

        [XmlElement("id")]
        public virtual int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }
    }
}
