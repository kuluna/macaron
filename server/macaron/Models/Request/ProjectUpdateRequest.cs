using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Update project)
    /// </summary>
    public class ProjectUpdateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Arcive
        /// </summary>
        [Required]
        public bool Arcived { get; set; }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="project">Project</param>
        public void Update(ref Project project)
        {
            project.Name = Name;
            project.Arcived = Arcived;
            project.LastUpdateDate = DateTimeOffset.UtcNow;
        }
    }
}
