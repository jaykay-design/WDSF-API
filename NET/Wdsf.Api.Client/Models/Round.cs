namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("round", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("round")]
    public class Round
    {
        private List<Dance> dances = new List<Dance>();

        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Maximum allowed deviation of scrores in AJS 3.0
        /// </summary>
        [XmlAttribute("maxDeviation")]
        [JsonProperty("maxDeviation")]
        public string MaxDeviation { get; set; }

        /// <summary>
        /// <para>Do not use this array to process dances.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("dances")]
        [JsonProperty("dances")]
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

        [XmlIgnore, JsonIgnore]
        public IList<Dance> Dances
        {
            get
            {
                return dances;
            }
        }
    }
}
