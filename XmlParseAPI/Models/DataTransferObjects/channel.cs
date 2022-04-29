using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlParseAPI.Models.DataTransferObjects
{
    public class channel
    {
        [XmlElement("item")]
        public List<item> item { get; set; }
    }
}
