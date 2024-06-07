using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds DataOverRideListModel details
    /// </summary>
    public class DataOverRideListModel
    {
        /// <summary>
        /// Gets or sets DebtOverrideReviewList
        /// </summary>
        public List<DebtOverrideReview> DebtOverrideReviewList { get; set; }
        /// <summary>
        /// Gets or sets ResolutionModelList
        /// </summary>
        public List<ResolutionModelDebt> ResolutionModelList { get; set; }
        /// <summary>
        /// Gets or sets DebtClassificationModelList 
        /// </summary>
        public List<DebtClassificationModelDebt> DebtClassificationModelList { get; set; }
        /// <summary>
        /// Gets or sets CauseModelList
        /// </summary>
        public List<CauseModelDebt> CauseModelList { get; set; }
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
        /// <summary>
        /// Gets or sets TicketRoleList
        /// </summary>
        public List<TicketRole> TicketRoleList { get; set; }
        }
}
