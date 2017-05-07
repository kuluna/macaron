using macaron.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        /// <param name="db">Database context</param>
        /// <param name="model">Will updated model</param>
        /// <param name="projectId">Project ID</param>
        public async Task UpdateAsync(DatabaseContext db, Testplan model, int projectId)
        {
            model.Name = Name;
            model.LeaderId = LeaderId;
            model.Testcases = await FindTestcaseAsync(db, projectId);
            model.DueDate = DueDate;
            model.Completed = Completed;
            model.LastUpdateDate = DateTimeOffset.UtcNow;
        }
    }
}
