using Newtonsoft.Json;
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
        /// Revision
        /// </summary>
        [Required]
        public int Revision { get; set; }
        /// <summary>
        /// Test result
        /// </summary>
        [Required]
        public TestResult Result { get; set; }
        /// <summary>
        /// Test user ID
        /// </summary>
        [Required]
        public string TestUsername { get; set; }

        /// <summary>
        /// Convert the testrun model
        /// </summary>
        /// <returns></returns>
        public Testrun ToTestrun(int testplanId)
        {
            return new Testrun()
            {
                TestplanId = testplanId,
                TestcaseId = TestcaseId,
                Revision = Revision,
                Result = Result,
                TestUsername = TestUsername,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
        
        /// <summary>
        /// Convert all testrun model
        /// </summary>
        /// <param name="testplanId">Test plan ID</param>
        /// <param name="requests"></param>
        /// <returns></returns>
        public static IEnumerable<Testrun> ToTestruns(int testplanId, IEnumerable<TestrunCreateRequest> requests)
        {
            return requests.Select(req => req.ToTestrun(testplanId));
        }
    }
}
