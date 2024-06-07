// <copyright file="SearchTicketParameters.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The SearchTicketParameters Class holds Search Ticket Parameters.
    /// </summary>
    public class SearchTicketParameters
    {
        /// <summary>
        /// Gets or sets the Customer IDs.
        /// </summary>
        public Int64 CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the CustomerName.
        /// </summary>
        /// 
        [MaxLength(500)]
        public string CustomerName { get; set; }
        /// <summary>
        /// Gets or sets the IsCognizant.
        /// </summary>
        public int IsCognizant{ get; set; }

        /// <summary>
        /// Gets or sets the Project IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ProjectIds { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        [DataType(DataType.Text)]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date.
        /// </summary>
        [DataType(DataType.Text)]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Is Filter By Open Date.
        /// </summary>
        public int IsFilterByOpenDate { get; set; }

        /// <summary>
        /// Gets or sets the Hierarchy 1 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy1Id { get; set; }

        /// <summary>
        /// Gets or sets the Hierarchy 2 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy2Id { get; set; }

        /// <summary>
        /// Gets or sets the Hierarchy 3 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy3Id { get; set; }
        /// <summary>
        /// Gets or sets the Hierarchy 4 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy4Id { get; set; }
        /// <summary>
        /// Gets or sets the Hierarchy 5 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy5Id { get; set; }
        /// <summary>
        /// Gets or sets the Hierarchy 6 IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Hierarchy6Id { get; set; }
        /// <summary>
        /// Gets or sets the Application IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ApplicationId { get; set; }
        /// <summary>
        /// Gets or sets the Ticket Status IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string TicketStatusId { get; set; }
        /// <summary>
        /// Gets or sets the Ticket Source IDs.
        /// </summary>
        [MaxLength(5)]
        public string TicketSourceId { get; set; }
        /// <summary>
        /// Gets or sets the Ticket Type IDs.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string TicketTypeId { get; set; }
        /// <summary>
        /// Gets or sets the Ticketing Data.
        /// </summary>
        [MaxLength(5)]
        public string TicketingData { get; set; }
        /// <summary>
        /// Gets or sets the Data Entry Completion.
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string DataEntryCompletion { get; set; }
        /// <summary>
        /// Gets or sets the Infra Enabled Flag.
        /// </summary>
        public bool InfraEnabled { get; set; }
        public int HeiraricyNum { get; set; }
    }
    /// <summary>
    /// Get Project List from search filter
    /// </summary>
    public class ProjectIDs
    {
        [Column(TypeName = "NVARCHAR")]
        [StringLength(4000)]
        public string ProjectId { get; set; }
    }

    public class SearchTicketTypeParameter
    {
        /// <summary>
        /// ProjectID
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string ProjectId { get; set; }

        /// <summary>
        /// IsCognizant
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string IsCognizant { get; set; }
    }
    public class HierarchyListModel
    {
        /// <summary>
        /// customerID
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string customerID { get; set; }

        /// <summary>
        /// associateID
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string associateID { get; set; }
    }
}
