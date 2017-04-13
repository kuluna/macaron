using macaron.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Testcase create)
    /// </summary>
    public class TestcaseCreateRequest
    {
        /// <summary>
        /// Want you to test carefully
        /// </summary>
        [Required]
        public bool MoreCareful { get; set; }
        /// <summary>
        /// Estimates
        /// </summary>
        [Required]
        public double Estimates { get; set; }
        /// <summary>
        /// Test precondition
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// Test step
        /// </summary>
        [Required, MinLength(1)]
        public string Test { get; set; }
        /// <summary>
        /// Expect test result
        /// </summary>
        [Required, MinLength(1)]
        public string Expect { get; set; }

        /// <summary>
        /// Convert to model
        /// </summary>
        /// <returns>Test case</returns>
        public Testcase ToTestcase()
        {
            return new Testcase()
            {
                MoreCareful = MoreCareful,
                Estimates = Estimates,
                Precondition = Precondition,
                Test = Test,
                Expect = Expect,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
