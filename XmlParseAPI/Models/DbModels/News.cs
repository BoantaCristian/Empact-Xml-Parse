using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XmlParseAPI.Models.DbModels
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public virtual Publisher Publishers { get; set; }
        public virtual ICollection<NewsGroup> NewsGroups { get; set; }
    }
}
