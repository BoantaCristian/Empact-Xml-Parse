using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlParseAPI.Models.DataTransferObjects
{
    public class item
    {
        [XmlElement("title")]
        public string title { get; set; }

        [XmlElement("link")]
        public string link { get; set; }

        [XmlElement("description")]
        public string description { get; set; }

        [XmlElement("pubDate")]
        public string pubDate { get; set; }
        
        [XmlElement(Namespace = "http://purl.org/dc/elements/1.1/", ElementName ="creator")]
        public string publisher { get; set; }

        [XmlElement("category")]
        public List<string> groups { get; set; }
    }
}
