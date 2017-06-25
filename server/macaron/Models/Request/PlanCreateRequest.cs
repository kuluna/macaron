using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(plan create)
    /// </summary>
    public class PlanCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required, MinLength(1)]
        public string Name { get; set; }
        /// <summary>
        /// Test pattern
        /// </summary>
        [Required]
        public PlanPattern Pattern { get; set; }
        /// <summary>
        /// Target branch name
        /// </summary>
        public string[] Sections { get; set; }
        /// <summary>
        /// Custom testcases
        /// </summary>
        public IList<CaseIdentity> CaseIds { get; set; }
        /// <summary>
        /// Plan leader
        /// </summary>
        [Required, AppUserName]
        public string LeaderName { get; set; }
        /// <summary>
        /// Due date
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Find cases for each patterns
        /// </summary>
        /// <param name="project">Project model</param>
        /// <returns>Cases</returns>
        /// <exception cref="NotImplementedException">This method is not enough other Pattern's case.</exception>
        protected IEnumerable<Case> FindCase(Project project)
        {
            switch (Pattern)
            {
                case PlanPattern.Section:
                    if (Sections == null || Sections.Count() == 0)
                    {
                        throw new ArgumentException("Set sections when select Section pattern.");
                    }
                    return project.Cases.Where(c => Sections.Contains(c.SectionName) && !c.IsOutdated);

                case PlanPattern.Ng:
                    var failIds = project.Plans.SelectMany(
                        p => p.Runs.Where(r => r.Result == TestResult.Ng).Select(r => new CaseIdentity(r.CaseId, r.CaseRevision)));
                    return project.Cases.Where(c => failIds.Contains(new CaseIdentity(c.AllocateId, c.Revision)));

                case PlanPattern.Custom:
                    if (CaseIds == null || CaseIds.Count() == 0)
                    {
                        throw new ArgumentException("Set TestcaseIds when select Custom pattern.");
                    }
                    return project.Cases.Where(t => CaseIds.Contains(new CaseIdentity(t.Id, t.Revision)));

                default:
                    throw new NotImplementedException("This method is not enough other TestPattern's case.");
            }
        }

        /// <summary>
        /// Convert to plan model
        /// </summary>
        /// <param name="project">Project model</param>
        /// <returns>Plan model</returns>
        public Plan ToPlan(Project project)
        {
            return new Plan()
            {
                Name = Name,
                LeaderName = LeaderName,
                Cases = FindCase(project).ToList(),
                DueDate = DueDate,
                Completed = false,
                Runs = new List<Run>(),
                CreatedDate = DateTimeOffset.UtcNow,
                LastUpdateDate = DateTimeOffset.UtcNow
            };
        }
    }

    /// <summary>
    /// Pattern
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlanPattern
    {
        /// <summary>
        /// All cases in sections
        /// </summary>
        Section,
        /// <summary>
        /// All NG cases
        /// </summary>
        Ng,
        /// <summary>
        /// User choice
        /// </summary>
        Custom
    }
}
