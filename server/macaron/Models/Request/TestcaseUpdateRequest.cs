using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Testcase update)
    /// </summary>
    public class TestcaseUpdateRequest: TestcaseCreateRequest
    {
        /// <summary>
        /// Target testcase revision
        /// </summary>
        [Required]
        public int TargetRevision { get; set; }
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
        /// Create model (update revision)
        /// </summary>
        /// <param name="revisions">Testcases</param>
        /// <param name="parent">Parent Testcase model</param>
        /// <returns>New testcase model</returns>
        public Testcase ToTestcase(IList<Testcase> revisions, Testcase parent)
        {
            var model = ToTestcase();
            model.AllocateId = parent.AllocateId;
            model.Revision = revisions.Select(t => t.Revision).Max() + 1;
            model.CommitMode = CommitMode;
            model.Order = Order;

            return model;
        }
    }
}
