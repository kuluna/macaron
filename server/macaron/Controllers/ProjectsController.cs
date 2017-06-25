using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Data;
using Microsoft.EntityFrameworkCore;
using macaron.Models.Request;
using macaron.Models.Response;
using macaron.Models;
using System.Data;
using System;
using macaron.Services;
using Macaron.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace macaron.Controllers
{
    /// <summary>
    /// Projects API
    /// </summary>
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private DatabaseContext db;
        private UserManager<AppUser> userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectsController(DatabaseContext db, UserManager<AppUser> um)
        {
            this.db = db;
            userManager = um;
        }

#region Projects

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>Projects</returns>
        [HttpGet]
        public async Task<IEnumerable<ProjectResponse>> GetProjects()
        {
            return await ProjectService.GetProjectsAsync(db);
        }

        /// <summary>
        /// Get the project
        /// </summary>
        /// <param name="projectId">ID</param>
        /// <param name="detail">Get the all project info</param>
        /// <returns>Project</returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(int projectId, [FromQuery] bool detail = false)
        {
            var project = await db.Projects.Where(p => p.Id == projectId).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }

            if (detail)
            {
                var testcases = await db.Cases.Where(m => m.ProjectId == projectId)
                                              .OrderByDescending(m => m.Id).ToListAsync();
                return Ok(new ProjectDetailResponse(project, testcases));
            }
            else
            {
                return Ok(new ProjectResponse(project));
            }
        }

        /// <summary>
        /// Create new project
        /// </summary>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody]ProjectCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await ProjectService.AddProjectAsync(db, req);
            return Created($"{HttpContext.Request.Path}/{project.Id}", project);
        }

        /// <summary>
        /// Update the project
        /// </summary>
        /// <param name="projectId">ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPut("{projectId}")]
        public async Task<IActionResult> PutProject(int projectId, [FromBody]ProjectUpdateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == projectId).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }
            
            req.Update(project);
            await db.SaveChangesAsync();
            return Ok(new ProjectResponse(project));
        }

        /// <summary>
        /// Delete the project
        /// </summary>
        /// <param name="projectId">ID</param>
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var project = await db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return NoContent();
        }

        #endregion
        
#region Cases

        /// <summary>
        /// Get all cases
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="groupBySection">Group by SectionName</param>
        /// <returns>Testcases</returns>
        [HttpGet("{projectId}/testcases")]
        public async Task<IActionResult> GetCases(int projectId, [FromQuery] bool groupBySection = false)
        {
            var cases = await db.Cases.Where(t => t.ProjectId == projectId && !t.IsOutdated)
                                      .AsNoTracking()
                                      .Select(t => new CaseResponse(t))
                                      .ToListAsync();
            if(groupBySection)
            {
                return Ok(GroupedCaseResponse.ToGroupedCaseResponse(cases));
            }
            else
            {
                return Ok(cases);
            }
        }

        /// <summary>
        /// Add case
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Milestone</returns>
        [HttpPost("{projectId}/testcases")]
        public async Task<IActionResult> AddCase(int projectId, [FromBody] CaseCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (c, error) = await ProjectService.AddCaseAsync(db, projectId, req);
            
            if (c != null)
            {
                return Created($"/api/{projectId}/testcases/{c.Id}", c);
            }
            else
            {
                return NotFound(error);
            }
        }

        /// <summary>
        /// Update case
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="caseId">Testcase ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Testcase</returns>
        [HttpPut("{projectId}/testcases/{caseId}")]
        public async Task<IActionResult> PutTestcase(int projectId, int caseId, [FromBody] CaseUpdateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Serialize transaction because read, up revision, and insert process should be consistently
            using (var tran = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable))
            {
                try
                {
                    var project = await db.Projects.Where(p => p.Id == projectId && !p.IsArcived)
                                                   .Include(p => p.Cases)
                                                   .SingleOrDefaultAsync();

                    var targetCases = project?.Cases.Where(c => c.AllocateId == caseId).ToList();
                    if (targetCases == null || targetCases.Count == 0)
                    {
                        return NotFound();
                    }


                    // mark old revision
                    foreach (var t in targetCases)
                    {
                        t.IsOutdated = true;
                    }

                    var newCase = req.ToCase(targetCases, caseId);
                    project.Cases.Add(newCase);
                    await db.SaveChangesAsync();

                    tran.Commit();
                    return Ok(new CaseResponse(newCase));
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Delete case(all revision)
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="testcaseId">Testcase ID</param>
        [HttpDelete("{projectId}/testcases/{testcaseId}")]
        public async Task<IActionResult> DeleteTestcase(int projectId, int testcaseId)
        {
            await db.Cases.Where(t => t.ProjectId == projectId && t.AllocateId == testcaseId)
                          .ForEachAsync(testcase => testcase.IsOutdated = true);
            await db.SaveChangesAsync();

            return NoContent();
        }

#endregion

#region Testplans

        /// <summary>
        /// Get all test plans
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="runnable">Filtering only testable plan</param>
        /// <returns>Test plans</returns>
        [HttpGet("{projectId}/testplans")]
        public async Task<IList<PlanResponse>> GetPlans(int projectId, [FromQuery] bool runnable = false)
        {
            var users = await db.Users.ToListAsync();

            return await db.Plans.Where(p => p.ProjectId == projectId)
                                     .Include(p => p.Cases)
                                     .Include(p => p.Runs)
                                     .Where(p => p.Cases.Count > 0 || !runnable)
                                     .AsNoTracking()
                                     .Select(t => new PlanResponse(t, users))
                                     .ToListAsync();
        }

        /// <summary>
        /// Get the testplan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Testplan ID</param>
        /// <returns>Testplan</returns>
        [HttpGet("{projectId}/testplans/{planId}")]
        public async Task<IActionResult> GetTestplan(int projectId, int planId)
        {
            var testplan = await ProjectService.GetPlanAsync(db, projectId, planId);
            if (testplan == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(testplan);
            }
        }

        /// <summary>
        /// Add the test plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Test plan</returns>
        [HttpPost("{projectId}/testplans")]
        public async Task<IActionResult> PostTestplans(int projectId, [FromBody] PlanCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (testplan, error) = await ProjectService.AddPlanAsync(db, projectId, req);

            if (testplan != null)
            {
                return Created($"/api/{projectId}/testplans/{testplan.Id}", testplan);
            }
            else
            {
                return NotFound(error);
            }
        }

        /// <summary>
        /// Update the test plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Test plan ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Test plan</returns>
        [HttpPut("{projectId}/testplans/{planId}")]
        public async Task<IActionResult> PutTestplan(int projectId, int planId, [FromBody] PlanUpdateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == projectId && !p.IsArcived)
                                           .Include(p => p.Cases)
                                           .Include(p => p.Plans)
                                             .ThenInclude(tp => tp.Runs)
                                           .SingleOrDefaultAsync();

            var targetPlan = project?.Plans.Where(p => p.Id == planId).SingleOrDefault();
            if (targetPlan == null)
            {
                return NotFound();
            }

            req.Update(targetPlan, project);
            await db.SaveChangesAsync();

            return Ok(new PlanResponse(targetPlan, await db.Users.ToListAsync()));
        }

#endregion

#region Runs
        
        /// <summary>
        /// Add testrun results
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Test plan ID</param>
        /// <param name="requests">Request body</param>
        /// <returns></returns>
        [HttpPost("{projectId}/testplans/{planId}/testruns")]
        public async Task<IActionResult> PostTestruns(int projectId, int planId, [FromBody] IEnumerable<RunCreateRequest> requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var targetPlan = await db.Plans.Where(p => p.ProjectId == projectId && p.Id == planId)
                                             .Include(t => t.Cases)
                                             .Include(t => t.Runs)
                                             .SingleOrDefaultAsync();
            if (targetPlan == null)
            {
                return NotFound();
            }

            foreach (var req in requests)
            {
                if (targetPlan.Cases.Any(t => t.AllocateId == req.CaseId && t.Revision == req.CaseRevision))
                {
                    var run = req.ToRun(planId);
                    targetPlan.Runs.Add(run);
                }
                else
                {
                    return NotFound();
                }
            }
            await db.SaveChangesAsync();

            return Ok(targetPlan);
        }

#endregion
    }
}
