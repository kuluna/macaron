using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Data;
using Microsoft.EntityFrameworkCore;
using macaron.Models.Request;
using macaron.Models.Response;
using macaron.Models;
using System;

namespace macaron.Controllers
{
    /// <summary>
    /// Projects API
    /// </summary>
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private DatabaseContext db;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectsController(DatabaseContext db)
        {
            this.db = db;
        }

#region Projects

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>Projects</returns>
        [HttpGet]
        public async Task<IEnumerable<ProjectResponse>> GetProjects()
        {
            return await db.Projects.AsNoTracking().Select(p => new ProjectResponse(p)).ToListAsync();
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
                var testcases = await db.Testcases.Where(m => m.ProjectId == projectId)
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

            var project = req.ToProject();
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return Created($"{HttpContext.Request.Path}/{project.Id}", new ProjectResponse(project));
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
        
#region Testcases

        /// <summary>
        /// Get all testcases
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Testcases</returns>
        [HttpGet("{projectId}/testcases")]
        public async Task<ICollection<TestcaseResponse>> GetTestcases(int projectId)
        {
            return await db.Testcases.Where(t => t.ProjectId == projectId && !t.IsDeleted)
                                     .AsNoTracking()
                                     .Select(t => new TestcaseResponse(t))
                                     .ToListAsync();
        }

        /// <summary>
        /// Add test case
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Milestone</returns>
        [HttpPost("{projectId}/testcases")]
        public async Task<IActionResult> AddTestcase(int projectId, [FromBody] TestcaseCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == projectId)
                                           .Include(p => p.Testcases)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound("Contains the testcase id does not exist.");
            }

            try
            {
                var testcase = req.ToTestcase();
                project.Testcases.Add(testcase);
                await db.SaveChangesAsync();
                // commit case id
                testcase.AllocateId = testcase.Id;
                await db.SaveChangesAsync();

                return Created($"/api/{projectId}/testcases/{testcase.Id}", new TestcaseResponse(testcase));
            }
            catch (Exception er)
            {
                Console.WriteLine(er.StackTrace);
                return BadRequest();
            }
        }

#endregion

#region Testplans

        /// <summary>
        /// Get all test plans
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns>Test plans</returns>
        [HttpGet("{projectId}/testplans")]
        public async Task<IList<Testplan>> GetTestplans(int projectId)
        {
            return await db.Testplans.Where(t => t.ProjectId == projectId)
                                     .ToListAsync();
        }

        /// <summary>
        /// Add the test plan
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Test plan</returns>
        [HttpPost("{projectId}/testplans")]
        public async Task<IActionResult> PostTestplans(int projectId, [FromBody] TestplanCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == projectId && !p.Arcived)
                                           .Include(p => p.Testplans)
                                           .SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }

            var testplan = await req.ToTestplanAsync(db);
            project.Testplans.Add(testplan);
            await db.SaveChangesAsync();

            return Created("", testplan);
        }

#endregion

#region Testruns

        /*
        /// <summary>
        /// Add testrun results
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="requests">Request body</param>
        /// <returns></returns>
        [HttpPost("{projectId}/testruns")]
        public async Task<IActionResult> PostTestruns(int projectId, [FromBody] IEnumerable<TestrunCreateRequest> requests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var testcaseIds = await db.Testcases.Where(t => t.ProjectId == projectId && !t.IsDeleted)
                                                .Select(t => t.Id)
                                                .ToListAsync();

            foreach (var req in requests)
            {
                if (!testcaseIds.Any(id => id == req.TestcaseId))
                {
                    return NotFound();
                }

                var testrun = req.ToTestrun();
                db.Testruns.Add(testrun);
            }
            await db.SaveChangesAsync();

            return NoContent();
        }
        */

        #endregion
    }
}
