using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XmlParseAPI.Models.DbModels
{
    public class NewsGroup
    {
        [Key]
        public int Id { get; set; }
        public virtual News News { get; set; }
        public virtual Group Groups { get; set; }
    }
}
