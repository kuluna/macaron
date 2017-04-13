using System;

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
            Arcived = project.Arcived;
            CreatedDate = project.CreatedDate;
            LastUpdateDate = project.LastUpdateDate;
        }
    }
}
