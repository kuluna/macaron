using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        /// Convert to model
        /// </summary>
        /// <returns></returns>
        public Project ToProject()
        {
            return new Project()
            {
                Name = Name,
                Platforms = new List<Platform>(),
                Arcived = false,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
