using System.Collections.Generic;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Holding all the Constants
    /// </summary>
    public static class Constants
    {
        public static string KeyEnableSaml => "EnableAzureADSAML";
        public static readonly string UploadFolderName = "Upload";
        public static readonly string XlsmFileExtension = ".xlsm";
        public static readonly string TemplateNotValidated = "Invalid Template.Please validate the template and upload it again";

        public readonly static string ApplicationName = "Work Profiler";
        public static readonly string UnAuthenticatedUser = "Admin";
        public static readonly string SuggestedActivitycheck  = "Suggested Activity is mandatory when Type is 'NonDelivery' and Activity Name is 'Others'";
        public static readonly string SuggestedRemarkscheck  = "Remarks is mandatory when Type is 'NonDelivery' and Activity Name is 'Others'";
        public static readonly string CommonforSuggestedActivity = "Suggested activity have the content which is not valid, refer read me sheet";
        public static readonly string NegativeEfforts= "Hours is mandatory and can accept only numeric values from 0.01 to 24.00";
        public static readonly string TicketType  = "Tickettype should be unique for Ticket ID";
        public static readonly string TimesheetFreezeDays  = "Timesheet(s) for the previous month can only be submitted until 4th calendar day of current month";
        public static readonly string Timesheetfuturedates = "Timesheet cannot be submitted for future dates";
        public static readonly string DuplicaterecordsInfra  = "There should not be any duplicate records for the following combination (ID + Activity/Task + Cognizant ID + Timesheet Date)";
        public static readonly string Duplicaterecords  = "There should be only 1 entry for the combination ID + Service Name + Activity Name + Cognizant ID + Timesheet Date";
        public static readonly string TicketIDDuplicate  = "TicketID has the Duplicate Entry";
        public static readonly string duplicateEntry  = "Duplicate Entry";
        public static readonly string EncryptionFailure = "We are facing technical challenges , Please try after sometime";
    }
}
