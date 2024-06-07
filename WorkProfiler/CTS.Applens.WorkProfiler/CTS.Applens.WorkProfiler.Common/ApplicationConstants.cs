using CTS.Applens.WorkProfiler.Entities.Base;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Class hold the constants which is used in application
    /// </summary>
    public class ApplicationConstants
    {                
        public ApplicationConstants()
        {
            
        }

        /*Cofiguration constants*/
        //Infra Excel Base
        public readonly static string DownloadExcelTempFile = new AppSettings().AppsSttingsKeyValues["DownloadExcelTempFile"];
        //Error Log
        public readonly static string ErrorLogPath = new AppSettings().AppsSttingsKeyValues["ErrorLogPath"];
        public readonly static string ErrorLogEnabled = new AppSettings().AppsSttingsKeyValues["ErrorLogEnabled"];
        public string CloseExcel { get; } = "Close Excel Template";
        public string EmptyString { get; } = "none";          
        public string InvalidTemplateMessage { get; } = "Invalid Template. Please download the latest template and validate before uploading..";
        public string InvalidUploadTemplateMessage { get; } = "Please validate the template before uploading.";
        public string NoDataMessage { get; } = "The file has no data to upload.";
        public string InvalidMessage { get; } = "Invalid Template";
     
        // Ticket Upload Constants
        public string ITSMConfigColumnMappingNotDone { get; } = "Column Mapping has not been done in ITSM Configuration.";
        public string ITSMConfigColumnMappingNotDone1 { get; } = "Column Mapping has not been done in ITSM Configuration";
        public string ProblemWithDownload { get; } = "Problem With Download";
        public string UploadValidTemplate { get; } = "Please upload Valid template, valid file is .xlsx";
        public string DumpUploadFailed { get; } = "Dump Upload Failed.Please check e-mail.";
        public string TemplateNotMatchingWithITSMColMapping { get; } = "Template is not matching with ITSM configuration Column mapping. Please upload valid template.";
        public string UploadedSuccessfully { get; } = "Uploaded successfully";
        public string UploadedSuccessfullyCheckFailedTickets { get; } = "Uploaded successfully. Please check error log for failed tickets.";
        public string FileNotExists { get; } = "File does not exists";
        public string CloseExcelTemplate { get; } = "Close Excel Template";
        public string AccountMisConfiguredMsg { get; } = "AccessdeniedYouraccountisnotconfiguredinAppLenstouseTicketingModulePleasereachouttoyourESAprojectmanager";
        public static readonly string ProjectsMisConfiguredMsg = "AccessdeniedYouraccounthasprojectsisnotconfiguredinAppLenstouseTicketingModulePleasereachouttoyourESAprojectmanager";
        public static readonly string LanguageTranslateDescriptionColumn = "Ticket Description";
        public static readonly string LanguageTranslateResolutionRemarksColumn = "Resolution Remarks";
        public static readonly string TranslateTicketDescriptionColumn = "TicketDescription";
        public static readonly string TranslatedTicketDescriptionColumn = "TranslatedTicketDescription";
        public static readonly string TranslateDestinationLanguageCode = "en";
        public static readonly string TranslateSpanishLanguageCode = "es";
        public static readonly string TranslateValidateString = "Testing if Translator is working";
        public static readonly string ErrorScope = "Ticketing Module: {0}";
        public static readonly string ErrorMessage = "Error: {0} Stack Trace: {1}";
        public static readonly string ApiErrorScope = "Translate Controller: {0}";
        public static readonly string ApiErrorMessage = "Response StatusCode: {0} Response ReasonPhrase: {1}";
        public static readonly string ApiTranslateErrorMessage = "Error in Translate Controller : ";
        public static readonly string ConstantY = "Y";
        public static readonly string DefulatCriteriaMet = "Not Enough";
        public const string MultiLingual_Flag = "MultiLingual";
        public const string ML_Flag = "ML";
        public const string Sampling_Flag = "Sampling";
        public const string Excel_Flag = "Excel";
        public const string OExcel_Flag = "OExcel";
        public const string TExcel_Flag = "TExcel";
        public const string Noise_Flag = "Noise";
        public const string NotEnough_Flag = "Not Enough";
        public const string ConstantN = "N";
        public static readonly string DefaultErrorMessage = "An Error Occured";
        public static readonly string MultiLingualMessage = "MultiLingual translation is in progress,Please visit after sometime";
        public static readonly string MLMessage = "Pattern generation is in progress. You will be notified via e-mail once the process is completed.";
        public static readonly string SamplingMessage = "Debt details are insufficient for Initial Learning. Proceed with sampling for learning generation.";
        public static readonly string DefaultSent = "Sent";
        public static readonly string NoiseMessage = "Keywords Identification for Noise Elimination is in progress.You will be notified via e-mail once the process is completed.";
        public static readonly string NotEnoughMessage = "Insufficient Tickets for performing Initial Learning process. Please upload Tickets Manually !!!";
        public static readonly string NotAutoClassifiedMessage = "Note :ML Debt classification has not been completed.";
        public static readonly string TriperHash = "###";
        public static readonly string ConstantQuestionMark = "?";
        public static readonly string ConstantPercentage = "%";
        public static readonly string Constantbacklash = "/";
        public static readonly string ConstantDataAvailablity = "Data Availability";
        public static readonly string ConstantNoiseElimination = "Noise Elimination";
        public static readonly string ConstantSampling = "Sampling";
        public static readonly string ConstantML = "ML";
        public static readonly string ConstantNA = "NA";
        public static readonly string ConstantNotStarted = "Not Started";
        public static readonly string ConstantSuccess = "Success";
        public static readonly string ConstantPending = "Pending";
        public static readonly string ConstantLevel1 = "Level1";
        public static readonly string ConstantLevel2 = "Level2";
        public static readonly string ConstantLevel3 = "Level3";
        public static readonly string ConstantLevel4 = "Level4";
        public static readonly string ConstantLevel5 = "Level5";
        public static readonly string ApplicationAPIURL = "SingleDebtClassification";
        public static readonly string ApplicationCcRcAPIURL = "SingleDebtClassificationCR";
        public static readonly string InfraAPIURL = "InfraSingleDebtClassification";
        public static readonly string InfraCcRcAPIURL = "InfraSingleDebtClassificationCR";
        public static readonly string constantSaved = "Saved";
        public static readonly string constantReceived = "Received";
        public static readonly string constantSubmitted = "Submitted";
        public static readonly string SamplingUnderProcessMessage = "Once Sampling is completed, you will be notified through mail. Please revisit after the mail is received.";
        public static readonly string MLUnderProcessSceenMessage = " Pattern generation is in progress. You will be notified via e-mail once the process is completed.";
        public static readonly string SuccessfullLengthValidation = "Successfully Completed the Validatelength - All the Length Validation, Result is  : ";
        public static readonly string NoNManBulkDataMessage = "Successfully Executed the ValidateNoNManBulkData - Non Mandatory Columns, the result is : ";
        public static readonly string BulkDataValidationMessage = "Successfully executed the ValidateBulkData - Mandatory Column, the result is : ";
        public static readonly string ColumnValidationMessage = "Successfully  the Column Validation, After Validation Rows Count is : ";

        // Continuous Learning Constants
        public static readonly string ReasonforResidual = "Reason for Residual";
        public static readonly string AvoidableFlag = "Avoidable Flag";
        public static readonly string DebtClassification = "Debt Classification";
        public static readonly string ResidualDebt = "Residual Debt";
        public static readonly string CauseCode = "Cause Code";
        public static readonly string ResolutionCode = "Resolution Code";
        public static readonly string TypeRegenerateApplicationDetails = "AVL.RegenerateApplicationDetails";
        public static readonly string TypeRegenerateTowerDetails = "[AVL].[IDList]";
        public static readonly string DataSaved = "Data saved successfully";
        public static readonly string NoDataFound = "No Changes to Save";
        public static readonly string NoChangesToSave = "No Changes to Save";
        public static readonly string DataSubmitted = "Data submitted successfully";
        public static readonly string NoNewLearnings = "No new learnings available for Review. Please revisit next week";
        public static readonly string MultiLingualMessageKey = "MultiLingualIdentification";
        public static readonly string MLMessageKey = "Patterngenerationisinprogress";
        public static readonly string SamplingMessageKey = "Debtdetailsareinsufficient";
        public static readonly string NoiseMessageKey = "KeywordsIdentification";
        public static readonly string NotEnoughMessageKey = "InsufficientTickets";
        public static readonly string SamplingUnderProcessMessageKey = "OnceSamplingiscompleted";
        public static readonly string MLUnderProcessSceenMessageKey = "Patterngenerationisinprogress";
        public static readonly string DefaultErrorMessageKey = "ErrorOccured";
        public static readonly string RevisitNextWeek = "Please revisit next week.";
        public static readonly string EmailAPIURL = "Email/SendEmail";
        public static readonly string AddTicketServiceClassification = "/ServiceClassification/SingleTicketServiceClassification";
        public readonly static string HadoopServices = new AppSettings().AppsSttingsKeyValues["HadoopServices"];

        //MyActivity Constants
        public static readonly string MyActivityURLConst = "Activity";
        public static readonly string GETActivityURL = "ActivityConfigurations";
        public static readonly string ExpiryActivityURL = "ActivityToExpiry";
        public static readonly string GETExistingActivitysURL = "ActivitiesBySource";
        public static readonly string ExpireBasedOnActivityURL = "ExpireBasedOnActivity";

        //Excel Download and Template Excel Path
        /// <summary>
        /// To Get ErrorLog Path
        /// </summary>
        public string ErrorLogExcel
        {
            get { return new AppSettings().AppsSttingsKeyValues["ErrorLogExcel"]; }
        }
        /// <summary>
        /// To get Error Log Effort Excel
        /// </summary>
        public string ErrorLogEffortExcel
        {
            get { return new AppSettings().AppsSttingsKeyValues["ErrorLogEffortExcel"]; }
        }

        /// <summary>
        /// To Get Download Path
        /// </summary>
        public string DownloadExcelTemp
        {
            get { return new AppSettings().AppsSttingsKeyValues["DownloadExcelTemp"]; }
        }
        /// <summary>
        /// To get Excel Upload Path
        /// </summary>
        public string ExcelEffortUploadPath
        {
            get { return new AppSettings().AppsSttingsKeyValues["ExcelEffortUploadPath"];}
        }
        /// <summary>
        /// To get Excel Upload Path Customer
        /// </summary>
        public string ExcelEffortUploadPathCustomer
        {
            get { return new AppSettings().AppsSttingsKeyValues["ExcelEffortUploadPathCustomer"]; }
        }

        /// <summary>
        /// To get teh grace period for Timesheet Unfreeze
        /// </summary>
        public string UnfreezeDay
        {
            get { return new AppSettings().AppsSttingsKeyValues["UnfreezeDay"]; }
        }
        
        /// <summary>
        /// To get Download Excel Template Path
        /// </summary>
        public string TicketDownloadExcelPath
        {
            get
            {           
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["TicketDetailsTemplate"]);
            }
        }
        /// <summary>
        /// Return ML Template Path
        /// </summary>
        public string ExcelMLTemplatePath
        {
            get
            {        
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["TicketDetailsTemplateML"]);
            }
        }
        /// <summary>
        /// Return ML Infra Template Path 
        /// </summary>
        public string ExcelMLInfraTemplateBasePath
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["MLBaseDetailsInfraTemplate"]);
            }
        }
        /// <summary>
        /// Return Excel Template path of MLBase Details
        /// </summary>
        public string ExcelMLTemplateBasePath
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["MLBaseDetailsTemplate"]);
            }
        }
        /// <summary>
        /// return Continous Learning Template Path
        /// </summary>
        public string ExcelCLTemplatePath
        {
            get
            {              
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["ContinuousLearningTemplate"]);
            }
        }
        /// <summary>
        /// return Continous Learning Template Path
        /// </summary>
        public string ExcelAUTemplatePath
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["ApproveUnfreezeTemplate"]);
            }
        }
        /// <summary>
        /// return Continous Learning Template Path
        /// </summary>
        public string ExcelAUTemplatePathWeekly
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["ApproveUnfreezeWeeklyTemplate"]);
            }
        }
        /// <summary>
        /// Return Debt Review Template Path details
        /// </summary>
        public string ExcelDebtReviewTemplatePath
        {
            get
            {              
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["DebtReviewTemplate"]);
            }
        }
        /// <summary>
        /// Return Debt Review Customer Template Path details
        /// </summary>
        public string ExcelDebtReviewCustomerTemplatePath
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["DebtReviewCustomerTemplate"]);               
            }
        }
        /// <summary>
        /// Return DataDictionary Template Path details
        /// </summary>
        public string ExcelDataDictionaryTemplatePath
        {
            get
            {
                return string.Concat(new AppSettings().AppsSttingsKeyValues["ExcelTemplateFile"],
                new AppSettings().AppsSttingsKeyValues["DataDictionaryTemplate"]);
            }
        }
        //Work Item Constants
        public static readonly int Agile = 5;
        public static readonly int Iterative = 17;
        public static readonly int Waterfall = 16;
        public static readonly int Others = 15;
        public static readonly int Cancelled = 5;
        public static readonly int Completed = 4;
        public static readonly int NewStatus = 1;

        // Search Tickets Constants
        public static readonly int AppSupportID = 1;
        public static readonly int InfraSupportID = 2;
        public static readonly int AppAndInfraSupportID = 3;
        public static readonly int IsApplication = 0;
        public static readonly int IsInfra = 1;

        // Data Dictionary
        public static readonly string ApplicationName = "Application Name";
        public static readonly string CauseCodeName = "Cause Code";
        public static readonly string ResolutionName = "Resolution Code";
        public static readonly string DebtClassficationName = "Debt Category";
        public static readonly string AvoidableFlagName = "Avoidable Flag";
        public static readonly string TicketCount = "Ticket Occurrence";
        public static readonly string ResidualFlag = "Residual Flag";
        public static readonly string Period = "Period";
        public static readonly string ExistingPattern = "Existing Pattern";

    }
}
