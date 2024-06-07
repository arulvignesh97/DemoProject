using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds DataDictionaryModelList details
    /// </summary>
    public class DataDictionaryModelList
    {
        /// <summary>
        /// Gets or sets GriddataList
        /// </summary>
        public GriddataList GridDataList { get; set; }
        /// <summary>
        /// Gets or sets Applicationlist
        /// </summary>
        public List<ApplicationModel> ApplicationList { get; set; }
        /// <summary>
        /// Gets or sets ResolutionList
        /// </summary>
        public List<ResolutionModelDebt> ResolutionList { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationList
        /// </summary>
        public List<DebtClassificationModelDebt> DebtClassificationList { get; set; }
        /// <summary>
        /// Gets or sets CauseList
        /// </summary>
        public List<CauseModelDebt> CauseList { get; set; }
        /// <summary>
        /// Gets or sets ResidualDebtList
        /// </summary>
        public List<ResidualModelDebt> ResidualDebtList { get; set; }
        /// <summary>
        /// Gets or sets AvoidableFlagList
        /// </summary>
        public List<AvoidableModelFlag> AvoidableFlagList { get; set; }
        /// <summary>
        /// Gets or sets ReasonForResidualList
        /// </summary>
        public List<ReasonForResidual> ReasonForResidualList { get; set; }
    }
}
