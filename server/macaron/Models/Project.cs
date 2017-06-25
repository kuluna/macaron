using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Test project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Description(markdown)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Arcive
        /// </summary>
        [Required]
        public bool IsArcived { get; set; }
        /// <summary>
        /// Cases
        /// </summary>
        public virtual IList<Case> Cases { get; set; }
        /// <summary>
        /// Plans
        /// </summary>
        public virtual IList<Plan> Plans { get; set; }
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
