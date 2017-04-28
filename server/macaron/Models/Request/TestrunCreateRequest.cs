using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(create testrun)
    /// </summary>
    public class TestrunCreateRequest
    {
        /// <summary>
        /// Target testcase ID
        /// </summary>
        [Required]
        public int TestcaseId { get; set; }
        /// <summary>
        /// Parent milestone ID
        /// </summary>
        public int? MilestoneId { get; set; }
        /// <summary>
        /// Test result
        /// </summary>
        [Required]
        public TestResult Result { get; set; }
        /// <summary>
        /// Test user ID
        /// </summary>
        [Required]
        public Guid TestUserId { get; set; }

        /// <summary>
        /// Convert the testrun model
        /// </summary>
        /// <returns></returns>
        public Testrun ToTestrun()
        {
            return new Testrun()
            {
                TestcaseId = TestcaseId,
                MilestoneId = MilestoneId,
                Result = Result,
                TestUserId = TestUserId,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
        
        /// <summary>
        /// Convert all testrun model
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static IEnumerable<Testrun> ToTestruns(IEnumerable<TestrunCreateRequest> requests)
        {
            return requests.Select(req => req.ToTestrun());
        }
    }
}
