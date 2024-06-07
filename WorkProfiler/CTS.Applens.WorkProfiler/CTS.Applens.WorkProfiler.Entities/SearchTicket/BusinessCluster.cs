// <copyright file="BusinessCluster.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    using System.Collections.Generic;
    /// <summary>
    /// This class holds BusinessCluster details
    /// </summary>
    public class BusinessCluster
    {
        /// <summary>
        /// Gets or sets the Business Cluster Name.
        /// </summary>
        public string BusinessClusterName { get; set; }

        /// <summary>
        /// Gets or sets the Business Cluster ID.
        /// </summary>
        public int BusinessClusterId { get; set; }

        /// <summary>
        /// Gets or sets the Row Number.
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Gets or sets the InfraFlag.
        /// </summary>
        public bool IsInfra { get; set; }

        /// <summary>
        /// Gets or sets the Sub Business Cluster List.
        /// </summary>
        public List<SubBusinessCluster> SubBusinessClusterList { get; set; }
    }
}
