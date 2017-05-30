using macaron.Models.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace macaron.test.pt
{
    [TestClass]
    public class TestcaseRequestResponseTest
    {
        [TestMethod]
        public void TestcaseCreateIfNameEmpty()
        {
            var model = new TestcaseCreateRequest()
            {
                SectionName = "",
                BranchName = "",
                MoreCareful = false,
                Estimates = 0,
                Precondition = "",
                Test = "OKOK",
                Expect = "All Ok"
            }.ToTestcase();

            Assert.AreEqual("Test", model.SectionName);
            Assert.AreEqual("master", model.BranchName);
        }

        [TestMethod]
        public void TestcaseCreate()
        {
            var model = new TestcaseCreateRequest()
            {
                SectionName = "TestName",
                BranchName = "Testbranch",
                MoreCareful = false,
                Estimates = 0,
                Precondition = "ABC",
                Test = "OKOK",
                Expect = "All Ok"
            }.ToTestcase();

            Assert.AreEqual("TestName", model.SectionName);
            Assert.AreEqual("Testbranch", model.BranchName);
        }
    }
}
