using macaron.Data;
using macaron.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using macaron.Models.Request;

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

        public static async Task<ProjectResponse> AddProjectAsync(DatabaseContext db,  ProjectCreateRequest req)
        {
            var project = req.ToProject();
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return new ProjectResponse(project);
        }

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
    }
}
