using macaron.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(create testplan)
    /// </summary>
    public class TestplanCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Target testcases
        /// </summary>
        public IList<int> TestcaseIds { get; set; }
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
        /// Convert to testplan model
        /// </summary>
        /// <returns></returns>
        public async Task<Testplan> ToTestplanAsync(DatabaseContext db)
        {
            var testcases = await db.Testcases.Where(t => TestcaseIds.Contains(t.Id)).ToListAsync();

            return new Testplan()
            {
                Name = Name,
                LeaderId = LeaderId,
                Testcases = testcases,
                DueDate = DueDate,
                IsCompleted = false,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
