using macaron.Data;
using macaron.Models;
using macaron.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace macaron.test.pt
{
    [TestClass]
    public class ProjectServiceTest
    {
        public DatabaseContext db { get; }
        
        public ProjectServiceTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("test").Options;
            db = new DatabaseContext(options);
        }

        [TestInitialize]
        public void Before()
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task GetAllProjects()
        {
            db.Projects.AddRange(new List<Project>()
            {
                new Project()
                {
                    Name = "test",
                    Description = null,
                    Arcived = false,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                }
            });
            await db.SaveChangesAsync();

            var projects = await ProjectService.GetProjectsAsync(db);
            Assert.AreEqual(1, projects.Count);
        }
    }
}
