using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities
{
    /// <summary>
    /// Detail Common Class
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public Int64 ID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// ESAProject ID
        /// </summary>
        [StringLength(50)] public string ESAProjectID { get; set; }

        /// <summary>
        /// Gets or Sets Regenerate
        /// </summary>
        public bool IsRegenerate { get; set; }
    }
}
