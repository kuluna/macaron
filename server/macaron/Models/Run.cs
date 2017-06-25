using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Run log
    /// </summary>
    public class Run
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
        /// <summary>
        /// Parent plan ID
        /// </summary>
        [Required]
        public int PlanId { get; set; }
        /// <summary>
        /// To record case ID
        /// </summary>
        [Required]
        public int CaseId { get; set; }
        /// <summary>
        /// Case revision
        /// </summary>
        [Required]
        public int CaseRevision { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        [Required]
        public TestResult Result { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        [Required, AppUserName]
        public string Username { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Last update date
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
