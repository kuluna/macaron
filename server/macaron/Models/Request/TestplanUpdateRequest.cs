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
    public class TestplanUpdateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Target testcases
        /// </summary>
        public IList<TestcaseIdentity> TestcaseIds { get; set; }
        /// <summary>
        /// Plan leader
        /// </summary>
        [Required]
        public Guid LeaderId { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }
        /// <summary>
        /// Mark the complete this plan
        /// </summary>
        [Required]
        public bool Completed { get; set; }

        /// <summary>
        /// Convert to testplan model
        /// </summary>
        /// <param name="model">Test plan</param>
        /// <param name="db">Database Context</param>
        public async Task UpdateAsync(Testplan model, DatabaseContext db)
        {
            var testcases = await db.Testcases.Where(t =>
                TestcaseIds.Contains(new TestcaseIdentity() { TestcaseId = t.Id, Revision = (int) t.Revision }))
                .ToListAsync();

            model.Name = Name;
            model.LeaderId = LeaderId;
            model.Testcases = testcases;
            model.DueDate = DueDate;
            model.Completed = Completed;
            model.LastUpdateDate = DateTimeOffset.UtcNow;
        }
    }
}
