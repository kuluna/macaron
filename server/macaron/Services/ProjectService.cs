using macaron.Data;
using macaron.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using macaron.Models.Request;
using Macaron.Models.Response;
using macaron.Models;

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
            return await db.Projects.Where(p => !p.IsArcived)
                                    .AsNoTracking()
                                    .Select(p => new ProjectResponse(p))
                                    .ToListAsync();
        }

        /// <summary>
        /// Get the project
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <returns>Project</returns>
        public static async Task<ProjectResponse> GetProjectAsync(DatabaseContext db, int projectId)
        {
            return await db.Projects.Where(p => !p.IsArcived && p.Id == projectId)
                                    .AsNoTracking()
                                    .Select(p => new ProjectResponse(p))
                                    .SingleOrDefaultAsync();
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
        public static async Task<(CaseResponse, string)> AddCaseAsync(DatabaseContext db, int projectId, CaseCreateRequest req)
        {
            var project = await db.Projects.Where(p => p.Id == projectId)
                                           .Include(p => p.Cases)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return (null, "Contains the project id does not exist.");
            }

            var testcase = req.ToTestcase();
            project.Cases.Add(testcase);
            await db.SaveChangesAsync();
            // commit case id
            testcase.AllocateId = testcase.Id;
            await db.SaveChangesAsync();

            return (new CaseResponse(testcase), null);
        }

        /// <summary>
        /// Add new testplan
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Response body or error</returns>
        public static async Task<(PlanResponse, string)> AddPlanAsync(DatabaseContext db, int projectId, PlanCreateRequest req)
        {
            var project = await db.Projects.Where(p => p.Id == projectId && !p.IsArcived)
                                           .Include(p => p.Cases)
                                           .Include(p => p.Plans)
                                             .ThenInclude(tp => tp.Runs)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return (null, "Contains the project id does not exist.");
            }

            var testplan = req.ToPlan(project);
            project.Plans.Add(testplan);
            await db.SaveChangesAsync();

            return (new PlanResponse(testplan, await db.Users.ToListAsync()), null);
        }

        /// <summary>
        /// Get the testplan
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Testplan ID</param>
        /// <param name="groupBySection">Cases group by SectionName</param>
        /// <returns>Plan or null(nothing)</returns>
        public static async Task<Plan> GetPlanAsync(DatabaseContext db, int projectId, int planId, bool groupBySection)
        {
            return await db.Plans.Where(p => p.ProjectId == projectId && p.Id == planId)
                                 .Include(p => p.Cases)
                                 .Include(p => p.Runs)
                                 .SingleOrDefaultAsync();
        }
    }
}
