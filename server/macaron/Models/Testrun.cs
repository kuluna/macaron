using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Run test log
    /// </summary>
    public class Testrun
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent testplain ID
        /// </summary>
        [Required]
        public int TestplanId { get; set; }
        /// <summary>
        /// To record test ID
        /// </summary>
        [Required]
        public int TestcaseId { get; set; }
        /// <summary>
        /// Testcase revision
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
        public Guid TestUserId { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }
    }

    /// <summary>
    /// Test status
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TestResult
    {
        /// <summary>
        /// Will tested or skip
        /// </summary>
        NotTest,
        /// <summary>
        /// OK (passed)
        /// </summary>
        Ok,
        /// <summary>
        /// NG (failed)
        /// </summary>
        Ng
    }
}
