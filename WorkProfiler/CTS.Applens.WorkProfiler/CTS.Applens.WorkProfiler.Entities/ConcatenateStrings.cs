using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds Translated Object details
    /// </summary>
    public class ConcatenateStrings
    {

        /// <summary>
        /// Gets or Sets OriginalColumn
        /// </summary>
        public string OriginalColumn { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedColumn
        /// </summary>
        public string TranslatedColumn { get; set; }
        /// <summary>
        /// Gets or Sets Text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedText
        /// </summary>
        public string TranslatedText { get; set; }
        /// <summary>
        /// Gets or Sets TextLength
        /// </summary>
        public int TextLength { get; set; }
        /// <summary>
        /// Gets or Sets IsTranslated
        /// </summary>
        public bool IsTranslated { get; set; }
        /// <summary>
        /// Gets or Sets TimeTickerID
        /// </summary>
        public long TimeTickerId { get; set; }
        /// <summary>
        /// Gets or Sets HasError
        /// </summary>
        public bool HasError { get; set; }
        /// <summary>
        /// Gets or Sets ErrorCol
        /// </summary>
        public string ErrorCol { get; set; }

        /// <summary>
        /// Gets or Sets Languages
        /// </summary>
        public List<string> Languages { get; set; }
        /// Gets or Sets DelimiterText
        /// </summary>
        public List<string> DelimiterText { get; set; }
        /// Gets or Sets SupportType
        /// </summary>
        public int SupportType { get; set; }
    }
}