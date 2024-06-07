using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    public class SelectedTicketModel
    {
        public string CustomerID { get; set; }
        public string EmployeeID { get; set; }
        public string FirstDateOfWeek { get; set; }
        public string LastDateOfWeek { get; set; }
        public string Mode { get; set; }
        public List<TicketIDSupport> TicketID_Desc { get; set; }
        public string ProjectID { get; set; }
        public int? isCognizant { get; set; }
    }
}
