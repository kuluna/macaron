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
        public bool Arcived { get; set; }
        /// <summary>
        /// Testcases
        /// </summary>
        public virtual ICollection<Testcase> Testcases { get; set; }
        /// <summary>
        /// Created datetime
        /// </summary>
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Last update datetime
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
