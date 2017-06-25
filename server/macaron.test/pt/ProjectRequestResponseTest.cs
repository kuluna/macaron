using macaron.Models.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using macaron.Models;
using macaron.Models.Response;
using System;

namespace macaron.test.pt
{
    [TestClass]
    public class ProjectRequestResponseTest
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
            Assert.IsFalse(project.IsArcived);
            Assert.IsNotNull(project.CreatedDate);
            Assert.IsNotNull(project.LastUpdateDate);
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
                 IsArcived = true
            };
            req.Update(project);

            Assert.AreEqual(req.Name, project.Name);
            Assert.AreEqual(req.Description, project.Description);
            Assert.AreEqual(req.IsArcived, project.IsArcived);
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
            req.Update(project);

            Assert.AreEqual(req.Name, project.Name);
            Assert.AreEqual("before", project.Description);
            Assert.AreEqual(false, project.IsArcived);
        }

#endregion

        [TestMethod]
        public void ResponseProject()
        {
            var project = new Project()
            {
                Id = 1,
                Name = "res",
                Description = "desc",
                IsArcived = true,
                Cases = new List<Case>()
                {
                    new Case()
                    {
                        Id = 1,
                        ProjectId = 1,
                        Step = "testing",
                        Expectation = "all ok"
                    }
                },
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow.AddDays(1)
            };

            var res = new ProjectResponse(project);
            Assert.AreEqual(project.Id, res.Id);
            Assert.AreEqual(project.Name, res.Name);
            Assert.AreEqual(project.Description, res.Description);
            Assert.AreEqual(project.IsArcived, res.IsArcived);
            Assert.AreEqual(project.CreatedDate, res.CreatedDate);
            Assert.AreEqual(project.LastUpdateDate, res.LastUpdateDate);
        }

        [TestMethod]
        public void ResponseProjectDetail()
        {
            var project = new Project()
            {
                Id = 1,
                Name = "res",
                Description = "desc",
                IsArcived = true,
                Cases = new List<Case>()
                {
                    new Case()
                    {
                        Id = 1,
                        ProjectId = 1,
                        Step = "testing",
                        Expectation = "all ok"
                    }
                },
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow.AddDays(1)
            };

            var res = new ProjectDetailResponse(project, project.Cases);
            Assert.AreEqual(project.Cases, res.Cases);
        }

        private (bool, List<ValidationResult>) ValidateModelState<TModel>(TModel model, bool validateAllProperties = true)
        {
            var result = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(model, new ValidationContext(model, null, null), result, validateAllProperties);
            return (validationResult, result);
        }
    }
}
