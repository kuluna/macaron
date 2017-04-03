using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using macaron.Data;
using Microsoft.EntityFrameworkCore;
using macaron.Models;
using macaron.Models.Request;

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

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns>Projects</returns>
        [HttpGet]
        public async Task<IEnumerable<Project>> Get()
        {
            return await db.Projects.Include(p => p.Platforms).ToListAsync();
        }

        /// <summary>
        /// Get the project
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Project</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await db.Projects.Where(p => p.Id == id).Include(p => p.Platforms).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }
            
            return Ok(project);
        }

        /// <summary>
        /// Create new project
        /// </summary>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProjectCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = req.ToProject();
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return Created($"{HttpContext.Request.Path}/{project.Id}", project);
        }

        /// <summary>
        /// Update the project
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ProjectUpdateRequest req)
        {
            var project = await db.Projects.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }

            req.Update(ref project);
            await db.SaveChangesAsync();
            return Ok(project);
        }

        /// <summary>
        /// Delete the project
        /// </summary>
        /// <param name="id">ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Add target platform
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="req">Request body</param>
        /// <returns>Project</returns>
        [HttpPost("{id}/platforms")]
        public async Task<IActionResult> AddPlatformAsync(int id, [FromBody] PlatformCreateRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await db.Projects.Where(p => p.Id == id).Include(p => p.Platforms).SingleOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }
            
            project.Platforms.Add(req.ToPlatform());
            await db.SaveChangesAsync();
            return Created($"{HttpContext.Request.PathBase}/api/projects/{id}", project);
        }
    }
}
