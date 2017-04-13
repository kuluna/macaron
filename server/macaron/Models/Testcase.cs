using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Testcase
    /// </summary>
    public class Testcase
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Track the base testcase id
        /// </summary>
        public int TrackingId { get; set; }
        /// <summary>
        /// Branch name (default: master)
        /// </summary>
        [Required]
        public string BranchName { get; set; }
        /// <summary>
        /// Commited
        /// </summary>
        [Required]
        public bool IsCommited { get; set; }
        /// <summary>
        /// Commit mode
        /// </summary>
        [Required, JsonConverter(typeof(StringEnumConverter))]
        public CommitMode CommitMode { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        [Required]
        public int Order { get; set; }
        /// <summary>
        /// Want you to test carefully
        /// </summary>
        [Required]
        public bool MoreCareful { get; set; }
        /// <summary>
        /// Estimates (hour)
        /// </summary>
        [Required]
        public double Estimates { get; set; }
        /// <summary>
        /// Test precondition (markdown format)
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// Test step (markdown format)
        /// </summary>
        [Required, MinLength(1)]
        public string Test { get; set; }
        /// <summary>
        /// Expect test result (markdown format)
        /// </summary>
        [Required, MinLength(1)]
        public string Expect { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
        /// <summary>
        /// Revisions
        /// </summary>
        public virtual Testcase Revision { get; set; }
    }

    /// <summary>
    /// Commit mode
    /// </summary>
    public enum CommitMode
    {
        /// <summary>
        /// Add
        /// </summary>
        Add,
        /// <summary>
        /// Modify change
        /// </summary>
        Modify,
        /// <summary>
        /// Delete testcase
        /// </summary>
        Delete
    }
}
