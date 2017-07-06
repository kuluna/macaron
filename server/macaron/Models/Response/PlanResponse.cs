using macaron.Models;
using macaron.Models.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Macaron.Models.Response
{
    /// <summary>
    /// Response body(plan)
    /// </summary>
    public class PlanResponse: BasePlanResponse
    {
        /// <summary>
        /// Target cases
        /// </summary>
        public IList<CaseResponse> Cases { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Plan model</param>
        /// <param name="users">Users</param>
        public PlanResponse(Plan model, IEnumerable<AppUser> users): base(model, users)
        {
            Cases = model.Cases.Where(c => !c.IsOutdated)
                               .Select(c => new CaseResponse(c, model.Runs))
                               .ToList();
        }
    }

    /// <summary>
    /// Response body(grouped plan)
    /// </summary>
    public class GroupedPlanResponse : BasePlanResponse
    {
        /// <summary>
        /// Target cases
        /// </summary>
        public IList<GroupedCaseResponse> Cases { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Plan model</param>
        /// <param name="users">Users</param>
        public GroupedPlanResponse(Plan model, IEnumerable<AppUser> users): base(model, users)
        {
            var cases = model.Cases.Where(c => !c.IsOutdated)
                             .Select(c => new CaseResponse(c, model.Runs));
            Cases = GroupedCaseResponse.ToGroupedCaseResponse(cases).ToList();
        }
    }

    /// <summary>
    /// Base model
    /// </summary>
    public abstract class BasePlanResponse
    {        /// <summary>
             /// ID
             /// </summary>
        public int Id { get; }
        /// <summary>
        /// Parent project ID
        /// </summary>
        public int ProjectId { get; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Leader
        /// </summary>
        public AppUser Leader { get; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; }
        /// <summary>
        /// Testplan completed
        /// </summary>
        public bool Completed { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        public DateTimeOffset LastUpdateDate { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Plan model</param>
        /// <param name="users">Users</param>
        public BasePlanResponse(Plan model, IEnumerable<AppUser> users)
        {
            Id = model.Id;
            ProjectId = model.ProjectId;
            Name = model.Name;
            Leader = users.Where(u => u.UserName.Equals(model.LeaderName)).FirstOrDefault();
            DueDate = model.DueDate;
            Completed = model.Completed;
            CreatedDate = model.CreatedDate;
            LastUpdateDate = model.LastUpdateDate;
        }

    }
}
