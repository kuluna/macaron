using macaron.Data;
using macaron.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using macaron.Models.Request;
using Macaron.Models.Response;

namespace macaron.Services
{
    /// <summary>
    /// Project API logics
    /// </summary>
    public class ProjectService
    {
        private ProjectService() { }

        /// <summary>
        /// Get projects
        /// </summary>
        /// <param name="db">Database context</param>
        /// <returns>Projects</returns>
        public static async Task<IList<ProjectResponse>> GetProjectsAsync(DatabaseContext db)
        {
            return await db.Projects.Where(p => !p.Arcived)
                                    .AsNoTracking()
                                    .Select(p => new ProjectResponse(p))
                                    .ToListAsync();
        }

        /// <summary>
        /// Add the project
        /// </summary>
        /// <param name="db">Databasec context</param>
        /// <param name="req">Request body</param>
        /// <returns>Response body</returns>
        public static async Task<ProjectResponse> AddProjectAsync(DatabaseContext db,  ProjectCreateRequest req)
        {
            var project = req.ToProject();
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return new ProjectResponse(project);
        }

        /// <summary>
        /// Add the testcase
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Response body or error</returns>
        public static async Task<(TestcaseResponse, string)> AddTestcaseAsync(DatabaseContext db, int projectId, TestcaseCreateRequest req)
        {
            var project = await db.Projects.Where(p => p.Id == projectId)
                                           .Include(p => p.Testcases)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return (null, "Contains the project id does not exist.");
            }

            var testcase = req.ToTestcase();
            project.Testcases.Add(testcase);
            await db.SaveChangesAsync();
            // commit case id
            testcase.AllocateId = testcase.Id;
            await db.SaveChangesAsync();

            return (new TestcaseResponse(testcase), null);
        }

        /// <summary>
        /// Add new testplan
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Response body or error</returns>
        public static async Task<(TestplanResponse, string)> AddTestplanAsync(DatabaseContext db, int projectId, TestplanCreateRequest req)
        {
            var project = await db.Projects.Where(p => p.Id == projectId && !p.Arcived)
                                           .Include(p => p.Testcases)
                                           .Include(p => p.Testplans)
                                             .ThenInclude(tp => tp.Testruns)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return (null, "Contains the project id does not exist.");
            }

            var testplan = req.ToTestplan(project);
            project.Testplans.Add(testplan);
            await db.SaveChangesAsync();

            return (new TestplanResponse(testplan, await db.Users.ToListAsync()), null);
        }

        /// <summary>
        /// Get the testplan
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <param name="testplanId">Testplan ID</param>
        /// <returns>Testplan or null(nothing)</returns>
        public static async Task<TestplanResponse> GetTestplanAsync(DatabaseContext db, int projectId, int testplanId)
        {
            var users = await db.Users.ToListAsync();
            return await db.Testplans.Where(t => t.ProjectId == projectId && t.Id == testplanId)
                                     .Include(t => t.Testcases)
                                     .Include(t => t.Testruns)
                                     .AsNoTracking()
                                     .Select(t => new TestplanResponse(t, users))
                                     .SingleOrDefaultAsync();
        }
    }
}
