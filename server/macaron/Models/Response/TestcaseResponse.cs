using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Response
{
    /// <summary>
    /// Response body(Testcase)
    /// </summary>
    public class TestcaseResponse : Testcase
    {
        /// <summary>
        /// Test result(lastet)
        /// </summary>
        public TestResult LastTestResult { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Testcase model</param>
        public TestcaseResponse(Testcase model)
        {
            Id = model.Id;
            ProjectId = model.ProjectId;
            TrackingId = model.TrackingId;
            BranchName = model.BranchName;
            IsCommited = model.IsCommited;
            CommitMode = model.CommitMode;
            Order = model.Order;
            SectionName = model.SectionName;
            MoreCareful = model.MoreCareful;
            Estimates = model.Estimates;
            Precondition = model.Precondition;
            Test = model.Test;
            Expect = model.Expect;
            LastUpdateDate = model.LastUpdateDate;
            Testruns = model.Testruns;

            var lastTestrun = model.Testruns.LastOrDefault();
            LastTestResult = lastTestrun?.Result ?? TestResult.NotTest;
        }
    }
}
