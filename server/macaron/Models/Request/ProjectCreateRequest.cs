using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Create project)
    /// </summary>
    public class ProjectCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Description(Markdown)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Convert to model
        /// </summary>
        /// <returns>Project</returns>
        public Project ToProject()
        {
            return new Project()
            {
                Name = Name,
                Description = Description,
                IsArcived = false,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
