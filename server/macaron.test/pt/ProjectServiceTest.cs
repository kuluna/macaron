using macaron.Data;
using macaron.Models;
using macaron.Models.Request;
using macaron.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using macaron.Models.Response;

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
        public async Task GetProject()
        {
            await AddInitalDataAsync();

            var project = await ProjectService.GetProjectAsync(db, 1);
            Assert.IsNotNull(project);
        }

        [TestMethod]
        public async Task GetProjectIfNotFound()
        {
            await AddInitalDataAsync();

            var project = await ProjectService.GetProjectAsync(db, int.MaxValue);
            Assert.IsNull(project);
        }

        [TestMethod]
        public async Task GetProjectIfArcived()
        {
            await AddInitalDataAsync();

            var project = await ProjectService.GetProjectAsync(db, 3);
            Assert.IsNull(project);
        }

        [TestMethod]
        public async Task GetAllProjects()
        {
            await AddInitalDataAsync();

            var projects = await ProjectService.GetProjectsAsync(db);
            Assert.AreEqual(3, projects.Count);
        }

        [TestMethod]
        public async Task AddTestcaseAsync()
        {
            await AddInitalDataAsync();
            var projectId = (await db.Projects.FirstAsync()).Id;

            var req = new CaseCreateRequest()
            {
                Step = "test",
                Expectation = "is All OK"
            };
            var (res, error) = await ProjectService.AddCaseAsync(db, projectId, req);
            Assert.IsNull(error, error);

            var testcase = await db.Cases.FindAsync(res.Id);
            var expectResponse = new CaseResponse(testcase);

            Assert.AreEqual(expectResponse.Id, res.Id);
        }

        private async Task AddInitalDataAsync()
        {
            db.Projects.AddRange(new List<Project>()
            {
                new Project()
                {
                    Name = "test",
                    Description = null,
                    IsArcived = false,
                    CreatedDate = DateTimeOffset.UtcNow.AddDays(1),
                    LastUpdateDate = DateTimeOffset.UtcNow.AddDays(2)
                },
                new Project()
                {
                    Name = "あいうえお",
                    Description = "かきくけこ",
                    IsArcived = false,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                },
                new Project()
                {
                    Name = "Arcived",
                    IsArcived = true,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                },
                new Project()
                {
                    Name = "Long name. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.",
                    Description = "Long description. The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog.",
                    IsArcived = false,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastUpdateDate = DateTimeOffset.UtcNow
                }
            });
            await db.SaveChangesAsync();
        }
    }
}
