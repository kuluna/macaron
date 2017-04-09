using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Test milestone
    /// </summary>
    public class Milestone
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Completion date of milestone(Expect)
        /// </summary>
        public DateTimeOffset? ExpectedCompleteDate { get; set; }
        /// <summary>
        /// Completion date of milestone
        /// </summary>
        public DateTimeOffset? CompleteDate { get; set; }
        /// <summary>
        /// Target platforms
        /// </summary>
        public virtual List<Platform> Platforms { get; set; }
        /// <summary>
        /// Test cases
        /// </summary>
        public virtual List<Testcase> Testcases { get; set; }
    }
}
