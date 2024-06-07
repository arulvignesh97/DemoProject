using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models.Work_Items
{
    public class MandateAttributes
    {
        /// <summary>
        /// Gets or Sets AttributeId
        /// </summary>
        public Int16 AttributeId { get; set; }
        /// <summary>
        /// Gets or Sets AttributeName
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// Gets or Sets ExecutionMethodId
        /// </summary>
        public Int64 ExecutionMethodId { get; set; }
        /// <summary>
        /// Gets or Sets ExecutionMethodName
        /// </summary>
        public string ExecutionMethodName { get; set; }
        /// <summary>
        /// Gets or Sets MandateId
        /// </summary>
        public Int16 MandateId { get; set; }
        /// <summary>
        /// Gets or Sets MandateName
        /// </summary>
        public string MandateName { get; set; }

    }
}
