namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("license", Namespace = "http://services.worlddancesport.org/api")]
    public class License
    {

        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("division")]
        public string Division { get; set; }

        [XmlElement("expiresOn")]
        public string ExpiresOn { get; set; }

        [XmlElement("wrlBLockedUntil")]
        public string WrlBlockedUntil { get; set; }

        [XmlElement("cupOrChampionshipBlockedUntil")]
        public string CupOrChampionshipBlockedUntil { get; set; }
    }
}