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
            var testcases = new List<Case>()
            {
                new Case() { Id = 1, Revision = 0 },
                new Case() { Id = 2, Revision = 1 },
                new Case() { Id = 3, Revision = 2 }
            };

            var identities = new List<CaseIdentity>()
            {
                new CaseIdentity(2, 1)
            };

            var result = testcases.Where(t => identities.Contains(new CaseIdentity(t.Id, t.Revision)));

            Assert.AreEqual(1, result.Count());
        }
    }
}
