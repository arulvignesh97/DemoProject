using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    public class TicketingModuleBaseModel
    {
        public List<ApplicationProjectModel> ApplicationProjectModels { get; set; }

        public List<ProjectModel> ProjectModels { get; set; }
    }

    public class PopupAttributesDetails
    {
        public List<LstCauseCode> MappedFalseCauses { get; set; }

        public List<LstCauseCode> MappedTrueCauses { get; set; }

        public string CurrentDate { get; set; }

        public string RowID { get; set; }

        public string IsAttributeUpdated { get; set; }

        public string ProjectTimeZoneName { get; set; }

        public string UserTimeZoneName { get; set; }

        public bool? IsAppEditable { get; set; }

        public bool IsTicketDescriptionOpted { get; set; }

        public List<PopupAttributeModel> PopupAttributeModels { get; set; }

    }

    public class NonTicketPopupDetailsModel
    {
        public List<ProjectsModel> ProjectModelDetails { get; set; }

        public NonTicketPopupDetails NonTicketPopupDetail { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CurrentDate { get; set; }
    }

    public class ChooseTicketBaseModel
    {
        public string ReturnType { get; set; }

        public List<ChooseSearchTicketDetailsModel> TimeSheetList { get; set; }

        public ChooseWorkItem ChooseWorkItemDetail { get; set; }
    }
}
