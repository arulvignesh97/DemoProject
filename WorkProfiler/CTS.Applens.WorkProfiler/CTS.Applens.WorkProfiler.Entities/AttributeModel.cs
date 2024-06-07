using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds AttributeModel
    /// </summary>
    public class AttributeModel
    {
        /// <summary>
        /// Gets or sets projectId
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ServiceId
        /// </summary>
        public Int64 ServiceId { get; set; }       
        /// <summary>
        /// Gets or sets AttributeName
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// Gets or sets StatusName
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Gets or sets ProjectStatusName
        /// </summary>
        public string ProjectStatusName { get; set; }       
        /// <summary>
        /// Gets or sets ValueName
        /// </summary>
        public string ValueName { get; set; }
        /// <summary>
        /// Gets or sets SDValueName
        /// </summary>
        public string SDValueName { get; set; }
        /// <summary>
        /// Gets or sets AttributeType
        /// </summary>
        public string AttributeType { get; set; }
        /// <summary>
        /// Gets or sets TicketMasterFields
        /// </summary>
        public string TicketMasterFields { get; set; }
        private string isMandatory;
        /// <summary>
        /// Gets or sets IsMandatory
        /// </summary>
        public string IsMandatory
        {
            get
            {
                return isMandatory;
            }
            set
            {
                isMandatory = value;
            }
        }
    }
       
}
