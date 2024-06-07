namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds properties of LanguageModel
    /// </summary>
    public class LanguageModel
    {
        /// <summary>
        /// Gets or sets the EmployeeID
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets the Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Get and Set's the Language Name In English Property
        /// </summary>
        public string LanguageNameInEnglish { get; set; }
        
        /// <summary>
        /// Gets or sets the Language Code
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the Is Selected
        /// </summary>
        public bool IsSelected { get; set; }

    }
}
