using macaron.Data;
using macaron.Models;
using macaron.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task GetAllProjectsIfEmpty()
        {
            var projects = await ProjectService.GetProjectsAsync(db);
            Assert.AreEqual(0, projects.Count);
        }

        [TestMethod]
        public async Task GetAllProjects()
        {
            await AddInitalDataAsync();

            var projects = await ProjectService.GetProjectsAsync(db);
            Assert.AreEqual(3, projects.Count);
        }

        [TestMethod]
        public void OkActionResult()
        {
            var res = "OK";
            var ok = ProjectService.ToActionResult(200, res);
            Assert.IsTrue(ok is OkObjectResult);
            Assert.AreEqual(res, (ok as OkObjectResult).Value);

            
        }

        [TestMethod]
        public void CreatedActionResult()
        {
            var res = "Created";
            var location = @"https://www.google.co.jp/";
            var created = ProjectService.ToActionResult(201, res, location);
            Assert.IsTrue(created is CreatedResult);
            Assert.AreEqual(res, (created as CreatedResult).Value);
            Assert.AreEqual(location, (created as CreatedResult).Location);
        }

        [TestMethod]
        public void NoContentActionResult()
        {
            var nocontent = ProjectService.ToActionResult(204, null);
            Assert.IsTrue(nocontent is NoContentResult);
        }

        [TestMethod]
        public void NotFoundActionResult()
        {
            var res = "notfound";
            var notfound = ProjectService.ToActionResult(404, res);
            Assert.IsTrue(notfound is NotFoundResult);
        }

        private async Task AddInitalDataAsync()
        {
            db.Projects.AddRange(new List<Project>()
            {
                new Project()
                {
                    Name = "test",
                    Description = null,
                    Arcived = false,
                    CreatedDate = DateTimeOffset.UtcNow.AddDays(1),
                    LastUpdateDate = DateTimeOffset.UtcNow.AddDays(2)
                },
                new Project()
                {
                    Name = "あいうえお",
                    Description = "かきくけこ",
                    Arcived = false,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                },
                new Project()
                {
                    Name = "Arcived",
                    Arcived = true,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                },
                new Project()
                {
                    Name = "Long name. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.",
                    Description = "Long description. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.",
                    Arcived = false,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                }
            });
            await db.SaveChangesAsync();
        }
    }
}
