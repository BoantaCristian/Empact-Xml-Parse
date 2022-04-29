using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XmlParseAPI.Models.DbModels
{
    public class VersionNumber
    {
        [Key]
        public string version_number { get; set; }
    }
}
