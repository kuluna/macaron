using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(create milestone)
    /// </summary>
    public class MilestoneCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// Convert to model
        /// </summary>
        /// <returns>Milestone</returns>
        public Milestone ToMilestone()
        {
            return new Milestone()
            {
                Name = Name,
                Testcases = new List<Testcase>()
            };
        }
    }
}
