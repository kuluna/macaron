using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(case update)
    /// </summary>
    public class CaseUpdateRequest: CaseCreateRequest
    {
        /// <summary>
        /// Order(ASC)
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Create model (update revision)
        /// </summary>
        /// <param name="revisions">Testcases</param>
        /// <param name="allocateId">Parent Testcase model</param>
        /// <returns>New testcase model</returns>
        public Case ToCase(IList<Case> revisions, int allocateId)
        {
            var model = ToTestcase();
            model.AllocateId = allocateId;
            model.Revision = revisions.Select(t => t.Revision).Max() + 1;
            model.Order = Order ?? model.Order;

            return model;
        }
    }
}
