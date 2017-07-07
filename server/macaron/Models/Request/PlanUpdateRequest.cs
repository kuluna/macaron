using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(update testplan model)
    /// </summary>
    public class PlanUpdateRequest : PlanCreateRequest
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
        public void Update(Plan model, Project project)
        {
            if (!Completed)
            {
                model.Name = Name;
                model.LeaderName = LeaderName;
                model.Cases = FindCase(project).ToList();
                model.DueDate = DueDate;
            }
            
            model.Completed = Completed;
            model.LastUpdateDate = DateTimeOffset.UtcNow;
        }
    }
}
