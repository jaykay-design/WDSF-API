namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("age", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.age")]
    public class AgeClass
    {
        [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("fromAge")]
        public int FromAge { get; set; }
        
        [XmlElement("toAge")]
        public int ToAge { get; set; }

        [XmlElement("minBirthdate", DataType = "date" )]
        public DateTime MinBirthDate { get; set; }

        [XmlElement("maxBirthdate", DataType = "date" )]
        public DateTime MaxBirthDate { get; set; }

        [XmlArray("allowedToDanceIn")]
        [XmlArrayItem("age")]
        public string[] AllowedToDanceIn { get; set; }
    }
}
