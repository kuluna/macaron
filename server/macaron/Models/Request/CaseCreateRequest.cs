using System;
using System.ComponentModel.DataAnnotations;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(case create)
    /// </summary>
    public class CaseCreateRequest
    {
        /// <summary>
        /// Section
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// Want to more carefully
        /// </summary>
        [Required]
        public bool IsCarefully { get; set; }
        /// <summary>
        /// Estimates (min)
        /// </summary>
        [Required, Range(0, double.MaxValue)]
        public double Estimates { get; set; }
        /// <summary>
        /// precondition (markdown)
        /// </summary>
        public string Precondition { get; set; }
        /// <summary>
        /// step (markdown)
        /// </summary>
        [Required, MinLength(1)]
        public string Step { get; set; }
        /// <summary>
        /// Expect result (markdown)
        /// </summary>
        public string Expectation { get; set; }

        /// <summary>
        /// Convert to model
        /// </summary>
        /// <returns>Test case</returns>
        public Case ToTestcase()
        {
            return new Case()
            {
                AllocateId = int.MaxValue, // Temporary
                Revision = 0,
                SectionName = string.IsNullOrWhiteSpace(SectionName) ? "Test" : SectionName,
                Order = 0,
                IsCarefully = IsCarefully,
                Estimates = Estimates,
                Precondition = Precondition,
                Step = Step,
                Expectation = Expectation,
                IsOutdated = false,
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }
}
