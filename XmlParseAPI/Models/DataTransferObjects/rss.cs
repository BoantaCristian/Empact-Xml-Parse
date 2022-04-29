using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlParseAPI.Models.DataTransferObjects
{
    [XmlRoot("rss")]
    public class rss
    {
        [XmlElement("channel")]
        public channel channel { get; set; }

    }
}
