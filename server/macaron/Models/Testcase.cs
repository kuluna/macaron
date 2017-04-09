using System.ComponentModel.DataAnnotations;

namespace macaron.Models
{
    /// <summary>
    /// Testcase
    /// </summary>
    public class Testcase
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Parent milestone ID
        /// </summary>
        public int MilestoneId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
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
        /// Copy the new testcase object(empty id)
        /// </summary>
        /// <returns>New testcase object</returns>
        public Testcase Copy()
        {
            return new Testcase()
            {
                Name = Name,
                MoreCareful = MoreCareful,
                Estimates = Estimates,
                Precondition = Precondition,
                Test = Test,
                Expect = Expect
            };
        }
    }
}
