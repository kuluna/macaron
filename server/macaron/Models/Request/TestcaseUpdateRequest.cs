using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Testcase update)
    /// </summary>
    public class TestcaseUpdateRequest: TestcaseCreateRequest
    {
        /// <summary>
        /// Commit mode
        /// </summary>
        [Required]
        public CommitMode CommitMode { get; set; }
        /// <summary>
        /// Order(ASC)
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// Update the model
        /// </summary>
        /// <param name="model">Model</param>
        public void Update(Testcase model)
        {
            
        }
    }
}
