// <copyright file="Application.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    /// <summary>
    /// The Application Class holds Application Details.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Gets or sets the ParentBusinessCluster ID.
        /// </summary>
        public string ParentBusinessClusterId { get; set; }
        /// <summary>
        /// Gets or sets the Project ID.
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets the Application ID.
        /// </summary>
        public string ApplicationId { get; set; } 

        /// <summary>
        /// Gets or sets the Application Name.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the Infra Flag.
        /// </summary>
        public bool IsInfra { get; set; }
    }
}
