namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType("round", Namespace = "http://services.worlddancesport.org/api")]
    public class Round
    {
        private List<Dance> dances = new List<Dance>();

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
                return dances.Count == 0 ? null : dances.ToArray();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                dances = new List<Dance>(value);
            }
        }

        [XmlIgnore]
        public IList<Dance> Dances
        {
            get
            {
                return dances;
            }
        }
    }
}
