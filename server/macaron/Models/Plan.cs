using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Plan
    /// </summary>
    public class Plan
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
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Target cases
        /// </summary>
        public virtual IList<Case> Cases { get; set; }
        /// <summary>
        /// Run logs
        /// </summary>
        public virtual IList<Run> Runs { get; set; }
        /// <summary>
        /// Plan leader
        /// </summary>
        [Required]
        public string LeaderName { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Mark the complete this plan
        /// </summary>
        [Required]
        public bool Completed { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Last update date
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
