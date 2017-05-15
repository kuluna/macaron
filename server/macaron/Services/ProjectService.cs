using macaron.Data;
using macaron.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Convert to ActionResult
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="responseBody">Response body(optional)</param>
        /// <param name="location">Resource access uri(optional)</param>
        /// <returns>ActionResult</returns>
        public static IActionResult ToActionResult(int statusCode, object responseBody, string location = null)
        {
            switch (statusCode)
            {
                case 200:
                    return new OkObjectResult(responseBody);
                case 201:
                    return new CreatedResult(location, responseBody);
                case 204:
                    return new NoContentResult();

                case 404:
                    return new NotFoundResult();


                default:
                    return new StatusCodeResult(statusCode);
            }
        }
    }
}
