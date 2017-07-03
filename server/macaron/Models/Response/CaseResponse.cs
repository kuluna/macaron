using System;
using System.Collections.Generic;
using System.Linq;

namespace macaron.Models.Response
{
    /// <summary>
    /// Response body(case)
    /// </summary>
    public class CaseResponse
    {
        /// <summary>
        /// Identity Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        public int Revision { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Section
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// Want you to test carefully
        /// </summary>
        public bool IsCarefully { get; set; }
        /// <summary>
        /// Estimates (hour)
        /// </summary>
        public double Estimates { get; set; }
        /// <summary>
        /// Test precondition (markdown format)
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// Test step (markdown format)
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// Expect test result (markdown format)
        /// </summary>
        public string Expectation { get; set; }
        /// <summary>
        /// Run logs
        /// </summary>
        public IList<Run> Runs { get; set; }
        /// <summary>
        /// Last result
        /// </summary>
        public TestResult LastResult { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        public DateTimeOffset LastUpdateDate { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Testcase model</param>
        public CaseResponse(Case model)
        {
            Id = model.AllocateId;
            ProjectId = model.ProjectId;
            Revision = model.Revision;
            Order = model.Order;
            SectionName = model.SectionName;
            IsCarefully = model.IsCarefully;
            Estimates = model.Estimates;
            Precondition = model.Precondition;
            Step = model.Step;
            Expectation = model.Expectation;
            Runs = new List<Run>();
            LastResult = TestResult.NotTest;
            LastUpdateDate = model.LastUpdateDate;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Testcase model</param>
        /// <param name="runs">Test runs</param>
        public CaseResponse(Case model, IEnumerable<Run> runs) : this(model)
        {
            Runs = runs.Where(t => t.CaseId == model.AllocateId && t.CaseRevision == model.Revision)
                                  .OrderByDescending(t => t.LastUpdateDate)
                                  .ToList();
            LastResult = Runs.OrderByDescending(r => r.Id).FirstOrDefault()?.Result ?? TestResult.NotTest;
        }
    }

    /// <summary>
    /// Response body(grouped case)
    /// </summary>
    public class GroupedCaseResponse
    {
        /// <summary>
        /// Section name
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// Grouped cases
        /// </summary>
        public IList<CaseResponse> Cases { get; set; }
        /// <summary>
        /// Total OK count
        /// </summary>
        public int OkCount { get; set; }
        /// <summary>
        /// Total NG count
        /// </summary>
        public int NgCount { get; set; }
        /// <summary>
        /// Total nottest count
        /// </summary>
        public int NotTestCount { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="section">Grouped sections</param>
        public GroupedCaseResponse(IGrouping<string, CaseResponse> section)
        {
            SectionName = section.Key;
            Cases = section.ToList();
            OkCount = Cases.Count(c => c.LastResult == TestResult.Ok);
            NgCount = Cases.Count(c => c.LastResult == TestResult.Ng);
            NotTestCount = Cases.Count(c => c.LastResult == TestResult.NotTest);
        }

        /// <summary>
        /// Convert to grouped case model
        /// </summary>
        /// <param name="cases">Cases</param>
        /// <returns>Response body</returns>
        public static IEnumerable<GroupedCaseResponse> ToGroupedCaseResponse(IEnumerable<CaseResponse> cases)
        {
            return cases.GroupBy(k => k.SectionName).Select(g => new GroupedCaseResponse(g));
        }
    }
}
