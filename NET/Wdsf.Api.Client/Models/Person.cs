namespace Wdsf.Api.Client.Models
{
	using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("person", Namespace = "http://services.worlddancesport.org/api")]
    public class Person
    {
        [XmlElement("link")]
        public Link[] Link  { get; set; }

        [XmlElement("id")]
        public string Min { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

		[XmlElement("sex")]
		public string SexForSerialization;

		public bool IsMale
		{
			get
			{
				if ("male".Equals(SexForSerialization, System.StringComparison.OrdinalIgnoreCase))
					return true;
				else if ("female".Equals(SexForSerialization, System.StringComparison.OrdinalIgnoreCase))
					return false;
				else
					throw new ApplicationException("Unexpected sex.");
			}
			set
			{
				SexForSerialization = (value ? "Male" : "Female");
			}
		}

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("activePartner")]
        public string ActivePartner { get; set; }

        [XmlElement("activeCoupleId")]
        public string ActiveCoupleId { get; set; }

        [XmlElement("activeCoupleAgeGroup")]
        public string AgeGroup { get; set; }
    }
}
