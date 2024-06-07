using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    public class InsertAttributeSaveModel
    {
        public List<SaveAttributeModel> InsertAttributeList { get; set; }

        public string UserId { get; set; }

        public int ResidualDebtId { get; set; }

        public Int64 AvoidableFlagId { get; set; }

        public string IsAttributeUpdated { get; set; }

        public string TicketStatusId { get; set; }

        public string IsTicketDescriptionUpdated { get; set; }

        public string IsTicketSummaryUpdated { get; set; }
        /// <summary>
        /// Gets or Sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }


    }
}
