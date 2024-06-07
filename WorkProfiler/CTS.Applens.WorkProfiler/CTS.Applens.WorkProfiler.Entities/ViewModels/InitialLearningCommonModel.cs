using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    /// <summary>
    /// Class to hold data for view
    /// </summary>
    public class UserDetailsBaseModel
    {
        public long CustomerId { get; set; }

        public int IsCognizant {get; set;}

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ErrorMessage { get; set; }

        public List<RolePrivilegeModel> RolePrevilageMenus { get; set; }

        public HiddenFieldsModel HiddenFields { get; set; }

        public List<GetProjectDetailsById> ProjectDetails { get; set; }

        public TimeZoneInfoByCustomer TimeZoneInfoByEmployeeID { get; set; }

        public string AutoClassificationMessage { get; set; }

        public List<UserDetails> UserDetails { get; set; }

        public List<ErrorLog> ErrorLogList { get; set; }
    }

    /// <summary>
    /// Class to hold data for view
    /// </summary>
    public class UserProjectDetailsBaseModel
    {
        public string CustomerId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ErrorMessage { get; set; }

        public List<RolePrivilegeModel> RolePrevilageMenus { get; set; }

        public HiddenFieldsModel HiddenFields { get; set; }

        public List<HiddenUserProjectID> HiddenUserProjectDetails { get; set; }

        public List<CustomerModel> UserWiseCustomer { get; set; }

        public string UnfreezeGracePeriod { get; set; }

        public TimeZoneInfoByCustomer TimeZoneInfoByEmployeeID { get; set; }
    }

    public class ValidationResultBaseModel
    {
        public string MessageValue { get; set; }

        public ILValidationResult ILValidationResults { get; set; }

        public MLDetails MLDetail { get; set; }

    }

    public class PattenValidation : ValidationResultBaseModel
    {

        public int SupportTypeID { get; set; }

        public bool ShowAdditionalTextPattern { get; set; }

        public bool ShowSubPattern { get; set; }

        public bool ShowAdditionalTextSubPattern { get; set; }

        public List<MLPatternValidationInfra> MLPatternDetails { get; set; }

        public List<DebtMLPatternValidationModel> DebtMLPatternDetails { get; set; }

        public NoiseEliminationInfra NoiseEliminationInfras { get; set; }

        public NoiseElimination NoiseEliminations { get; set; }

        public List<DebtSamplingGetModel> DebtSamplingGetModels { get; set; }
    }

    public class SPDebtMLPatternValidationModel
    {
        public int SupportTypeID { get; set; }

        public SpDebtMLPatternValidationModelForViewAll SpDebtMLPatternValidationModels { get; set; }
    }

    public class PatternOccurenceInfra
    {
        public int AvoidableFlagID { get; set; }

        public int DebtClassificationID { get; set; }

        public int ResidualCodeID { get; set; }

        public MlPatternInfraDetails MLPatternOccurenceInfra { get; set; }

        public List<DebtMLPatternValidationModel> DebtMLPatternValidationModelDetails { get; set; }
    }

    public class VerifiedJobStatusResult 
    {
        public bool Status { get; set; }

        public string Date { get; set; }

        public bool IsCLEnabled { get; set; }        
    }

    public class ReloadErrors
    {
        public string ViewName { get; set; }

        public List<ErrorLog> ErrorLogList { get; set; }
    }
}
