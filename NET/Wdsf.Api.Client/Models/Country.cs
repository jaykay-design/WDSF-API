namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;

    [XmlType("country", Namespace = "http://services.worlddancesport.org/api")]
    public class Country
    {
        [XmlText]
        public string Name { get; set; }
    }
}
