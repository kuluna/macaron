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
        public void TestcaseCreate()
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
    }
}
