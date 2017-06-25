using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(create run)
    /// </summary>
    public class RunCreateRequest
    {
        /// <summary>
        /// Target case ID
        /// </summary>
        [Required]
        public int CaseId { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        [Required]
        public int CaseRevision { get; set; }
        /// <summary>
        /// Test result
        /// </summary>
        [Required]
        public TestResult Result { get; set; }
        /// <summary>
        /// User ID
        /// </summary>
        [Required, AppUserName]
        public string Username { get; set; }

        /// <summary>
        /// Convert the run model
        /// </summary>
        /// <returns>Run model</returns>
        public Run ToRun(int planId)
        {
            return new Run()
            {
                PlanId = planId,
                CaseId = CaseId,
                CaseRevision = CaseRevision,
                Result = Result,
                Username = Username,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
        
        /// <summary>
        /// Convert all run model
        /// </summary>
        /// <param name="planId">Plan ID</param>
        /// <param name="requests">Requests</param>
        /// <returns>Run models</returns>
        public static IEnumerable<Run> ToRuns(int planId, IEnumerable<RunCreateRequest> requests)
        {
            return requests.Select(req => req.ToRun(planId));
        }
    }
}
