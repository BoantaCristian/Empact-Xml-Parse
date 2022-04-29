using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XmlParseAPI.Models.DbModels;

namespace XmlParseAPI.Models.DbContext
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<News> News { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<NewsGroup> NewsGroups { get; set; }
        public DbSet<VersionNumber> VersionNumbers { get; set; }
    }
}
