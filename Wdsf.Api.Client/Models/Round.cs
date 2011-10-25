namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType("round", Namespace = "http://services.worlddancesport.org/api")]
    public class Round
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Do not use this array to process dances.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("dances")]
        public Dance[] DancesForSerialization
        {
            get
            {
                return Dances == null ? null : Dances.ToArray();
            }
            set
            {
                Dances = new List<Dance>(value);
            }
        }

        [XmlIgnore]
        public List<Dance> Dances { get; set; }

        public Round()
        {
            this.Dances = new List<Dance>();
        }
    }
}
