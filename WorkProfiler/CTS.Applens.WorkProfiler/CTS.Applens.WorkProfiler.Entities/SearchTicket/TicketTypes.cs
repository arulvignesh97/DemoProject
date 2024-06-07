// <copyright file="TicketTypes.cs" company="CTS">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CTS.Applens.WorkProfiler.Models.SearchTicket
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Class TicketTypes holds Ticket Type Details.
    /// </summary>
    public class TicketTypes
    {
        /// <summary>
        /// Gets or sets the Ticket Type Mapping ID.
        /// </summary>
        public Int32? TicketTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Ticket Type.
        /// </summary>
        [MaxLength(100)]
        public string TicketTypeName { get; set; }

        /// <summary>
        /// Gets or sets the Support Type.
        /// </summary>
        public Int32 SupportTypeId { get; set; }
    }
}
