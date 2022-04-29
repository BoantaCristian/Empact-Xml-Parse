# README #

### EMPACT .NET Core Web API for XML Parsing and Data Management


### Back-end
   * ASP.NET Core Web Api
   * Entity Framework Core
   * SQL Express Server
 
### Languages and syntax: 
   * C#
   * SQL
   * LINQ
   * Lambda expressions
  
### Tools
   * Visual Studio for Api
   * SQL Server Management Studio for database
   * Postman to check Api http methods
 
### Estimations
   * timespan: 3 days from start to finish:
      * 1st day (~1h): documentation consisting on google search and youtube videos for xml parsing, deserialization
      * 2nd day (~6h): project interpretation + api (develop requirements 1) and 2)a)b)c))-> configure project, add db connection, models (dto: xml modeling/template, dbContext, dbModels), migrations, database, create controler: download xml, deserialize, return ordered list with desired attributes, search by keyword, asynchronous db data manipulation: add/update news, add/assign publishers, groups, newsGroups (many to many relationships between groups and news)
      * 3rd day (~3h): get publisher and groups from xml and implement extra feature for getting the production version and also for removing the branch name
      * code review and optimization (~1h)
 
### Challanges
   * deal with every error handling, every case, scenario and exception
   * deal with adding asynchronous groups and news in newsGroups right after each article iterated
   * use first time BitBucket (almost same as GitHub): found out about app password for git commands
   * decide between deserialization, xpath (more hardcoded) and ASP.NET SyndicationFeed (complex framework from NuGet Packages)
   * get publisher from xml: find what attribute - namespace
  
### Improvements
   *  find and handle each article's publisher and list of groups
   *  code legibility
   *  code optimization: 
      *  error handling for all scenarios
      *  more methods for similar purposes (polimorphism; ex: one method to add/update data entries)
      *  reduce code complexity
      *  use as many as possible async methods, because subsequent methods might need data from methods from above which can take longer to return/execute