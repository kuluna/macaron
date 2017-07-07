using macaron.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace macaron.test.pt
{
    [TestClass]
    public class AppUserTest
    {
        [TestMethod]
        public void AttributeTest()
        {
            var validator = new AppUserNameAttribute();

            // These are verify using different validators.
            Assert.IsTrue(validator.IsValid(null));
            Assert.IsTrue(validator.IsValid(""));
            // Should be validate.
            Assert.IsTrue(validator.IsValid("username"));
            Assert.IsTrue(validator.IsValid("123ABC"));
            Assert.IsTrue(validator.IsValid("ABC123def"));
            Assert.IsFalse(validator.IsValid(10.6));
            Assert.IsFalse(validator.IsValid("10.6"));
            Assert.IsFalse(validator.IsValid("全角文字"));
        }
    }
}
