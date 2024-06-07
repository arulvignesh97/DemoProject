using System.Collections.Generic;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds RootObjectTranslations details 
    /// </summary>
    public class RootObjectTranslations
    {
        /// <summary>
        /// Gets or Sets Translations
        /// </summary>
        public List<Translations> Translations { get; set; }
    }
    /// <summary>
    /// This class holds BreakSentenceLength details
    /// </summary>
    public class BreakSentenceLength
    {
        /// <summary>
        /// Gets or Sets DetectedLanguage
        /// </summary>
        public BreakSentenceLanguage DetectedLanguage { get; set; }
        /// <summary>
        /// Gets or Sets SentLen
        /// </summary>
        public int[] SentLen { get; set; }
    }
    /// <summary>
    /// This class holds BreakSentenceLanguage details
    /// </summary>
    public class BreakSentenceLanguage
    {
        /// <summary>
        /// Gets or Sets Language
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Gets or Sets Score
        /// </summary>
        public string Score { get; set; }
    }
}