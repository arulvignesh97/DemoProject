// <copyright file="SubBusinessCluster.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    using System.Collections.Generic;

    /// <summary>
    /// The SubBusinessCluster Class holds Sub Business Cluster Details.
    /// </summary>
    public class SubBusinessCluster
    {
        /// <summary>
        /// Gets or sets the Business Cluster Base Name.
        /// </summary>
        public string BusinessClusterBaseName { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster Map ID.
        /// </summary>
        public string BusinessClusterMapId { get; set; }

        /// <summary>
        /// Gets or sets the Row Number.
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Gets or sets the Parent Business Cluster Map ID.
        /// </summary>
        public string ParentBusinessClusterMapId { get; set; }

        /// <summary>
        /// Gets or sets the Sub Cluster ID.
        /// </summary>
        public string SubClusterId { get; set; }

        /// <summary>
        /// Gets or sets the Project Id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the list of Applications.
        /// </summary>
        public List<Application> ApplicationList { get; set; }

        /// <summary>
        /// Gets or sets the Infra Flag.
        /// </summary>
        public bool IsInfra { get; set; }
    }
}
