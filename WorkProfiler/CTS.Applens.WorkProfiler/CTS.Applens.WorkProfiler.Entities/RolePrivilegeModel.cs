using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds RolePrivilegeModel details
    /// </summary>
    public class RolePrivilegeModel
    {
        /// <summary>
        /// Gets or sets PrivilegeID
        /// </summary>
        public int PrivilegeId { get; set; }
        /// <summary>
        /// Gets or sets MenuName
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// Gets or sets MenuRole
        /// </summary>
        public string MenuRole { get; set; }

    }
}
