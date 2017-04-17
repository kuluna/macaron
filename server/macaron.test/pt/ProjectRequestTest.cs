using macaron.Models.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace macaron.test.pt
{
    [TestClass]
    public class ProjectRequestTest
    {

#region ProjectCreateRequest

        [TestMethod]
        public void ValidProjectCreate()
        {
            var req = new ProjectCreateRequest()
            {
                Name = "test",
                Description = "desc"
            };

            var (valid, _) = ValidateModelState(req);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ValidProjectCreateIncludedNull()
        {
            var req = new ProjectCreateRequest()
            {
                Name = "test"
            };

            var (valid, _) = ValidateModelState(req);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void InvalidProjectCreate()
        {
            var req = new ProjectCreateRequest();

            var (valid, result) = ValidateModelState(req);

            Assert.IsFalse(valid);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ConvertToProject()
        {
            var req = new ProjectCreateRequest()
            {
                Name = "test",
                Description = "desc"
            };
            var project = req.ToProject();

            Assert.AreEqual(req.Name, project.Name);
            Assert.AreEqual(req.Description, req.Description);
            Assert.IsFalse(project.Arcived);
            Assert.IsNotNull(project.CreatedDate);
            Assert.IsNotNull(project.LastUpdateDate);

            var milestone = project.Milestones.First();
            Assert.AreEqual(project.Name, milestone.Name);
            Assert.AreEqual(0, milestone.Progress);
            Assert.IsNull(milestone.ExpectedCompleteDate);
            Assert.IsNull(milestone.CompleteDate);
            Assert.AreEqual(0, milestone.Testcases.Count);

            var platform = milestone.Platforms.First();
            Assert.AreEqual("default", platform.Name);
        }

#endregion

#region ProjectUpdate

        [TestMethod]
        public void ValidProjectUpdate()
        {
            var req = new ProjectUpdateRequest()
            {
                Name = "updated",
                Description = null
            };

            var (valid, _) = ValidateModelState(req);

            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void InvalidProjectUpdate()
        {
            var req = new ProjectUpdateRequest()
            {
                Name = ""
            };
            var (valid, result) = ValidateModelState(req);

            Assert.IsFalse(valid);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void UpdateProject()
        {
            var project = new ProjectCreateRequest()
            {
                Name = "origin",
                Description = "before"
            }
            .ToProject();

            var req = new ProjectUpdateRequest()
            {
                 Name = "updated",
                 Description = "after",
                 Arcived = true
            };
            req.Update(ref project);

            Assert.AreEqual(req.Name, project.Name);
            Assert.AreEqual(req.Description, project.Description);
            Assert.AreEqual(req.Arcived, project.Arcived);
        }

        [TestMethod]
        public void UpdateProjectIncludedNull()
        {
            var project = new ProjectCreateRequest()
            {
                Name = "origin",
                Description = "before"
            }
            .ToProject();

            var req = new ProjectUpdateRequest()
            {
                Name = "updated"
            };
            req.Update(ref project);

            Assert.AreEqual(req.Name, project.Name);
            Assert.AreEqual("before", project.Description);
            Assert.AreEqual(false, project.Arcived);
        }

#endregion

        private (bool, List<ValidationResult>) ValidateModelState<TModel>(TModel model, bool validateAllProperties = true)
        {
            var result = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(model, new ValidationContext(model, null, null), result, validateAllProperties);
            return (validationResult, result);
        }
    }
}
