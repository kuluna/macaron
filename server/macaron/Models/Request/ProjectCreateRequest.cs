using System;
using System.Collections.Generic;
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
        /// Convert to model
        /// </summary>
        /// <returns>Project</returns>
        public Project ToProject()
        {
            var defaultMilestone = new List<Milestone>
            {
                new Milestone()
                {
                    Name = Name,
                    Platforms = new List<Platform>
                    {
                        new Platform()
                        {
                            Name = "default"
                        }
                    },
                    Testcases = new List<Testcase>()
                }
            };

            return new Project()
            {
                Name = Name,
                Milestones = defaultMilestone,
                Arcived = false,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
