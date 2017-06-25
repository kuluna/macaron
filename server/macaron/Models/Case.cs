using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Case
    /// </summary>
    public class Case
    {
        /// <summary>
        /// Identity ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        [Required, Range(0, int.MaxValue)]
        public int ProjectId { get; set; }
        /// <summary>
        /// Allocated ID
        /// </summary>
        [Required, Range(0, int.MaxValue)]
        public int AllocateId { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        [Required, Range(0, int.MaxValue)]
        public int Revision { get; set; }
        /// <summary>
        /// Order(ASC)
        /// </summary>
        [Required, Range(0, int.MaxValue)]
        public int Order { get; set; }
        /// <summary>
        /// Section name (default: Tests)
        /// </summary>
        [Required, MinLength(1)]
        public string SectionName { get; set; }
        /// <summary>
        /// Want to test more carefully
        /// </summary>
        [Required]
        public bool IsCarefully { get; set; }
        /// <summary>
        /// Estimates (min)
        /// </summary>
        [Required, Range(0, double.MaxValue)]
        public double Estimates { get; set; }
        /// <summary>
        /// Precondition (markdown format)
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// step (markdown format)
        /// </summary>
        [Required, MinLength(1)]
        public string Step { get; set; }
        /// <summary>
        /// Expect result (markdown format)
        /// </summary>
        [MinLength(1)]
        public string Expectation { get; set; }
        /// <summary>
        /// Delete this case
        /// </summary>
        [Required]
        public bool IsOutdated { get; set; }
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

    /// <summary>
    /// Identity the case
    /// </summary>
    public class CaseIdentity
    {
        /// <summary>
        /// Case ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        [Required]
        public int Revision { get; set; }

        /// <summary>
        /// Identity of testcase
        /// </summary>
        /// <param name="testcaseId">Testcase ID</param>
        /// <param name="revision">Revision</param>
        public CaseIdentity(int testcaseId, int revision)
        {
            Id = testcaseId;
            Revision = revision;
        }

        /// <summary>
        /// True if the same ID and revision.
        /// </summary>
        /// <param name="obj">TestcaseIdentity object</param>
        /// <returns>Result</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CaseIdentity))
            {
                return false;
            }

            var compare = obj as CaseIdentity;
            return (Id == compare.Id && Revision == compare.Revision);
        }

        /// <summary>
        /// Get the hashcode.
        /// </summary>
        /// <returns>Cashcode</returns>
        public override int GetHashCode() => base.GetHashCode();
    }
}
