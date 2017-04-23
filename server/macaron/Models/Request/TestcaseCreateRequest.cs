using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Testcase create)
    /// </summary>
    public class TestcaseCreateRequest
    {
        /// <summary>
        /// Track the base testcase id
        /// </summary>
        public int? TrackingId { get; set; }
        /// <summary>
        /// Section
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// Branch name (if null, automatically "master")
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Want you to test carefully
        /// </summary>
        [Required]
        public bool MoreCareful { get; set; }
        /// <summary>
        /// Estimates
        /// </summary>
        [Required]
        public double Estimates { get; set; }
        /// <summary>
        /// Test precondition
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// Test step
        /// </summary>
        [Required, MinLength(1)]
        public string Test { get; set; }
        /// <summary>
        /// Expect test result
        /// </summary>
        [Required, MinLength(1)]
        public string Expect { get; set; }

        /// <summary>
        /// Convert to model
        /// </summary>
        /// <returns>Test case</returns>
        public Testcase ToTestcase()
        {
            return new Testcase()
            {
                TrackingId = TrackingId,
                SectionName = SectionName,
                BranchName = BranchName ?? "master",
                IsCommited = false,
                CommitMode = CommitMode.Add,
                Order = int.MaxValue,
                MoreCareful = MoreCareful,
                Estimates = Estimates,
                Precondition = Precondition,
                Test = Test,
                Expect = Expect,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
