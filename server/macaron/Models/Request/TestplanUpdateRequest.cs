using macaron.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(update testplan model)
    /// </summary>
    public class TestplanUpdateRequest : TestplanCreateRequest
    {
        /// <summary>
        /// Mark the complete this plan
        /// </summary>
        [Required]
        public bool Completed { get; set; }

        /// <summary>
        /// Update to testplan model
        /// </summary>
        /// <param name="model">Will updated model</param>
        /// <param name="project">Project</param>
        public void Update(Testplan model, Project project)
        {
            model.Name = Name;
            model.LeaderName = LeaderName;
            model.Testcases = FindTestcase(project).ToList();
            model.DueDate = DueDate;
            model.Completed = Completed;
            model.LastUpdateDate = DateTimeOffset.UtcNow;
        }
    }
}
