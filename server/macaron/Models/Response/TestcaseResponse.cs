﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Response
{
    /// <summary>
    /// Response body(Testcase)
    /// </summary>
    public class TestcaseResponse
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
        /// Branch name (default: master)
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Commit mode
        /// </summary>
        public CommitMode CommitMode { get; set; }
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
        public bool MoreCareful { get; set; }
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
        public string Test { get; set; }
        /// <summary>
        /// Expect test result (markdown format)
        /// </summary>
        public string Expect { get; set; }
        /// <summary>
        /// Test results
        /// </summary>
        public IList<Testrun> TestResults { get; set; }
        /// <summary>
        /// Last test result
        /// </summary>
        public TestResult LastTestResult { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        public DateTimeOffset LastUpdateDate { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Testcase model</param>
        public TestcaseResponse(Testcase model)
        {
            Id = (int) model.AllocateId;
            ProjectId = model.ProjectId;
            Revision = (int) model.Revision;
            BranchName = model.BranchName;
            CommitMode = model.CommitMode;
            Order = model.Order;
            SectionName = model.SectionName;
            MoreCareful = model.MoreCareful;
            Estimates = model.Estimates;
            Precondition = model.Precondition;
            Test = model.Test;
            Expect = model.Expect;
            TestResults = new List<Testrun>();
            LastTestResult = TestResult.NotTest;
            LastUpdateDate = model.LastUpdateDate;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Testcase model</param>
        /// <param name="testResults"></param>
        public TestcaseResponse(Testcase model, IEnumerable<Testrun> testResults) : this(model)
        {
            TestResults = TestResults.Where(t => t.TestcaseId == model.AllocateId && t.Revision == model.Revision)
                                     .OrderByDescending(t => t.LastUpdateDate)
                                     .ToList();
            LastTestResult = TestResults.FirstOrDefault()?.Result ?? TestResult.NotTest;
        }
    }
}
