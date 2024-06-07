using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
  public class EmailDetail
    {
        /// <summary>
        /// Gets or sets FromAddress
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets ToAddress
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets CC
        /// </summary>

        public string CC { get; set; }

        /// <summary>
        /// Gets or sets Subject
        /// </summary>

        public string Subject { get; set; }


        /// <summary>
        /// Gets or sets Body
        /// </summary>
        public string Body { get; set; }

    }
}
