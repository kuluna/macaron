using macaron.Models;
using macaron.Models.Request;
using macaron.Models.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace macaron.test.pt
{
    [TestClass]
    public class CaseRequestResponseTest
    {
        [TestMethod]
        public void CaseCreateIfNameEmpty()
        {
            var model = new CaseCreateRequest()
            {
                SectionName = "",
                IsCarefully = false,
                Estimates = 0,
                Precondition = "",
                Step = "OKOK",
                Expectation = "All Ok"
            }.ToTestcase();

            Assert.AreEqual("Test", model.SectionName);
        }

        [TestMethod]
        public void CaseCreate()
        {
            var model = new CaseCreateRequest()
            {
                SectionName = "TestName",
                IsCarefully = false,
                Estimates = 0,
                Precondition = "ABC",
                Step = "OKOK",
                Expectation = "All Ok"
            }.ToTestcase();

            Assert.AreEqual("TestName", model.SectionName);
        }

        [TestMethod]
        public void ResponseGroupedCaseIfNotRuns()
        {
            var cases = new List<Case>()
            {
                new Case()
                {
                    AllocateId = 1,
                    Revision = 0,
                    SectionName = "Test",
                    Step = "First",
                    IsOutdated = false
                },
                new Case()
                {
                    AllocateId = 2,
                    Revision = 1,
                    SectionName = "Test",
                    Step = "Second",
                    IsOutdated = false
                }
            };


            var res = GroupedCaseResponse.ToGroupedCaseResponse(cases.Select(c => new CaseResponse(c)))
                                         .ToList();

            Assert.AreEqual(1, res.Count);
            var first = res.First();
            Assert.AreEqual(0, first.OkCount);
            Assert.AreEqual(0, first.NgCount);
            Assert.AreEqual(2, first.NotTestCount);
        }

        [TestMethod]
        public void ResponseGroupedCaseIncludeRuns()
        {
            var cases = new List<Case>()
            {
                new Case()
                {
                    AllocateId = 1,
                    Revision = 0,
                    SectionName = "Test",
                    Step = "First",
                    IsOutdated = false
                },
                new Case()
                {
                    AllocateId = 2,
                    Revision = 1,
                    SectionName = "HOGE",
                    Step = "Second",
                    IsOutdated = false
                }
            };

            var runs = new List<Run>()
            {
                new Run()
                {
                    Id = 1,
                    CaseId = 1,
                    CaseRevision = 0,
                    Result = Models.TestResult.Ok,
                },
                new Run() // dummy
                {
                    Id = 2,
                    CaseId = 2,
                    CaseRevision = 0,
                    Result = Models.TestResult.Ok
                },
                new Run()
                {
                    Id = 4,
                    CaseId = 2,
                    CaseRevision = 1,
                    Result = Models.TestResult.Ok,
                    LastUpdateDate = DateTime.UtcNow
                },
                new Run()
                {
                    Id = 3,
                    CaseId = 2,
                    CaseRevision = 1,
                    Result = Models.TestResult.Ng,
                    LastUpdateDate = DateTime.UtcNow.AddDays(-1)
                }
            };

            var res = GroupedCaseResponse.ToGroupedCaseResponse(cases.Select(c => new CaseResponse(c, runs)))
                                         .ToList();

            var first = res[0];
            Assert.AreEqual(1, first.OkCount);
            Assert.AreEqual(0, first.NgCount);
            Assert.AreEqual(0, first.NotTestCount);

            var second = res[1];
            Assert.AreEqual(1, second.OkCount);
            Assert.AreEqual(0, second.NgCount);
            Assert.AreEqual(0, second.NotTestCount);
        }
    }
}
