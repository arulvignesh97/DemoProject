using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds RootObject Object details
    /// </summary>
    public class RootObject
    {
        /// <summary>
        /// Gets or Sets Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets LanguageTo
        /// </summary>
        public string LanguageTo { get; set; }
        /// <summary>
        /// Gets or Sets ConcatenateStrings
        /// </summary>
        public List<ConcatenateStrings> ConcatenateStrings { get; set; }
        /// <summary>
        /// Gets or Sets Languages
        /// </summary>
        public List<string> Languages { get; set; }
    }

    /// <summary>
    /// ServiceRootObject
    /// </summary>
    public class ServiceRootObject
    {
        /// <summary>
        /// Gets or Length of Text
        /// </summary>
        public string LengthText { get; set; }

        /// <summary>
        /// Gets or Sets SubcriptionKey
        /// </summary>
        public string SubcriptionKey { get; set; }

    }
}