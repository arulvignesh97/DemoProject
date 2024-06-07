using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    public class MultilingualConfigModel
    {
        /// <summary>
        /// Gets or sets the subscription Key.
        /// </summary>
        public string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the Is SingleOrMulti.
        /// </summary>
        public int IsSingleOrMulti { get; set; }

        /// <summary>
        /// Gets or sets the Is Multilingual Enable.
        /// </summary>
        public int IsMultilingualEnable { get; set; }

        /// <summary>
        /// Gets or sets the Translate Language. 
        /// </summary>
        public IList<MultilinugalTranslateFieldsModel> ListTranslateFields { get; set; }

        /// <summary>
        /// Gets or sets the Translate Fields.
        /// </summary>
        public IList<LanguageModel> ListTranslateLanguage { get; set; }
    }
}
