using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Common;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ManagedDataSets;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// Class file for holding the Business Logics for Timesheet related functionalities
    /// </summary>
    public class TimeSheetRepository : DBContext
    {
        /// <summary>
        /// GetAssignessOrDefaulters
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<AssignessOrDefaulters> GetAssignessOrDefaulters(ApprovalUnfreezeInputParams2 InputParameter)
        {
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            List<AssignessOrDefaulters> lstResult = new List<AssignessOrDefaulters>();
            string sPName = "[AVL].[sp_GetLeadReportees]";
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@CustomerId", InputParameter.CustomerId);
                prms[1] = new SqlParameter("@EmployeeId", InputParameter.EmployeeId);
                prms[2] = new SqlParameter("@StartDate", DateTime.ParseExact(InputParameter.StartDate,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prms[3] = new SqlParameter("@EndDate", DateTime.ParseExact(InputParameter.EndDate,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                datasetResultSetObject.Tables.Add(new DBHelper().GetTableFromSP(sPName, prms, ConnectionString).Copy());
                if (datasetResultSetObject.Tables.Count > 0)
                {
                    lstResult = ListExtensions.ToListof<AssignessOrDefaulters>(datasetResultSetObject.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        /// <summary>
        /// GetTimeSheetData
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<TimeSheetData> GetTimeSheetData(ApprovalUnfreezeInputParams InputParameter)
        {
            string[] format = new string[] { "dd/MM/yyyy" };
            DateTime datetime;
            bool IsExistsstartDate;
            bool IsExistsEndDate;
            if (DateTime.TryParseExact(InputParameter.StartDate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime))
            {
                IsExistsstartDate = false;
            }
            else
            {
                IsExistsstartDate = true;
            }
            if (DateTime.TryParseExact(InputParameter.EndDate, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime))
            {
                IsExistsEndDate = false;
            }
            else
            {
                IsExistsEndDate = true;
            }
            List<TimeSheetData> lstResult = new List<TimeSheetData>();
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            List<TimeSheetDataDaily> DailyTimeSheetDataList = new List<TimeSheetDataDaily>();
            string sPName = "[AVL].[ApproveUnfreezeBindGrid]";
            DateTimeOffset fromDate = DateTimeOffset.ParseExact(IsExistsstartDate == false ? InputParameter.StartDate : DateTimeOffset.Now.DateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            DateTimeOffset endDate = DateTimeOffset.ParseExact(IsExistsEndDate == false ? InputParameter.EndDate : DateTimeOffset.Now.DateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            DataSetResultSet dsIDParam = new DataSetResultSet();
            List<String> lstSubmitterId = new List<string>();
            List<TimesheetDetails> lstSubmitterIdDetails = new List<TimesheetDetails>();
            DateTimeOffset currDate = DateTimeOffset.Now.DateTime;
            System.TimeSpan diff = endDate.Subtract(currDate);
            int freezeFlag;
            int period = Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["WeekPeriod"]);
            if (diff.Days < period)
            {
                freezeFlag = 1;
            }
            else
            {
                freezeFlag = 0;
            }
            int dropDown = 1;
            try
            {
                if (!string.IsNullOrEmpty(InputParameter.AssignessOrDefaultersID))
                {
                    lstSubmitterId = InputParameter.AssignessOrDefaultersID.Split(',').ToList();
                    dropDown = 1;
                }
                else if (!string.IsNullOrEmpty(InputParameter.DefaulterId))
                {
                    lstSubmitterId = InputParameter.DefaulterId.Split(',').ToList();
                    dropDown = 2;
                }
                else
                {
                    //mandatory else
                }

                if (lstSubmitterId != null && lstSubmitterId.Count > 0)
                {
                    lstSubmitterId.ForEach(x =>
                    {
                        lstSubmitterIdDetails.Add(new TimesheetDetails
                        {
                            EmployeeId = x
                        });
                    });
                }
                dsIDParam.Tables.Add(ListExtensions.ToDataTable<TimesheetDetails>(lstSubmitterIdDetails).Copy());
                dsIDParam.Tables[0].TableName = "AVL.TVP_Assigness_GetTimeSheetData";

                SqlParameter[] prms = new SqlParameter[5];

                prms[0] = new SqlParameter("@FromDate", fromDate);
                prms[1] = new SqlParameter("@ToDate", endDate);
                prms[2] = new SqlParameter("@CustomerId", InputParameter.CustomerId);
                prms[3] = new SqlParameter("@DropDownFlag", dropDown);
                prms[4] = new SqlParameter("@EmployeeIdTVP", dsIDParam.Tables[0]);
                prms[4].SqlDbType = SqlDbType.Structured;
                prms[4].TypeName = "AVL.TVP_Assigness_GetTimeSheetData";
                DataSet dsGetTimeSheetWeekly = new DBHelper().GetDatasetFromSP(sPName, prms, ConnectionString);
                if (dsGetTimeSheetWeekly.Tables.Count > 0 && dsGetTimeSheetWeekly.Tables[0].Rows.Count > 0)
                {
                    lstResult = dsGetTimeSheetWeekly.Tables[0].AsEnumerable().Select(row => new TimeSheetData
                    {
                        RejectionCommects = Convert.ToString(row["RejectionComments"]),
                        TotalHours = Convert.ToDecimal(row["TotalHours"]),
                        EmployeeId = Convert.ToString(row["EmployeeId"]),
                        EmployeeName = Convert.ToString(row["EmployeeName"]),
                    }).ToList();
                }

                if (dsGetTimeSheetWeekly.Tables.Count > 1 && dsGetTimeSheetWeekly.Tables[1].Rows.Count > 0)
                {

                    DailyTimeSheetDataList = dsGetTimeSheetWeekly.Tables[1].AsEnumerable().
                        Select(dailyrow => new TimeSheetDataDaily
                        {
                            EmployeeId = Convert.ToString(dailyrow["EmployeeId"]),
                            EmployeeName = Convert.ToString(dailyrow["EmployeeName"]),
                            TimeSheetDate = Convert.ToDateTime(dailyrow["TimeSheetDate"]),
                            TotalHours = Convert.ToDecimal(dailyrow["TotalHours"]),
                            TimeSheetStatus = Convert.ToString(dailyrow["TimeSheetStatus"]),
                            TimesheetStatusId = Convert.ToInt32(dailyrow["TimesheetStatusId"]),
                            TimesheetId = Convert.ToInt64(dailyrow["TimesheetId"]),
                            ProjectId = Convert.ToString(dailyrow["ProjectId"])
                        }).ToList();

                }
                lstResult.ForEach(x => x.DailyTimeSheetData = DailyTimeSheetDataList.
                        Where(a => a.EmployeeId == x.EmployeeId).ToList());
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && a.TimesheetStatusId == 1).Count() > 0 ? "Saved" : x.TimeSheetStatus);
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && x.TotalHours == 0).Count() > 0 ? "NA" : x.TimeSheetStatus);
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && a.TimesheetStatusId == 2).Count() > 0 ? "Submitted" : x.TimeSheetStatus);
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && a.TimesheetStatusId == 4).Count() > 0 ? "Unfrozen" : x.TimeSheetStatus);
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && a.TimesheetStatusId == 3).Count() > 0 ? "Approved" : x.TimeSheetStatus);
                lstResult.ForEach(x => x.TimeSheetStatus = x.DailyTimeSheetData.Where(a => a.EmployeeId ==
                x.EmployeeId && a.TimesheetStatusId == 6).Count() > 0 ? "Freezed" : x.TimeSheetStatus);

                //Freezing Logic

                lstResult.ForEach(x => x.IsFreezed = ((x.TimeSheetStatus.ToLower() == "Saved".ToLower() ||
                x.TimeSheetStatus.ToLower() == "NA".ToLower()) && (freezeFlag == 1)) ? true : false);
            }


            catch (Exception ex)
            {
                throw ex;
            }

            return lstResult;
        }
        /// <summary>
        /// This Method Is Used To GetTimeSheetDataDaily
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<TimeSheetDataDaily> GetTimeSheetDataDaily(ApprovalUnfreezeInputParams InputParameter)
        {

            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            List<TimeSheetDataDaily> dailyTimeSheetDataList = new List<TimeSheetDataDaily>();
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime fromDate = DateTime.ParseExact(InputParameter.StartDate, new string[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" }, provider, System.Globalization.DateTimeStyles.None);
            DateTime endDate = DateTime.ParseExact(InputParameter.EndDate, new string[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" }, provider, System.Globalization.DateTimeStyles.None);
            DataSetResultSet dsIDParam = new DataSetResultSet();
            List<String> lstSubmitterId = new List<string>();
            List<TimesheetDetails> lstSubmitterIdDetails = new List<TimesheetDetails>();
            DateTime currDate = DateTimeOffset.Now.DateTime;
            System.TimeSpan diff = endDate.Subtract(currDate);
            int freezeFlag;
            int dropDown = 1;
            int period = Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["DailyPeriod"]);
            if (diff.Days < period)
            {
                freezeFlag = 1;
            }
            else
            {
                freezeFlag = 0;
            }
            string sPName = "[AVL].[ApproveUnfreezeBindGrid]";
            try
            {
                if (!string.IsNullOrEmpty(InputParameter.AssignessOrDefaultersID))
                {
                    lstSubmitterId = InputParameter.AssignessOrDefaultersID.Split(',').ToList();
                    dropDown = 1;
                }
                else if (!string.IsNullOrEmpty(InputParameter.DefaulterId))
                {
                    lstSubmitterId = InputParameter.DefaulterId.Split(',').ToList();
                    dropDown = 2;
                }
                else
                {
                    //mandatory else
                }

                if (lstSubmitterId != null && lstSubmitterId.Count > 0)
                {
                    lstSubmitterId.ForEach(x =>
                    {
                        lstSubmitterIdDetails.Add(new TimesheetDetails
                        {
                            EmployeeId = x
                        });
                    });
                }
                dsIDParam.Tables.Add(ListExtensions.ToDataTable<TimesheetDetails>(lstSubmitterIdDetails).Copy());
                dsIDParam.Tables[0].TableName = "AVL.TVP_Assigness_GetTimeSheetData";
                SqlParameter[] prms = new SqlParameter[5];

                prms[0] = new SqlParameter("@FromDate", DateTime.ParseExact(InputParameter.StartDate,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prms[1] = new SqlParameter("@ToDate", DateTime.ParseExact(InputParameter.EndDate,
                    "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                prms[2] = new SqlParameter("@CustomerId", InputParameter.CustomerId);
                prms[3] = new SqlParameter("@DropDownFlag", dropDown);
                prms[4] = new SqlParameter("@EmployeeIdTVP", dsIDParam.Tables[0]);
                prms[4].SqlDbType = SqlDbType.Structured;
                prms[4].TypeName = "AVL.TVP_Assigness_GetTimeSheetData";

                datasetResultSetObject.Tables.Add(new DBHelper().GetDatasetFromSP(sPName, prms, ConnectionString).Tables[1].Copy());
                if (datasetResultSetObject.Tables.Count > 0)
                {
                    dailyTimeSheetDataList = datasetResultSetObject.Tables[0].AsEnumerable().
                        Select(dailyrow => new TimeSheetDataDaily
                        {
                            EmployeeId = Convert.ToString(dailyrow["EmployeeId"]),
                            EmployeeName = Convert.ToString(dailyrow["EmployeeName"]),
                            TimeSheetDate = Convert.ToDateTime(dailyrow["TimeSheetDate"]),
                            TotalHours = Convert.ToDecimal(dailyrow["TotalHours"]),
                            TimeSheetStatus = Convert.ToString(dailyrow["TimeSheetStatus"]) == "" ?
                        "NA" : Convert.ToString(dailyrow["TimeSheetStatus"]),
                            TimesheetStatusId = Convert.ToInt32(dailyrow["TimesheetStatusId"]),
                            TimesheetId = Convert.ToInt64(dailyrow["TimesheetId"]),
                            ProjectId = Convert.ToString(dailyrow["ProjectId"])
                        }).ToList();

                    if (currDate.DayOfWeek == DayOfWeek.Monday)
                    {
                        dailyTimeSheetDataList.ForEach(x => x.IsFreezed = ((x.TimeSheetStatus.ToLower() ==
                        "Saved".ToLower() || x.TimeSheetStatus.ToLower() == "NA".ToLower()) &&
                        (x.TimeSheetDate.Subtract(currDate).Days <= (period - 2))) ? true : false);
                    }
                    else
                    {
                        dailyTimeSheetDataList.ForEach(x => x.IsFreezed = ((x.TimeSheetStatus.ToLower() ==
                        "Saved".ToLower() || x.TimeSheetStatus.ToLower() == "NA".ToLower()) &&
                        (x.TimeSheetDate.Subtract(currDate).Days <= period)) ? true : false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dailyTimeSheetDataList;
        }
        /// <summary>
        /// This Method Is Used To UpdateTimeSheetData
        /// </summary>
        /// <param name="lstApproveUnfreezeTimesheet"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool UpdateTimeSheetData(List<ApproveUnfreezeTimesheet> lstApproveUnfreezeTimesheet, Int64 CustomerID,bool isDaily, string userid, string access)
        {
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            string sPName = "[AVL].[UpdateTimesheetForApproval]";
            datasetResultSetObject.Tables.Add(ListExtensions.ToDataTable<ApproveUnfreezeTimesheet>
                (lstApproveUnfreezeTimesheet).Copy());
            datasetResultSetObject.Tables[0].TableName = "ApproveUnfreezeTimesheet";

            string MyActivityNeededKey = new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"];


            try
            {
                datasetResultSetObject.Tables[0].Columns.Remove("CustomerID");
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ApproveUnfreezeTimesheet", datasetResultSetObject.Tables[0]);
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "AVL.AVL_ApproveUnfreezeTimesheet";
                prms[1] = new SqlParameter("@CustomerID", CustomerID);
                DataTable tblUnfreeze = new DataTable();
                tblUnfreeze.Locale = CultureInfo.InvariantCulture;
                tblUnfreeze = new DBHelper().GetTableFromSP(sPName, prms, ConnectionString);
                if (tblUnfreeze != null && tblUnfreeze.Rows.Count > 0)
                {

                    List<TaskDetailsList> taskList = new List<TaskDetailsList>();
                    taskList = tblUnfreeze.AsEnumerable().Select(dailyrow => new TaskDetailsList
                    {
                        UserId = dailyrow["UserID"].ToString(),
                        AccountId = Convert.ToInt64(CustomerID),
                        Application = dailyrow["Application"].ToString(),
                        CreatedBy = lstApproveUnfreezeTimesheet.First().EmployeeId,
                        CreatedTime = DateTimeOffset.Now.DateTime.ToString(),
                        DueDate = "",
                        ExpiryAfterRead = dailyrow["ExpiryAfterRead"].ToString(),
                        ExpiryDate = "",
                        Read = dailyrow["Read"].ToString(),
                        RefreshedTime = DateTimeOffset.Now.DateTime.ToString(),
                        Status = dailyrow["Status"].ToString(),
                        TaskDetails = dailyrow["TaskDetails"].ToString(),
                        TaskId = Convert.ToInt16(dailyrow["TaskID"]),
                        TaskName = dailyrow["TaskName"].ToString(),
                        TaskType = dailyrow["TaskType"].ToString(),
                        URL = dailyrow["TaskURL"].ToString(),
                        ModifiedBy = lstApproveUnfreezeTimesheet.First().EmployeeId,
                        ModifiedTime = DateTimeOffset.Now.DateTime.ToString()


                    }).ToList();

                    MyTaskRepository taskRep = new MyTaskRepository();
                    JArray taskdetailsObj = JArray.FromObject(taskList);

                }
                //if (MyActivityNeededKey == "true" && lstApproveUnfreezeTimesheet[0].StatusId != 3)
                //{
                //    string startdayoftheweek = DateTime.Parse(lstApproveUnfreezeTimesheet[0].TimeSheetDate.ToString(), CultureInfo.InvariantCulture)
                //        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                //    string enddayoftheweek = DateTime.Parse(lstApproveUnfreezeTimesheet[lstApproveUnfreezeTimesheet.Count - 1].TimeSheetDate.ToString(), CultureInfo.InvariantCulture)
                //        .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                //    string Workitemcode = new AppSettings().AppsSttingsKeyValues["TimesheetUnfreezeCode"];
                //    long SourceRecordvalue = 0;
                //    List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
                //    bool CheckSourceidstatus;

                //    List<string> Employeeids = new List<string>();
                //    foreach (var item in lstApproveUnfreezeTimesheet)
                //    {
                //        Employeeids.Add(item.EmployeeId);

                //    }
                //    Employeeids = Employeeids.ToHashSet().ToList();

                //    string ActivityDescription;
                //    string Requestorjson;
                //    if (isDaily)
                //    {
                //        ActivityDescription = "Your timesheet has been unfrozen for " + startdayoftheweek + " under the Account " + customername +
                //            ". Click here to submit your timesheet.";

                //        Requestorjson = DateTime.Parse(startdayoftheweek, CultureInfo.InvariantCulture).ToString("MM/d/yyyy", CultureInfo.InvariantCulture);
                //    }
                //    else
                //    {
                //        ActivityDescription = "Your timesheet has been unfrozen for the week " + startdayoftheweek + "-" + enddayoftheweek +
                //            " under the Account " + customername + ". Click here to submit your timesheet.";
                //        Requestorjson = DateTime.Parse(startdayoftheweek, CultureInfo.InvariantCulture).ToString("MM/d/yyyy", CultureInfo.InvariantCulture)
                //        + "," + DateTime.Parse(enddayoftheweek, CultureInfo.InvariantCulture).ToString("MM/d/yyyy", CultureInfo.InvariantCulture);
                //    }
                //    foreach (var item in Employeeids)
                //    {
                //        SourceRecordvalue = CustomerID;
                //        existingAcitivities = new MyActivity().GetExistingActivitys(SourceRecordvalue, Workitemcode, access);

                //        CheckSourceidstatus = existingAcitivities.Any(i => i.SourceRecordID == SourceRecordvalue && i.ActivityTo == item
                //        && Requestorjson.Split(",")[0] == i.RequestorJson.Split(",")[0]);
                //        if (!CheckSourceidstatus)
                //        {
                //            MyActivitySourceDto myActivitySource = new MyActivitySourceDto();
                //            myActivitySource.ActivityDescription = ActivityDescription;
                //            myActivitySource.WorkItemCode = Workitemcode;
                //            myActivitySource.SourceRecordID = SourceRecordvalue;
                //            myActivitySource.RaisedOnDate = DateTime.Today;
                //            myActivitySource.RequestorJson = Requestorjson;
                //            myActivitySource.CreatedBy = userid;
                //            myActivitySource.ActivityTo = item;
                //            string st = new MyActivity().MySaveActivity(myActivitySource, access);
                //        }
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                //MyTaskRepository taskRep = new MyTaskRepository();
               // taskRep.ErrorLOG(ex.Message + " Stack trace: " + ex.StackTrace, "Reached My task Timesheet Rep",
                    //Convert.ToInt64(CustomerID));
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetCalendarData
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<CalendarViewData> GetCalendarData(ApprovalUnfreezeInputParams2 InputParameter)
        {
            byte[] bytes = System.Convert.FromBase64String(InputParameter.EmployeeId);
            string employeeID = System.Text.Encoding.UTF8.GetString(bytes);
            List<CalendarViewData> lstResult = new List<CalendarViewData>();
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            List<TimeSheetDataDaily> dailyTimeSheetDataList = new List<TimeSheetDataDaily>();
            string sPName = "[AVL].[GetCalendarView]";
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@FromDate", InputParameter.StartDate);
                prms[1] = new SqlParameter("@ToDate", InputParameter.EndDate);
                prms[2] = new SqlParameter("@SubmitterId", employeeID);
                prms[3] = new SqlParameter("@CustomerId", InputParameter.CustomerId);

                datasetResultSetObject.Tables.Add(new DBHelper().GetDatasetFromSP(sPName, prms, ConnectionString).Tables[0].Copy());
                lstResult = datasetResultSetObject.Tables[0].AsEnumerable().Select(row => new CalendarViewData
                {
                    DateValue = ((Convert.ToDateTime(row["DateValue"])).Year).ToString(),
                    DayValue = Convert.ToString(row["DayValue"]),
                    ResultValue = Convert.ToInt32(row["ResultValue"]),
                    TimesheetStatus = Convert.ToString(row["TimesheetStatus"]),
                    TimesheetStatusId = Convert.ToInt32(row["TimesheetStatusID"])
                }).ToList();

                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketDetailsForApprovalUnfreeze
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        public List<TicketDetails> GetTicketDetailsForApprovalUnfreeze(int CustomerID,
            string FromDate, string ToDate,
            string SubmitterId, string TsApproverId)
        {
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            List<TicketDetails> lstResult = new List<TicketDetails>();
            DataSetResultSet datasetResultSetObject = new DataSetResultSet();
            List<TimeSheetDataDaily> dailyTimeSheetDataList = new List<TimeSheetDataDaily>();
            string sPName = "AVL.GetTicketDetailsForApprovalUnfreeze";
            try
            {

                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@CustomerID", CustomerID);
                prms[1] = new SqlParameter("@FromDate", DateTime.ParseExact(FromDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture));
                prms[2] = new SqlParameter("@ToDate", DateTime.ParseExact(ToDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture));
                prms[3] = new SqlParameter("@SubmitterId", SubmitterId);
                prms[4] = new SqlParameter("@TsApproverId", TsApproverId);

                datasetResultSetObject.Tables.Add(new DBHelper().GetDatasetFromSP(sPName, prms, ConnectionString).Tables[0].Copy());
                if (datasetResultSetObject.Tables[0] != null && datasetResultSetObject.Tables[0].Rows.Count > 0)
                {
                    lstResult = datasetResultSetObject.Tables[0].AsEnumerable().Select(row => new TicketDetails
                    {
                        TicketId = Convert.ToString(row["TicketId"]),
                        Description =
                        Convert.ToString(row["Type"]) == "T" ?
                        Convert.ToString(string.IsNullOrEmpty(row["Description"].ToString()) ?
                        string.Empty :
                        encryptionEnabled.ToLower() == "enabled" ?
                        aesMod.DecryptStringBytes((string)row["Description"],
                        AseKeyDetail.AesKeyConstVal) : (string)row["Description"]) :
                        Convert.ToString(string.IsNullOrEmpty(row["Description"].ToString()) ?
                        string.Empty :
                        (string)row["Description"]),
                        Service = Convert.ToString(row["Service"]),
                        Activity = Convert.ToString(row["Activity"]),
                        TicketType = Convert.ToString(row["TicketType"]),
                        ITSMEffort = Convert.ToInt32(row["ITSMEffort"]),
                        EffortTillDate = Convert.ToDecimal(row["EffortTillDate"]),
                        MarkAsDataEntry = Convert.ToBoolean(row["MarkAsDataEntry"]),
                        ProjectId = Convert.ToInt32(row["ProjectId"]),
                        ApplicationName = Convert.ToString(row["ApplicationName"]),
                        Tower = Convert.ToString(row["Tower"]),
                        Remarks = Convert.ToString(row["Remarks"]),
                        TimesheetDate = Convert.ToDateTime(row["TimesheetDate"]),
                        DebtClassification = Convert.ToString(row["DebtClassification"]),
                        CauseCode = Convert.ToString(row["CauseCode"]),
                        ResolutionCode = Convert.ToString(row["ResolutionCode"]),
                        AvoidableFlag = Convert.ToString(row["AvoidableFlag"]),
                        ResidualDebt = Convert.ToString(row["ResidualDebt"]),
                        CustomerId = Convert.ToInt32(row["CustomerId"]),
                        IsCognizant = Convert.ToInt32(row["IsCognizant"]),
                        IsEfforTracked = Convert.ToBoolean(row["IsEfforTracked"]),
                        IsITSMLinked = Convert.ToBoolean(row["IsITSMLinked"]),
                        IsDebtEnabled = Convert.ToBoolean(row["IsDebtEnabled"]),
                        IsAcitivityTracked = Convert.ToBoolean(row["IsAcitivityTracked"]),
                        IsMainspringConfigured = Convert.ToBoolean(row["IsMainspringConfigured"]),


                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        /// <summary>
        /// This Method Is Used To GetTicketDetailsPopUp
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        public List<TicketDetails> GetTicketDetailsPopUp(int CustomerID, string FromDate, string ToDate,
            string SubmitterId, string TsApproverId)
        {
            List<TicketDetails> lstResult = null;
            try
            {
                lstResult = new List<TicketDetails>();
                lstResult = GetTicketDetailsForApprovalUnfreeze(CustomerID, FromDate, ToDate, SubmitterId, TsApproverId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        /// <summary>
        /// This Method Is Used To GetTicketDetailsForDownload
        /// </summary>
        /// <param name="lstobject"></param>
        /// <param name="DestinationTemplateFileName"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public byte[] GetTicketDetailsForDownload(List<TicketDetails> lstobject, string DestinationTemplateFileName,
            int CustomerID, bool IsCognizant, bool IsADMApplicableforCustomer)
        {
            try
            {
                var projectID = lstobject.Select(x => x.ProjectId).Distinct().FirstOrDefault();
                DataTable dt = ListExtensions.ToDataTable<TicketDetails>(lstobject);
                dt.Columns["EffortTillDate"].ColumnName = "Effort Hours";
                if (!IsCognizant && !IsADMApplicableforCustomer)
                {
                    dt.Columns["TicketId"].ColumnName = "Ticket ID";
                }
                else
                {
                    dt.Columns["TicketId"].ColumnName = "Ticket /Work Item ID";
                }
                if (lstobject.Select(X => X.IsCognizant).Distinct().First() == 1)
                {
                    dt.Columns.Remove("TicketType");
                    dt.Columns.Remove("UserId");
                    dt.Columns.Remove("CustomerId");
                    dt.Columns.Remove("IsCognizant");
                    dt.Columns.Remove("IsEfforTracked");
                    dt.Columns.Remove("IsITSMLinked");
                    dt.Columns.Remove("IsDebtEnabled");
                    dt.Columns.Remove("IsAcitivityTracked");
                    dt.Columns.Remove("IsMainspringConfigured");
                    dt.Columns.Remove("ITSMEffort");
                    dt.Columns.Remove("MarkAsDataEntry");
                    dt.Columns.Remove("Description");
                    dt.Columns.Remove("Status");
                }
                else
                {
                    dt.Columns.Remove("Service");
                    dt.Columns.Remove("Activity");
                    dt.Columns.Remove("ProjectId");
                    dt.Columns.Remove("UserId");
                    dt.Columns.Remove("CustomerId");
                    dt.Columns.Remove("IsCognizant");
                    dt.Columns.Remove("IsEfforTracked");
                    dt.Columns.Remove("IsITSMLinked");
                    dt.Columns.Remove("IsDebtEnabled");
                    dt.Columns.Remove("IsAcitivityTracked");
                    dt.Columns.Remove("IsMainspringConfigured");
                    dt.Columns.Remove("ITSMEffort");
                    dt.Columns.Remove("MarkAsDataEntry");
                    dt.Columns.Remove("Status");
                }
                new OpenXMLFunctions().ExportDataSet(dt, DestinationTemplateFileName);
                var fileBytes = System.IO.File.ReadAllBytes(DestinationTemplateFileName);
                return fileBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
