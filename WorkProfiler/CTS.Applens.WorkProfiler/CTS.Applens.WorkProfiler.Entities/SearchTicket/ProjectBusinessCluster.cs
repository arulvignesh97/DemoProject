// <copyright file="ProjectBusinessCluster.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    using System.Collections.Generic;

    /// <summary>
    /// The ProjectBusinessCluster Class holds Projectwise Business Cluster Details
    /// </summary>
    public class ProjectBusinessCluster
    {
        /// <summary>
        /// Gets or sets the list of Projects.
        /// </summary>
        public List<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the list of Business Clusters.
        /// </summary>
        public List<BusinessCluster> BusinessCluster { get; set; }
    }
}
