using System;
using System.Collections.Generic;

namespace macaron.Models.Response
{
    /// <summary>
    /// Response body (Project)
    /// </summary>
    public class ProjectResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Arcive
        /// </summary>
        public bool Arcived { get; }
        /// <summary>
        /// Created datetime
        /// </summary>
        public DateTimeOffset CreatedDate { get; }
        /// <summary>
        /// Last update datetime
        /// </summary>
        public DateTimeOffset LastUpdateDate { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">Project model</param>
        public ProjectResponse(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            Arcived = project.Arcived;
            CreatedDate = project.CreatedDate;
            LastUpdateDate = project.LastUpdateDate;
        }
    }

    /// <summary>
    /// Response body(project include milestones)
    /// </summary>
    public class ProjectDetailResponse : ProjectResponse
    {
        /// <summary>
        /// Milestones
        /// </summary>
        public ICollection<Milestone> Milestones { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="project">Project model</param>
        /// <param name="milestones">Milestones model</param>
        public ProjectDetailResponse(Project project, ICollection<Milestone> milestones) : base(project)
        {
            Milestones = milestones;
        }
    }
}
