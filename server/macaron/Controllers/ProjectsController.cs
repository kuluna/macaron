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
using Microsoft.AspNetCore.Authorization;

namespace macaron.Controllers
{
    /// <summary>
    /// Projects API
    /// </summary>
    [Route("api/projects"), Authorize]
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
                var cases = await db.Cases.Where(m => m.ProjectId == projectId)
                                          .OrderByDescending(m => m.Id).ToListAsync();
                return Ok(new ProjectDetailResponse(project, cases));
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
        /// <returns>Cases</returns>
        [HttpGet("{projectId}/cases")]
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
        /// Get all cases
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="caseId">Case ID</param>
        /// <param name="revision">Revision</param>
        /// <returns>Cases</returns>
        [HttpGet("{projectId}/cases/{caseId}")]
        public async Task<IActionResult> GetCase(int projectId, int caseId, [FromQuery] int? revision = null)
        {
            var revisions = await db.Cases.Where(t => t.ProjectId == projectId && t.AllocateId == caseId)
                                          .OrderByDescending(t => t.Revision)
                                          .ToListAsync();

            var target = (revision != null) ? revisions.Where(c => c.Revision == revision).SingleOrDefault()
                                            : revisions.FirstOrDefault();
            if (target != null)
            {
                return Ok(new CaseResponse(target));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Add case
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Milestone</returns>
        [HttpPost("{projectId}/cases")]
        public async Task<IActionResult> AddCase(int projectId, [FromBody] CaseCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (c, error) = await ProjectService.AddCaseAsync(db, projectId, req);
            
            if (c != null)
            {
                return Created($"/api/{projectId}/cases/{c.Id}", c);
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
        /// <param name="caseId">Case ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Case</returns>
        [HttpPut("{projectId}/cases/{caseId}")]
        public async Task<IActionResult> PutCase(int projectId, int caseId, [FromBody] CaseUpdateRequest req)
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
        /// <param name="caseId">Case ID</param>
        [HttpDelete("{projectId}/cases/{caseId}")]
        public async Task<IActionResult> DeleteCase(int projectId, int caseId)
        {
            await db.Cases.Where(t => t.ProjectId == projectId && t.AllocateId == caseId)
                          .ForEachAsync(c => c.IsOutdated = true);
            await db.SaveChangesAsync();

            return NoContent();
        }

#endregion

#region Plans

        /// <summary>
        /// Get all plans
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="runnable">Filtering only runnable plan</param>
        /// <returns>Plans</returns>
        [HttpGet("{projectId}/plans")]
        public async Task<IList<PlanResponse>> GetPlans(int projectId, [FromQuery] bool runnable = false)
        {
            var users = await db.Users.ToListAsync();

            return await db.Plans.Where(p => p.ProjectId == projectId)
                                 .Include(p => p.Cases)
                                 .Include(p => p.Runs)
                                 .Where(p => (p.Cases.Count > 0 && !p.Completed) || !runnable)
                                 .AsNoTracking()
                                 .Select(t => new PlanResponse(t, users))
                                 .ToListAsync();
        }

        /// <summary>
        /// Get the plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Plan ID</param>
        /// <param name="groupBySection">Cases group by SectionName</param>
        /// <returns>Plan</returns>
        [HttpGet("{projectId}/plans/{planId}")]
        public async Task<IActionResult> GetPlan(int projectId, int planId, [FromQuery] bool groupBySection = false)
        {
            var plan = await ProjectService.GetPlanAsync(db, projectId, planId, groupBySection);
            if (plan == null)
            {
                return NotFound();
            }

            var users = await db.Users.ToListAsync();
            if (groupBySection)
            {
                return Ok(new GroupedPlanResponse(plan, users));
            }
            else
            {
                return Ok(new PlanResponse(plan, users));
            }
        }

        /// <summary>
        /// Add the plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Plan</returns>
        [HttpPost("{projectId}/plans")]
        public async Task<IActionResult> PostPlans(int projectId, [FromBody] PlanCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (plan, error) = await ProjectService.AddPlanAsync(db, projectId, req);

            if (plan != null)
            {
                return Created($"/api/{projectId}/plans/{plan.Id}", plan);
            }
            else
            {
                return NotFound(error);
            }
        }

        /// <summary>
        /// Update the plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Plan ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Plan</returns>
        [HttpPut("{projectId}/plans/{planId}")]
        public async Task<IActionResult> PutPlan(int projectId, int planId, [FromBody] PlanUpdateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var plan = await ProjectService.GetPlanAsync(db, projectId, planId, false);
            if (plan == null)
            {
                return NotFound();
            }

            var project = await db.Projects.FindAsync(projectId);
            req.Update(plan, project);
            await db.SaveChangesAsync();

            return Ok(new PlanResponse(plan, await db.Users.ToListAsync()));
        }

#endregion

#region Runs
        
        /// <summary>
        /// Add run results
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="planId">Plan ID</param>
        /// <param name="requests">Request body</param>
        /// <returns></returns>
        [HttpPost("{projectId}/plans/{planId}/runs")]
        public async Task<IActionResult> PostRuns(int projectId, int planId, [FromBody] IEnumerable<RunCreateRequest> requests)
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

        /// <summary>
        /// Get all section names
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Sections.</returns>
        [HttpGet("{projectId}/sections")]
        public async Task<IActionResult> GetSections(int projectId)
        {
            var names = await db.Cases.Where(c => c.ProjectId == projectId && !c.IsOutdated)
                                      .Select(c => c.SectionName)
                                      .Distinct()
                                      .OrderBy(n => n)
                                      .ToListAsync();
            var response = new { sectionNames = names };
            return Ok(response);
        }
    }
}
