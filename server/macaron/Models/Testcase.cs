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
        /// Identity Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
        /// <summary>
        /// Allocated ID
        /// </summary>
        [Required]
        public int AllocateId { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        public int? Revision { get; set; }
        /// <summary>
        /// Branch name (default: master)
        /// </summary>
        [Required]
        public string BranchName { get; set; }
        /// <summary>
        /// Commit mode
        /// </summary>
        public CommitMode CommitMode { get; set; }
        /// <summary>
        /// Order(ASC)
        /// </summary>
        [Required]
        public int Order { get; set; }
        /// <summary>
        /// Section
        /// </summary>
        public string SectionName { get; set; }
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
        /// Delete this testcase
        /// </summary>
        [Required]
        public bool IsOutdated { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
    }

    /// <summary>
    /// Commit mode
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommitMode
    {
        /// <summary>
        /// Master commited
        /// </summary>
        Commited,
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

    /// <summary>
    /// Identity the testcase
    /// </summary>
    public class TestcaseIdentity
    {
        /// <summary>
        /// Testcase ID
        /// </summary>
        [Required]
        public int TestcaseId { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        [Required]
        public int Revision { get; set; }

        /// <summary>
        /// True if the same ID and revision.
        /// </summary>
        /// <param name="obj">TestcaseIdentity object</param>
        /// <returns>Result</returns>
        public override bool Equals(object obj)
        {
            // block rule
            if (obj == null || !(obj is TestcaseIdentity))
            {
                return false;
            }

            var compare = obj as TestcaseIdentity;
            return (TestcaseId == compare.TestcaseId && Revision == compare.Revision);
        }

        /// <summary>
        /// Get the hashcode.
        /// </summary>
        /// <returns>Cashcode</returns>
        public override int GetHashCode() => base.GetHashCode();
    }
}
