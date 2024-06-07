using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds Roles details
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Gets or sets the Role ID.
        /// </summary>
        public Int32 RoleId { get; set; }

        /// <summary>
        /// Gets or sets the Role Name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the Priority.
        /// </summary>
        public Int32 Priority { get; set; }
    }
}
