using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public string BranchName { get; set; }
        /// <summary>
        /// Custom testcases
        /// </summary>
        public IList<TestcaseIdentity> TestcaseIds { get; set; }
        /// <summary>
        /// Plan leader
        /// </summary>
        [Required, AppUserName]
        public string LeaderName { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Find testcases for each patterns
        /// </summary>
        /// <param name="project">Project model</param>
        /// <returns>Testcases</returns>
        /// <exception cref="NotImplementedException">This method is not enough other TestPattern's case.</exception>
        protected IEnumerable<Testcase> FindTestcase(Project project)
        {
            var targetBranchTests = project.Testcases.Where(t => t.BranchName.Equals(BranchName));

            switch (TestPattern)
            {
                case TestPattern.Branch:
                    return targetBranchTests;

                case TestPattern.Ng:
                    var failIds = project.Testplans.SelectMany(p => p.Testruns.Where(t => t.Result == TestResult.Ng).Select(t => t.Id)).ToList();
                    return targetBranchTests.Where(t => failIds.Contains(t.AllocateId));

                case TestPattern.Custom:
                    return project.Testcases.Where(t => TestcaseIds.Contains(new TestcaseIdentity(t.Id, t.Revision)));

                default:
                    throw new NotImplementedException("This method is not enough other TestPattern's case.");
            }
        }

        /// <summary>
        /// Convert to testplan model
        /// </summary>
        /// <param name="project">Project model</param>
        /// <returns>Test plan model</returns>
        public Testplan ToTestplan(Project project)
        {
            return new Testplan()
            {
                Name = Name,
                LeaderName = LeaderName,
                Testcases = FindTestcase(project).ToList(),
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
