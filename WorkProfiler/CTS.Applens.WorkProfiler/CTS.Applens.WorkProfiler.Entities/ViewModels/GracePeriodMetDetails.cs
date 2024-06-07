using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    public class GracePeriodMetDetails
    {
        public DateTime? ClosedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int GracePeriod { get; set; }

        [MaxLength(int.MaxValue)]
        public string ProjectTimeZoneName { get; set; }

        [MaxLength(int.MaxValue)]
        public string UserTimeZoneName { get; set; }

        public bool IsGracePeriodMet { get; set; }

        public int DARTStatusID { get; set; }
    }

    public class TicketAttributeAdditionParam
    {
        public List<InsertAttributeModel> InsertAttributeList { get; set; }
        [MaxLength(int.MaxValue)] 
        public string UserID { get; set; }

        public int ResidualDebtId { get; set; }

        [MaxLength(int.MaxValue)] 
        public string AvoidalFlagId { get; set; }

        [MaxLength(int.MaxValue)] 
        public string IsAttributeUpdated { get; set; }

        [MaxLength(int.MaxValue)] 
        public string TicketStatusID { get; set; }

        [MaxLength(int.MaxValue)] 
        public string UserTimeZone { get; set; }

        [MaxLength(int.MaxValue)] 
        public string ProjectTimeZone { get; set; }

        public int SupportTypeID { get; set; }
    }
}
