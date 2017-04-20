﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Data;
using Microsoft.EntityFrameworkCore;
using macaron.Models.Request;
using macaron.Models.Response;
using macaron.Models;

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
                var milestones = await db.Milestones.Where(m => m.ProjectId == projectId)
                                                    .Include(m => m.Platforms)
                                                    .Include(m => m.Testcases)
                                                    .OrderByDescending(m => m.Id).ToListAsync();
                return Ok(new ProjectDetailResponse(project, milestones));
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

#region Milestones

        /// <summary>
        /// Get all milestones
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <returns></returns>
        [HttpGet("{projectId}/milestones")]
        public async Task<IEnumerable<Milestone>> GetMilestones(int projectId)
        {
            return await db.Milestones.Where(m => m.ProjectId == projectId).ToListAsync();
        }

        /// <summary>
        /// Get the milestone
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="milestoneId">Milestone ID</param>
        /// <returns>Milestone</returns>
        [HttpGet("{projectId}/milestones/{milestoneId}")]
        public async Task<IActionResult> GetMilestone(int projectId, int milestoneId)
        {
            var milestone = await db.Milestones.Where(m => m.ProjectId == projectId && m.Id == milestoneId).SingleOrDefaultAsync();
            if (milestone == null)
            {
                return NotFound();
            }

            return Ok(milestone);
        }

        /// <summary>
        /// Add milestone
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPost("{projectId}/milestones")]
        public async Task<IActionResult> AddMilestone(int projectId, [FromBody] MilestoneCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == projectId).Include(p => p.Milestones).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }
            
            project.Milestones.Add(req.ToMilestone());
            await db.SaveChangesAsync();
            return Created($"{HttpContext.Request.PathBase}/api/projects/{projectId}", project);
        }

        /// <summary>
        /// Add target platform
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="milestoneId">Milestone ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Milestone</returns>
        [HttpPost("{projectId}/milestones/{milestoneId}/platforms")]
        public async Task<IActionResult> AddPlatform(int projectId, int milestoneId, [FromBody] PlatformCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var milestone = await db.Milestones.Where(m => m.ProjectId == projectId && m.Id == milestoneId)
                                               .Include(m => m.Platforms)
                                               .Include(m => m.Testcases)
                                               .SingleOrDefaultAsync();
            if (milestone == null)
            {
                return NotFound();
            }

            milestone.Platforms.Add(req.ToPlatform());
            await db.SaveChangesAsync();
            return Created("", milestone);
        }

        #endregion

#region Testcases

        /// <summary>
        /// Get all testcases
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="milestoneId">Milestone ID</param>
        /// <returns>Testcases</returns>
        [HttpGet("{projectId}/milestones/{milestoneId}/testcases")]
        public async Task<IActionResult> GetTestcases(int projectId, int milestoneId)
        {
            if (!await db.Milestones.AnyAsync(m => m.ProjectId == projectId && m.Id == milestoneId))
            {
                return NotFound();
            }

            var testcases = await db.Testcases.Where(t => t.MilestoneId == milestoneId)
                                              .OrderBy(t => t.Order)
                                              .OrderByDescending(t => t.LastUpdateDate).ToListAsync();
            return Ok(testcases);
        }

        /// <summary>
        /// Add test case
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="milestoneId">Milestone ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Milestone</returns>
        [HttpPost("{projectId}/milestones/{milestoneId}/testcases")]
        public async Task<IActionResult> AddTestcase(int projectId, int milestoneId, [FromBody] TestcaseCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var milestone = await db.Milestones.Where(m => m.ProjectId == projectId && m.Id == milestoneId)
                                               .Include(m => m.Platforms)
                                               .Include(m => m.Testcases)
                                               .SingleOrDefaultAsync();
            if (milestone == null)
            {
                return NotFound();
            }

            milestone.Testcases.Add(req.ToTestcase());
            await db.SaveChangesAsync();
            return Created("", milestone);
        }

#endregion
    }
}
