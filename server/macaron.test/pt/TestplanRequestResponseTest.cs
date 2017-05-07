using macaron.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace macaron.test.pt
{
    [TestClass]
    public class TestplanRequestResponseTest
    {
        [TestMethod]
        public void TestcaseIdentityEquals()
        {
            var testcases = new List<Testcase>()
            {
                new Testcase() { Id = 1, Revision = 0 },
                new Testcase() { Id = 2, Revision = 1 },
                new Testcase() { Id = 3, Revision = 2 }
            };

            var identities = new List<TestcaseIdentity>()
            {
                new TestcaseIdentity() { TestcaseId = 2, Revision = 1 }
            };

            var result = testcases.Where(t => identities.Contains(new TestcaseIdentity() { TestcaseId = t.Id, Revision = (int) t.Revision }));

            Assert.AreEqual(1, result.Count());
        }
    }
}
