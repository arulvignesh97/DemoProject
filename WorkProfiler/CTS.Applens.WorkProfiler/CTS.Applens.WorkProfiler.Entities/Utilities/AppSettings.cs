using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.Base
{
    public class AppSettings
    {
        private Dictionary<string, string> appSettingsKeyValues;

        public Dictionary<string, string> AppsSttingsKeyValues
        {
            get { return AppSettingsSingleton.Instance().GetAppSettingValues(); }
            set { appSettingsKeyValues = value; }
        }
        public AppSettings()
        {

        }
        public AppSettings(IConfiguration config)
        {
            this.appSettingsKeyValues = AppSettingsSingleton.GetInstance(config).GetAppSettingValues();

        }
    }
    public sealed class AppSettingsSingleton
    {
        private static AppSettingsSingleton instance = null;
        private readonly Dictionary<string, string> keyValuePairs;
        private static Object _mutex = new Object();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppSettingsSingleton()
        {
        }

        private AppSettingsSingleton()
        {
        }
        private AppSettingsSingleton(IConfiguration configuration)
        {
            keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("LDAPConnection", configuration["LDAPConnection"]);
            keyValuePairs.Add("isAppServiceEnabled",configuration["isAppServiceEnabled"]);
            keyValuePairs.Add("IsLDAPNeeded", configuration["IsLDAPNeeded"]);
            keyValuePairs.Add("EncryptionEnabled", configuration["EncryptionEnabled"]);
            keyValuePairs.Add("MiddlewareURL", configuration["MiddlewareURL"]);
            keyValuePairs.Add("DataLakePathRoot", configuration["DataLakePathRoot"]);
            keyValuePairs.Add("NameNodeHost", configuration["NameNodeHost"]);
            keyValuePairs.Add("NameNodePort", configuration["NameNodePort"]);
            keyValuePairs.Add("HdfsUserName", configuration["HdfsUserName"]);
            keyValuePairs.Add("DownloadUploadPath", configuration["DownloadUploadPath"]);
            keyValuePairs.Add("TicketsWorkPatternUploadPath", configuration["TicketsWorkPatternUploadPath"]);
            keyValuePairs.Add("RHMSAPI", configuration["RHMSAPI"]);
            keyValuePairs.Add("webhdfsContextRoot", configuration["webhdfsContextRoot"]);
            keyValuePairs.Add("DomainName", configuration["DomainName"]);
            keyValuePairs.Add("HadoopAppApi", configuration["HadoopAppApi"]);
            keyValuePairs.Add("HadoopAPIUrl", configuration["HadoopAPIUrl"]);
            keyValuePairs.Add("EncryptionEnabledAPI", configuration["EncryptionEnabledAPI"]);
            keyValuePairs.Add("UnfreezeDay", configuration["UnfreezeDay"]);
            keyValuePairs.Add("IsADMApplicableforCustomer", configuration["IsADMApplicableforCustomer"]);
            keyValuePairs.Add("TicktingModuleWebURL", configuration["TicktingModuleWebURL"]);
            keyValuePairs.Add("LoginPage", configuration["LoginPage"]);
            keyValuePairs.Add("WebApiURL", configuration["WebApiURL"]);
            keyValuePairs.Add("CommonAPIURL", configuration["CommonAPIURL"]);
            keyValuePairs.Add("MyTaskHost", configuration["MyTaskHost"]);
            keyValuePairs.Add("MyTaskURL", configuration["MyTaskURL"]);
            keyValuePairs.Add("ErrorLogEnabled", configuration["ErrorLogEnabled"]);
            keyValuePairs.Add("IsAuditAvailable", configuration["IsAuditAvailable"]);
            keyValuePairs.Add("EnableThreadforTicketUpload", configuration["EnableThreadforTicketUpload"]);
            keyValuePairs.Add("SleepTimings", configuration["SleepTimings"]);
            keyValuePairs.Add("IsQlikEnabled", configuration["IsQlikEnabled"]);
            keyValuePairs.Add("ErrorLogPath", configuration["ErrorLogPath"]);
            keyValuePairs.Add("ClientApiURL", configuration["ClientApiURL"]);
            keyValuePairs.Add("ClientApiURLInfra", configuration["ClientApiURLInfra"]);
            keyValuePairs.Add("AutoClassificationMessge", configuration["AutoClassificationMessge"]);
            keyValuePairs.Add("ExcelTemplateFile", configuration["ExcelTemplateFile"]);
            keyValuePairs.Add("ExcelEffortUploadPath", configuration["ExcelEffortUploadPath"]);
            keyValuePairs.Add("ExcelEffortUploadPathCustomer", configuration["ExcelEffortUploadPathCustomer"]);
            keyValuePairs.Add("DownloadExcelTemp", configuration["DownloadExcelTemp"]);
            keyValuePairs.Add("DownloadTempPath", configuration["DownloadTempPath"]);
            keyValuePairs.Add("MLTempPath", configuration["MLTempPath"]);
            keyValuePairs.Add("HomePage", configuration["HomePage"]);
            keyValuePairs.Add("ErrorLogExcel", configuration["ErrorLogExcel"]);
            keyValuePairs.Add("Domain", configuration["Domain"]);
            keyValuePairs.Add("TicketDetailsTemplate", configuration["TicketDetailsTemplate"]);
            keyValuePairs.Add("MLBaseDetailsTemplate", configuration["MLBaseDetailsTemplate"]);
            keyValuePairs.Add("MLBaseDetailsInfraTemplate", configuration["MLBaseDetailsInfraTemplate"]);
            keyValuePairs.Add("ContinuousLearningTemplate", configuration["ContinuousLearningTemplate"]);
            keyValuePairs.Add("DebtReviewTemplate", configuration["DebtReviewTemplate"]);
            keyValuePairs.Add("DebtReviewCustomerTemplate", configuration["DebtReviewCustomerTemplate"]);
            keyValuePairs.Add("DataDictionaryTemplate", configuration["DataDictionaryTemplate"]);
            keyValuePairs.Add("ApproveUnfreezeTemplate", configuration["ApproveUnfreezeTemplate"]);
            keyValuePairs.Add("ApproveUnfreezeWeeklyTemplate", configuration["ApproveUnfreezeWeeklyTemplate"]);
            keyValuePairs.Add("InspectCodeElement", configuration["InspectCodeElement"]);
            keyValuePairs.Add("webpages:Version", configuration["webpages:Version"]);
            keyValuePairs.Add("webpages:Enabled", configuration["webpages:Enabled"]);
            keyValuePairs.Add("ClientValidationEnabled", configuration["ClientValidationEnabled"]);
            keyValuePairs.Add("UnobtrusiveJavaScriptEnabled", configuration["UnobtrusiveJavaScriptEnabled"]);
            keyValuePairs.Add("owin:AutomaticAppStartup", configuration["owin:AutomaticAppStartup"]);
            keyValuePairs.Add("SDSupport", configuration["SDSupport"]);
            keyValuePairs.Add("EffortBulkUpload", configuration["EffortBulkUpload"]);
            keyValuePairs.Add("EffortBulkUploadAPIURL", configuration["EffortBulkUploadAPIURL"]);
            keyValuePairs.Add("aspnet:MaxJsonDeserializerMembers", configuration["aspnet: MaxJsonDeserializerMembers"]);
            keyValuePairs.Add("UserAccountActiveDirectory", configuration["UserAccountActiveDirectory"]);
            keyValuePairs.Add("ExcelMLBaseTemplatePath", configuration["ExcelMLBaseTemplatePath"]);
            keyValuePairs.Add("ExcelMLTemplatePath", configuration["ExcelMLTemplatePath"]);
            keyValuePairs.Add("MLExcelsaveTemplatePath", configuration["MLExcelsaveTemplatePath"]);
            keyValuePairs.Add("ExcelsaveTemplatePath", configuration["ExcelsaveTemplatePath"]);
            keyValuePairs.Add("ExcelCLTemplatePath", configuration["ExcelCLTemplatePath"]);
            keyValuePairs.Add("ExcelSaveCLTempPath", configuration["ExcelSaveCLTempPath"]);
            keyValuePairs.Add("DownloadExcelTempFile", configuration["DownloadExcelTempFile"]);
            keyValuePairs.Add("ExcelDebtReviewsaveTemplatePath", configuration["ExcelDebtReviewsaveTemplatePath"]);
            keyValuePairs.Add("ExcelDebtReviewTemplatePath", configuration["ExcelDebtReviewTemplatePath"]);
            keyValuePairs.Add("ExcelDebtReviewCustomersaveTemplatePath", configuration["ExcelDebtReviewCustomersaveTemplatePath"]);
            keyValuePairs.Add("ExcelDebtReviewCustomerTemplatePath", configuration["ExcelDebtReviewCustomerTemplatePath"]);
            keyValuePairs.Add("ExcelDataDictionarysaveTemplatePath", configuration["ExcelDataDictionarysaveTemplatePath"]);
            keyValuePairs.Add("ExcelDataDictionaryTemplatePath", configuration["ExcelDataDictionaryTemplatePath"]);
            keyValuePairs.Add("ExcelExportPath", configuration["ExcelExportPath"]);
            keyValuePairs.Add("ErrorLogEffortExcel", configuration["ErrorLogEffortExcel"]);
            keyValuePairs.Add("ApiKeyHandler", configuration["ApiKeyHandler"]);
            keyValuePairs.Add("APIValueHandler", configuration["APIValueHandler"]);
            keyValuePairs.Add("APIAuthKeyHandler", configuration["APIAuthKeyHandler"]);
            keyValuePairs.Add("APIAuthValueHandler", configuration["APIAuthValueHandler"]);
            keyValuePairs.Add("SMTPADD", configuration["SMTPADD"]);
            keyValuePairs.Add("WeekPeriod", configuration["WeekPeriod"]);
            keyValuePairs.Add("DailyPeriod", configuration["DailyPeriod"]);
            keyValuePairs.Add("Version", configuration["Version"]);
            keyValuePairs.Add("QlikSenseUrl", configuration["QlikSenseUrl"]);
            keyValuePairs.Add("ServicePerformanceReportUrl", configuration["ServicePerformanceReportUrl"]);
            keyValuePairs.Add("DetailedTimesheetReportUrl", configuration["DetailedTimesheetReportUrl"]);
            keyValuePairs.Add("MyAssociateURL", configuration["MyAssociateURL"]);
            keyValuePairs.Add("DataLakeIP", configuration["DataLakeIP"]);
            keyValuePairs.Add("DataLakeInfraIP", configuration["DataLakeInfraIP"]);
            keyValuePairs.Add("DataLakeUsername", configuration["DataLakeUsername"]);
            keyValuePairs.Add("DataLakePort", configuration["DataLakePort"]);
            keyValuePairs.Add("IsBenchMarkApplicable", configuration["IsBenchMarkApplicable"]);
            keyValuePairs.Add("HadoopNoise", configuration["HadoopNoise"]);
            keyValuePairs.Add("HadoopSampling", configuration["HadoopSampling"]);
            keyValuePairs.Add("HadoopRuleExtraction", configuration["HadoopRuleExtraction"]);
            keyValuePairs.Add("IsExtended", configuration["IsExtended"]);
            keyValuePairs.Add("ExtensionStart", configuration["ExtensionStart"]);
            keyValuePairs.Add("ExtensionEnd", configuration["ExtensionEnd"]);
            keyValuePairs.Add("ExtensionDailyStart", configuration["ExtensionDailyStart"]);
            keyValuePairs.Add("ExtensionWeeklyStart", configuration["ExtensionWeeklyStart"]);
            keyValuePairs.Add("ChooseDaysCount", configuration["ChooseDaysCount"]);
            keyValuePairs.Add("ADMWayRedirect", configuration["ADMWayRedirect"]);
            keyValuePairs.Add("ADMWebApiURL", configuration["ADMWebApiURL"]);
            keyValuePairs.Add("FooterAPI", configuration["FooterAPI"]);
            keyValuePairs.Add("SprintReviewClosureUrl", configuration["SprintReviewClosureUrl"]);
            keyValuePairs.Add("AllowedHosts", configuration["AllowedHosts"]);
            keyValuePairs.Add("HadoopServices", configuration["HadoopServices"]);
            keyValuePairs.Add("SmartExeMetricsUrl", configuration["SmartExeMetricsUrl"]);
            keyValuePairs.Add("BasemeasureReportUrl", configuration["BasemeasureReportUrl"]);
            keyValuePairs.Add("ConnectionStrings:AppLensConnection", configuration["ConnectionStrings:AppLensConnection"]);
            keyValuePairs.Add("ExcelDebtTemplatePath", configuration["ExcelDebtTemplatePath"]);
            keyValuePairs.Add("DebtExcelsaveTemplatePath", configuration["DebtExcelsaveTemplatePath"]);
            keyValuePairs.Add("LoggerUrl", configuration["LoggerUrl"]);
            keyValuePairs.Add("MyActivityURL", configuration["MyActivityURL"]); 
            keyValuePairs.Add("MyActivityNavigation", configuration["MyActivityNavigation"]);
            keyValuePairs.Add("IsMyActivityNeeded", configuration["IsMyActivityNeeded"]);
            keyValuePairs.Add("AutoDDWorkItemCode", configuration["AutoDDWorkItemCode"]);
            keyValuePairs.Add("PPDDWorkItemCode", configuration["PPDDWorkItemCode"]);
            keyValuePairs.Add("DebtReviewWorkItemCode", configuration["DebtReviewWorkItemCode"]);
            keyValuePairs.Add("MainspringWorkItemCode", configuration["MainspringWorkItemCode"]);
            keyValuePairs.Add("ConflictPatternWorkItemCode", configuration["ConflictPatternWorkItemCode"]);
            keyValuePairs.Add("DefaulterWorkItemCode", configuration["DefaulterWorkItemCode"]);
            keyValuePairs.Add("TimesheetUnfreezeCode", configuration["TimesheetUnfreezeCode"]);
            keyValuePairs.Add("TicketuploadFailedCode", configuration["TicketuploadFailedCode"]);
            keyValuePairs.Add("EnabledRedisCache", configuration["EnabledRedisCache"]);
            keyValuePairs.Add("KeyCloakEnabled", configuration["KeyCloakEnabled"]);
            keyValuePairs.Add("NewAlgoApiURLApp", configuration["NewAlgoApiURLApp"]);
            keyValuePairs.Add("SheetName", configuration["SheetName"]);
            keyValuePairs.Add("TimesheetFreezeDay", configuration["TimesheetFreezeDay"]);
            keyValuePairs.Add("EnableTimesheetMonthFreeze", configuration["EnableTimesheetMonthFreeze"]);
            keyValuePairs.Add("TimesheetFreezeMonthCount", configuration["TimesheetFreezeMonthCount"]);
            keyValuePairs.Add("IsEmailRequired", configuration["IsEmailRequired"]);
            keyValuePairs.Add("ErrorExcelTemplatePath", configuration["ErrorExcelTemplatePath"]);
            keyValuePairs.Add("ErrorExcelPath", configuration["ErrorExcelPath"]);
            keyValuePairs.Add("ErrorExcelTemplatePathInfra", configuration["ErrorExcelTemplatePathInfra"]);
            keyValuePairs.Add("ErrorExcelTemplatePathCustomer", configuration["ErrorExcelTemplatePathCustomer"]);
            keyValuePairs.Add("FromAddress", configuration["FromAddress"]);
            keyValuePairs.Add("GraphAPI", configuration["GraphAPI"]);
            keyValuePairs.Add("IsCognizant", configuration["IsCognizant"]);
            keyValuePairs.Add("AzureServiceUserID", configuration["AzureServiceUserID"]);
            keyValuePairs.Add("Logs", configuration["Logs"]);
            keyValuePairs.Add("Middlewarekey", configuration["Middlewarekey"]);
        }

        public static AppSettingsSingleton GetInstance(IConfiguration configuration)
        {
            if (instance == null)
            {
                lock (_mutex)
                {
                    if (instance == null)
                    {
                        instance = new AppSettingsSingleton(configuration);
                    }
                }
            }

            return instance;
        }
        public static AppSettingsSingleton Instance()
        {
            if (instance == null)
            {
                lock (_mutex)
                {
                    if (instance == null)
                    {
                        instance = new AppSettingsSingleton(null);
                    }
                }
            }

            return instance;
        }

        public Dictionary<string, string> GetAppSettingValues()
        {
            return keyValuePairs;
        }

    }
}
