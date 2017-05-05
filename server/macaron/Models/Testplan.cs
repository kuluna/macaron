using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Test planning
    /// </summary>
    public class Testplan
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Target testcases
        /// </summary>
        public virtual IList<Testcase> Testcases { get; set; }
        /// <summary>
        /// Run test logs
        /// </summary>
        public virtual IList<Testrun> Testruns { get; set; }
        /// <summary>
        /// Plan leader
        /// </summary>
        [Required]
        public Guid LeaderId { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Mark the complete this plan
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
