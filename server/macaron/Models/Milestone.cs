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
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Platforms
        /// </summary>
        public List<Platform> Platforms { get; set; }
        /// <summary>
        /// Test case
        /// </summary>
        public List<Testcase> Testcases { get; set; }
    }
}
