using macaron.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(create testplan)
    /// </summary>
    public class TestplanCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Test pattern
        /// </summary>
        [Required]
        public TestPattern TestPattern { get; set; }
        /// <summary>
        /// Target branch name
        /// </summary>
        [Required, MinLength(1)]
        public string BranchName { get; set; }
        /// <summary>
        /// Custom testcases
        /// </summary>
        public IList<TestcaseIdentity> TestcaseIds { get; set; }
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
        /// Convert to testplan model
        /// </summary>
        /// <returns></returns>
        public async Task<Testplan> ToTestplanAsync(DatabaseContext db, int projectId)
        {
            List<Testcase> testcases = null;
            switch (TestPattern)
            {
                case TestPattern.Branch:
                    testcases = await db.Testcases.Where(t =>
                        t.ProjectId == projectId && t.BranchName.Equals(BranchName))
                        .ToListAsync();
                    break;
                case TestPattern.Ng:
                    var failIds = await db.Testruns.Where(t => t.Result == TestResult.Ng).Select(t => t.Id).ToListAsync();
                    testcases = await db.Testcases.Where(t =>
                        t.ProjectId == projectId && t.BranchName.Equals(BranchName) && failIds.Contains((int)t.AllocateId))
                        .ToListAsync();
                    break;
                case TestPattern.Custom:
                    testcases = await db.Testcases.Where(t =>
                        TestcaseIds.Contains(new TestcaseIdentity() { TestcaseId = t.Id, Revision = (int)t.Revision }))
                        .ToListAsync();
                    break;
            }

            return new Testplan()
            {
                Name = Name,
                LeaderId = LeaderId,
                Testcases = testcases,
                Testruns = new List<Testrun>(),
                DueDate = DueDate,
                Completed = false,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }

    /// <summary>
    /// Test pattern
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestPattern
    {
        /// <summary>
        /// All testcases in branch
        /// </summary>
        Branch,
        /// <summary>
        /// All NG testcases
        /// </summary>
        Ng,
        /// <summary>
        /// User choice testcases
        /// </summary>
        Custom
    }
}
