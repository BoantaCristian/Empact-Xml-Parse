using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XmlParseAPI.Models;
using XmlParseAPI.Models.DataTransferObjects;
using XmlParseAPI.Models.DbContext;
using XmlParseAPI.Models.DbModels;

namespace XmlParseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XmlParseController : ControllerBase
    {
        private ApplicationContext _context;
        private ApplicationSettings _appSettings;

        public XmlParseController(ApplicationContext context, IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("GetNewsList")]
        public async Task<IActionResult> GetNewsList()
        {
            var sourceXmlString = await DownloadXml();
            var sortedNewsList = DeserializeXml(sourceXmlString);

            await SaveNewsToDatabase(sortedNewsList);

            var result = sortedNewsList.Select(s => new { s.title, s.link, s.description, s.pubDate });

            return Ok(result);
        }

        [HttpGet]
        [Route("GetSavedNews")]
        public IActionResult GetSavedNews()
        {
            //lambda expression approach
            var savedNews = _context.NewsGroups.Include(i => i.Groups)
                                               .Include(i => i.News).ThenInclude(ti => ti.Publishers)
                                               .Select(s => new { 
                                                   s.News.Title, s.News.Link, s.News.Description, s.News.PublicationDate,
                                                   PublisherName = s.News.Publishers.Name,
                                                   Group = _context.NewsGroups.Include(i => i.Groups).Include(i => i.News).Where(w => w.News == s.News).Select(sel => sel.Groups.Name).ToList()
                                               })
                                               .GroupBy(g => g.Title)
                                               .Select(s => s.First());
                                               //.Distinct() //doesn't work



            //linq approach
            var savedNewsLinq = from newsGroups in _context.NewsGroups
                                join news in _context.News on newsGroups.News.Id equals news.Id
                                join groups in _context.Groups on newsGroups.Groups.Id equals groups.Id
                                join publisher in _context.Publishers on news.Publishers.Id equals publisher.Id
                                select new
                                {
                                    news.Title,
                                    news.Link,
                                    news.Description,
                                    news.PublicationDate,
                                    PublisherName = news.Publishers.Name,
                                    GroupName = _context.NewsGroups.Include(i => i.Groups).Include(i => i.News).Where(w => w.News == news).Select(s => s.Groups.Name).ToList()
                                };

            var disctinctSavedNewsLinq = from newsLinq in savedNewsLinq
                                         group newsLinq by newsLinq.Title into distinctNews
                                         select distinctNews.First();

            return Ok(savedNews);
        }

        public async Task<string> DownloadXml()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var sourceXmlString = await client.GetStringAsync(_appSettings.XML_Source_URL);
                    return sourceXmlString;
                }
                catch (Exception)
                {
                    return "Source not found!";
                    throw;
                }
            }
        }

        public IEnumerable<item> DeserializeXml(string sourceXmlString)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(rss));
                var stringReader = new StringReader(sourceXmlString);
                var news = (rss)xmlSerializer.Deserialize(stringReader);
                var sortedNewsList = (news.channel.item).OrderBy(ob => ob.title);

                return sortedNewsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task SaveNewsToDatabase(IEnumerable<item> newsList)
        {

            foreach (var post in newsList)
            {
                var publisher = await checkPublisher(post.publisher);
                var existentPost = new News();
                var postAlreadyExists = false;
                foreach (var currentPost in _context.News)
                {
                    if (currentPost.Title == post.title)
                    {
                        postAlreadyExists = true;
                        existentPost = currentPost;
                        break;
                    }
                }
                if (!postAlreadyExists)
                {
                    var newPost = new News
                    {
                        Title = post.title,
                        Link = post.link,
                        Description = post.description,
                        PublicationDate = DateTime.Parse(post.pubDate),
                        Publishers = publisher
                    };

                    await _context.News.AddAsync(newPost);
                    await _context.SaveChangesAsync();

                    await checkNewsGroups(post.groups, newPost.Title);
                }
                else
                {
                    existentPost.Title = post.title;
                    existentPost.Link = post.link;
                    existentPost.Description = post.description;
                    existentPost.PublicationDate = DateTime.Parse(post.pubDate);
                    existentPost.Publishers = publisher;

                    _context.Entry(existentPost).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    await checkNewsGroups(post.groups, existentPost.Title);
                }
            }
        }

        [HttpGet("{keyWord}")]
        [Route("Search/{keyWord}")]
        public IEnumerable<News> Search(string keyWord)
        {
            var newsList = _context.News.Where(post => post.Title.ToLower().Contains(keyWord.ToLower()) || 
                                                       post.Description.ToLower().Contains(keyWord.ToLower()) ||
                                                       post.Link.ToLower().Contains(keyWord.ToLower()));

            return newsList;
        }

        public async Task<Publisher> checkPublisher(string publisherName)
        {
            try
            {
                var publisher = await _context.Publishers.Where(w => w.Name == publisherName).FirstAsync();

                return publisher;
            }
            catch (Exception)
            {
                var newPublisher = new Publisher
                {
                    Name = publisherName
                };

                await _context.Publishers.AddAsync(newPublisher);
                await _context.SaveChangesAsync();
                return newPublisher;
                throw;
            }

        }

        public async Task checkNewsGroups(List<string> groups, string postTitle)
        {
            try
            {
                var currentPost = await _context.News.Where(w => w.Title == postTitle).FirstAsync();
                foreach (var group in groups)
                {
                    var newsGroupsAssigned = false;
                    var currenNewsGroup = new NewsGroup();
                    foreach (var newsGroup in _context.NewsGroups.Include(i => i.News).Include(i => i.Groups))
                    {
                        if(newsGroup.News.Title == postTitle && newsGroup.Groups.Name == group)
                        {
                            currenNewsGroup = newsGroup;
                            newsGroupsAssigned = true;
                            break;
                        }
                    }

                    var newGroup = await checkGroups(group);

                    currenNewsGroup.Groups = newGroup;
                    currenNewsGroup.News = currentPost;
                    
                    if (!newsGroupsAssigned)
                        await _context.NewsGroups.AddAsync(currenNewsGroup);
                    else
                        _context.Entry(currenNewsGroup).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Group> checkGroups(string groupName)
        {
            try
            {
                var group = await _context.Groups.Where(w => w.Name == groupName).FirstAsync();

                return group;
            }
            catch (Exception)
            {
                var newGroup = new Group
                {
                    Name = groupName
                };

                await _context.Groups.AddAsync(newGroup);
                await _context.SaveChangesAsync();
                return newGroup;
                throw;
            }

        }

        [HttpGet]
        [Route("GetProductionVersion")]
        public IEnumerable<VersionNumber> GetProductionVersion()
        {
            return _context.VersionNumbers.Where(w => !EF.Functions.Like(w.version_number, "%.%.%-%.%"));
        }

        [HttpGet]
        [Route("GetProductionVersionWithoutBranchName")]
        public object GetProductionVersionByRemoveAndCompare()
        {
            var versionNumbers = _context.VersionNumbers;
            var versionsBranchNameRemoved  = new List<string>();
            var versionsWithoutBranchName = new List<string>();

            foreach (var version in versionNumbers)
            {
                var startIndex = version.version_number.IndexOf("-");
                var endIndex = version.version_number.LastIndexOf(".");

                if (startIndex < endIndex)
                    versionsBranchNameRemoved.Add(version.version_number.Remove(startIndex, endIndex - startIndex));

                if (startIndex > endIndex)
                {
                    versionsBranchNameRemoved.Add(version.version_number);
                    versionsWithoutBranchName.Add(version.version_number);
                }
            }

            return new { versionsBranchNameRemoved, versionsWithoutBranchName };
        }
    }

}