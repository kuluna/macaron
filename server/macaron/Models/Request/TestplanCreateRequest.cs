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
        [MinLength(1)]
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
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <returns>Testcases</returns>
        /// <exception cref="NotImplementedException">This method is not enough other TestPattern's case.</exception>
        protected async Task<IList<Testcase>> FindTestcaseAsync(DatabaseContext db, int projectId)
        {
            switch (TestPattern)
            {
                case TestPattern.Branch:
                    return await db.Testcases.Where(t =>
                        t.ProjectId == projectId && t.BranchName.Equals(BranchName))
                        .ToListAsync();
                case TestPattern.Ng:
                    var failIds = await db.Testruns.Where(t => t.Result == TestResult.Ng).Select(t => t.Id).ToListAsync();
                    return await db.Testcases.Where(t =>
                        t.ProjectId == projectId && t.BranchName.Equals(BranchName) && failIds.Contains((int)t.AllocateId))
                        .ToListAsync();
                case TestPattern.Custom:
                    return await db.Testcases.Where(t =>
                        TestcaseIds.Contains(new TestcaseIdentity() { TestcaseId = t.Id, Revision = (int)t.Revision }))
                        .ToListAsync();
                default:
                    throw new NotImplementedException("This method is not enough other TestPattern's case.");
            }
        }

        /// <summary>
        /// Convert to testplan model
        /// </summary>
        /// <param name="db">Database context</param>
        /// <param name="projectId">Project ID</param>
        /// <returns>Test plan model</returns>
        public async Task<Testplan> ToTestplanAsync(DatabaseContext db, int projectId)
        {
            return new Testplan()
            {
                Name = Name,
                LeaderId = LeaderId,
                Testcases = await FindTestcaseAsync(db, projectId),
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
