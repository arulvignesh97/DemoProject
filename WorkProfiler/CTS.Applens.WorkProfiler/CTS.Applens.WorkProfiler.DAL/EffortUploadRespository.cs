using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Models.EffortUpload;
using Microsoft.Data.SqlClient.Server;
using System.Linq;
using System.Reflection;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.IO;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Common;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using Azure.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    public class EffortUploadRespository : DBContext
    {
        private readonly string strFrom = new AppSettings().AppsSttingsKeyValues["SDSupport"];
        private readonly string commonAPIURL = new AppSettings().AppsSttingsKeyValues["CommonAPIURL"];       
        EfforUploadTracker objTrack = new EfforUploadTracker();
        bool Iscognizant;
        Int32 ProjectID;
        string TrackID;
        bool IsEffortTrackActivityWise;
        bool IsDaily;
        bool isinfraproject;
        string result;
        List<EffortUploadDet> EffFinaldata = new List<EffortUploadDet>();

        public List<EffortBulkUpload> GetEffortUploadDetails()
        {
            List<EffortBulkUpload> LstEffortUpload = new List<EffortBulkUpload>();

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("GetEffortProject",ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EffortBulkUpload effup = new EffortBulkUpload();
                        effup.ProjectID = dt.Rows[i]["ProjectID"] != null ? Convert.ToInt32(dt.Rows[i]["ProjectID"]) : 0;
                        effup.EsaProjectID = dt.Rows[i]["EsaProjectID"].ToString();
                        effup.SharePathName = dt.Rows[i]["SharePathName"].ToString();
                        effup.IsCognizant = dt.Rows[i]["IsCognizant"] != null ? Convert.ToBoolean(dt.Rows[i]["IsCognizant"]) : false;
                        effup.IsEffortTrackActivityWise = dt.Rows[i]["IsEffortTrackActivityWise"] != null ? Convert.ToBoolean(dt.Rows[i]["IsEffortTrackActivityWise"]) : false;
                        effup.IsDaily = dt.Rows[i]["IsDaily"] != null ? Convert.ToBoolean(dt.Rows[i]["IsDaily"]) : false;
                        LstEffortUpload.Add(effup);
                    }
                }

            }
            catch(Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return LstEffortUpload;
        }
       
        public EfforUploadTracker GetEffortUploadTracker(int? ID,string ProjectID, string EmployeeID, string EffortUploadDumpFileName,string EffortUploadErrorDumpFile,
                                                               string Status,string FilePickedTime,string APIRequestedTime,string APIRespondedTime,string Remarks)
        {
            EfforUploadTracker effup = new EfforUploadTracker();

            try
            {
                SqlParameter[] prms = new SqlParameter[10];
                prms[0] = new SqlParameter("@ID", ID);
                prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                prms[2] = new SqlParameter("@ProjectID", ProjectID);
                prms[3] = new SqlParameter("@EffortUploadDumpFileName", EffortUploadDumpFileName);
                prms[4] = new SqlParameter("@EffortUploadErrorDumpFile", EffortUploadErrorDumpFile);
                prms[5] = new SqlParameter("@Status", Status);
                prms[6] = new SqlParameter("@FilePickedTime", FilePickedTime);
                prms[7] = new SqlParameter("@APIRequestedTime", APIRequestedTime);
                prms[8] = new SqlParameter("@APIRespondedTime", APIRespondedTime);
                prms[9] = new SqlParameter("@Remarks", Remarks);
                DataTable dt = (new DBHelper()).GetTableRow("EffortUploadTracker", prms, ConnectionString);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        effup.ID = dt.Rows[i]["ID"] != null ? Convert.ToInt32(dt.Rows[i]["ID"]) : 0;
                        effup.ProjectID = dt.Rows[i]["ProjectID"] != null ? dt.Rows[i]["ProjectID"].ToString() : "0";
                        effup.EmployeeID = dt.Rows[i]["CreatedBy"] != null ? dt.Rows[i]["CreatedBy"].ToString() : "0";
                        effup.APIRequestedTime = dt.Rows[i]["APIRequestedTime"] != null ? dt.Rows[i]["APIRequestedTime"].ToString() : string.Empty;
                        effup.APIRespondedTime = dt.Rows[i]["APIRespondedTime"] != null ? dt.Rows[i]["APIRespondedTime"].ToString() : string.Empty;
                        effup.EffortUploadDumpFileName = dt.Rows[i]["EffortUploadDumpFileName"].ToString() != null ? dt.Rows[i]["EffortUploadDumpFileName"].ToString() : "";
                        effup.EffortUploadErrorDumpFile = dt.Rows[i]["EffortUploadErrorDumpFile"].ToString() != null ? dt.Rows[i]["EffortUploadErrorDumpFile"].ToString() : "";
                        effup.Status = dt.Rows[i]["Status"] != null ? dt.Rows[i]["Status"].ToString() : "0";
                        effup.FilePickedTime = dt.Rows[i]["FilePickedTime"] != null ? dt.Rows[i]["FilePickedTime"].ToString() : string.Empty;
                        effup.IsActive = dt.Rows[i]["IsActive"] != null ? Convert.ToBoolean(dt.Rows[i]["IsActive"]) : false;
                        effup.Remarks = dt.Rows[i]["Remarks"] != null ? dt.Rows[i]["Remarks"].ToString() : string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return effup;
        }
        public bool SentMail(EmailDetail MailParam)
        {
            bool result = false;
            try
            {
                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@To", MailParam.To);
                prms[1] = new SqlParameter("@From", MailParam.From);
                prms[2] = new SqlParameter("@CC", MailParam.CC);
                prms[3] = new SqlParameter("@Body", MailParam.Body);
                prms[4] = new SqlParameter("@Subject", MailParam.Subject);

                DataTable dt = (new DBHelper()).GetTableRow("AVL.SendMailEffortUpload", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = Convert.ToBoolean(dt.Rows[0]["Result"]);
                }

            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return result;
        }
        public EffortUploadResultGrid InsertEffortBulkUploadDetails(List<EffortUploadDet> effUpDet, List<EffortUploadDet> effUpDetError, EfforUploadTracker objTrack, string EmployeeID)
        {

            EffortBulkUploadDetailsCollection EffUploadList = new EffortBulkUploadDetailsCollection();
            EffortUploadResultGrid effres = new EffortUploadResultGrid();
            int ErrorlogCount = effUpDetError.Count;
            try
            {
                if (effUpDet.Count > 0)
                {
                    for (int i = 0; i < effUpDet.Count; i++)
                    {
                        EffUploadList.Add(new EffortUploadDet
                        {
                            TrackID = effUpDet[i].TrackID,
                            TicketID = effUpDet[i].TicketID,
                            ServiceName = effUpDet[i].ServiceName,
                            ActivityName = effUpDet[i].ActivityName,
                            SuggestedActivity = effUpDet[i].SuggestedActivity,
                            Remarks = effUpDet[i].SuggestedRemarks,
                            TicketType = effUpDet[i].TicketType,
                            TimeSheetDate = effUpDet[i].TimeSheetDate,
                            HoursCheck = effUpDet[i].HoursCheck,
                            ProjectID = effUpDet[i].ProjectID,
                            CognizantID = effUpDet[i].CognizantID,
                            IsCognizant = effUpDet[i].IsCognizant,
                            Type = effUpDet[i].Type


                        }
                        );
                    }
                    DataTable dt = ToDataTable(EffUploadList);
                    if (dt != null)
                    {
                        if (dt.Columns.Contains("isefforttrackactivitywise"))
                        {
                            dt.Columns.Remove("isefforttrackactivitywise");
                        }
                        if (dt.Columns.Contains("hours"))
                        {
                            dt.Columns.Remove("hours");
                        }
                        if (dt.Columns.Contains("timesheetdatecheck"))
                        {
                            dt.Columns.Remove("timesheetdatecheck");
                        }
                        if (dt.Columns.Contains("isdaily"))
                        {
                            dt.Columns.Remove("isdaily");
                        }
                        if (dt.Columns.Contains("suggestedremarks"))
                        {
                            dt.Columns.Remove("suggestedremarks");
                        }
                    }
                    SqlParameter[] prms = new SqlParameter[2];
                    prms[0] = new SqlParameter("@TvpEffortUpload", dt);
                    prms[0].SqlDbType = SqlDbType.Structured;
                    prms[0].TypeName = "TVP_InputTableTVP";
                    prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
                    DataSet dtError = (new DBHelper()).GetDatasetFromSP("EffortBulkValidation", prms, ConnectionString);
                    if (dtError.Tables[0] != null && dtError.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dtError.Tables[0].Rows.Count; i++)
                        {

                            EffortUploadDet eff = new EffortUploadDet();
                            eff.TicketID = dtError.Tables[0].Rows[i]["TicketID"].ToString();
                            eff.ServiceName = dtError.Tables[0].Rows[i]["ServiceName"].ToString();
                            eff.ActivityName = dtError.Tables[0].Rows[i]["ActivityName"].ToString();
                            eff.TicketType = dtError.Tables[0].Rows[i]["TicketType"].ToString();
                            eff.Hours = Convert.ToDecimal(dtError.Tables[0].Rows[i]["Hours"] == null ? 0.00 : dtError.Tables[0].Rows[i]["Hours"]);
                            eff.CognizantID = dtError.Tables[0].Rows[i]["CognizantID"].ToString();
                            eff.IsCognizant = Convert.ToBoolean(dtError.Tables[0].Rows[i]["IsCognizant"]);
                            eff.ProjectID = Convert.ToInt32(dtError.Tables[0].Rows[i]["ProjectID"]);
                            eff.TimeSheetDate = Convert.ToDateTime(dtError.Tables[0].Rows[i]["TimeSheetDate"]);
                            eff.Remarks = dtError.Tables[0].Rows[i]["Remarks"].ToString();
                            eff.Type = dtError.Tables[0].Rows[i]["Type"].ToString();
                            effUpDetError.Add(eff);
                        }
                    }
                }
                else
                {
                    objTrack = GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                     , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "There is no valid data for Insert ");

                }
                EfforUploadTracker Track = new EfforUploadTracker();

                effres.LstErrorLogDetails = effUpDetError;
                int diff = effUpDetError.Count - ErrorlogCount;
                int SuccessCount = EffUploadList.Count - diff;
                int FailCount = 0;
                FailCount = effUpDetError.Count;
                string TrackId = EffUploadList.Count == 0 ? effUpDetError[0].TrackID : EffUploadList[0].TrackID;
                Int32 ProjectID = EffUploadList.Count == 0 ? effUpDetError[0].ProjectID : EffUploadList[0].ProjectID;
                string Status = "";
                if (FailCount > 0)
                {
                    effres.Status = "Uploaded Successfully. ErrorLog generated";
                    Status = "Failed";
                    Track = GetEffortUploadTracker(Convert.ToInt32(TrackId), null, null, null, null, "1"
                            , null, null, null, "Uploaded Successfully.ErrorLog generated");
                    Deletefile(Track.EffortUploadDumpFileName, Convert.ToInt32(TrackId));
                }
                else if (FailCount == 0)
                {
                    effres.Status = "Uploaded Successfully";
                    Status = "Success";
                    Track = GetEffortUploadTracker(Convert.ToInt32(TrackId), null, null, null, null, "1"
                         , null, null, null, "Uploaded Successfully.");
                    Deletefile(Track.EffortUploadDumpFileName, Convert.ToInt32(TrackId));
                }
                else
                {
                    //CCAP Fix
                }
                EffortUploadSuccessCount effresnew = new EffortUploadSuccessCount();

                effresnew.SuccessCount = SuccessCount;
                effresnew.FailedCount = FailCount;
                effresnew.ProjectID = ProjectID;
                effresnew.Status = Status;
                effresnew.TrackID = TrackId;
                effres.EffortResultCount = effresnew;
            }
            catch (Exception ex)
            {
                effres.Status = "Error in upload";
                GetEffortUploadTracker(Convert.ToInt32(effUpDet[0].TrackID), null, null, null, null, "-1"
                             , null, null, null, "Exception On creating Insert Function:" + ex.Message);
            }
            return effres;
        }

        public string UpdateTicketIsAttributeFlagDetails(Int64 ProjectID)
        {
            string UpdateStatus = "";

            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectId", ProjectID);
                prms[1] = new SqlParameter("@Mode", "EffortBulkUpload");
                DataSet dsTickets = (new DBHelper()).GetDatasetFromSP("[AVL].[Effort_UpdateTicketIsAttributeFlagByProject]", prms, ConnectionString);
                UpdateStatus = "Success";
            }
            catch (Exception ex)
            {

                UpdateStatus = "Failed";
                new ExceptionLogging().LogException(ex, Constants.UnAuthenticatedUser);
            }
            return UpdateStatus;
        }

        private void InsertIntoErrorLog(int SuccessCount, int FailCount, string ErrorFileName, string TrackID, Int32 ProjectID, string Status)
        {
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@SuccessCount", SuccessCount);
            prms[1] = new SqlParameter("@FailCount", FailCount);
            prms[2] = new SqlParameter("@ProjectID", ProjectID);
            prms[3] = new SqlParameter("@TrackID", TrackID);
            prms[4] = new SqlParameter("@EffortUploadErrorDumpFile", ErrorFileName);
            prms[5] = new SqlParameter("@Status", Status);

            DataTable dt = (new DBHelper()).GetTableRow("EffortBulk_InsertErrorLog", prms, ConnectionString);
        }
        public string CreateExcel(List<ErrorExcell> EffortUpload, int ProjectID, string TrackID, bool IsCognizant, bool isinfraproject)
        {

            string orgpath = string.Empty;
            try
            {
                List<ErrorExcell> EffortUploadTicket = new List<ErrorExcell>();
                List<ErrorExcell> EffortUploadNONTicket = new List<ErrorExcell>();
                List<ErrorExcell> EffortUploadFinal = new List<ErrorExcell>();
                List<ErrorExcellCust> EffortUploadFinalCust = new List<ErrorExcellCust>();
                List<ErrorExcellInfra> EffortUploadFinalInfra = new List<ErrorExcellInfra>();
                DataTable dtErrorCust = new DataTable();
                dtErrorCust.Locale = CultureInfo.InvariantCulture;
                DataTable dtErrorInfra = new DataTable();
                dtErrorInfra.Locale = CultureInfo.InvariantCulture;
                DataTable dtError = new DataTable();
                dtError.Locale = CultureInfo.InvariantCulture;

                if (IsCognizant)
                {
                    if (!isinfraproject)
                    {
                        EffortUploadTicket = EffortUpload.GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, 
                            x.CognizantID }).Select(group => group.First()).Where(x => x.TicketID != "NONDELIVERY").ToList();
                    }
                    else
                    {
                        EffortUploadTicket = EffortUpload.GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, 
                            x.CognizantID, x.ActivityName }).Select(group => group.First()).Where(x => x.TicketID != "NONDELIVERY").ToList();
                    }
                    EffortUploadNONTicket = EffortUpload.GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, 
                        x.CognizantID, x.ActivityName }).Select(group => group.First()).Where(x => x.TicketID == "NONDELIVERY" && x.Remarks != "").ToList();
                }
                else
                {
                    EffortUploadTicket = EffortUpload.GroupBy(x => new { x.TicketID, x.Remarks, x.TimeSheetDate, 
                        x.CognizantID }).Select(group => group.First()).Where(x => x.TicketID != "NONDELIVERY").ToList();

                    EffortUploadNONTicket = EffortUpload.GroupBy(x => new { x.TicketID, x.Remarks, x.TimeSheetDate, 
                        x.CognizantID, x.TicketType }).Select(group => group.First()).Where(x => x.TicketID == "NONDELIVERY" && x.Remarks != "").ToList();

                }

                foreach (var ticket in EffortUploadTicket)
                {
                    EffortUploadFinal.Add(ticket);
                }
                foreach (var NONticket in EffortUploadNONTicket)
                {
                    EffortUploadFinal.Add(NONticket);
                }


                if (!IsCognizant)
                {
                    EffortUploadFinalCust = EffortUploadFinal.Select(x => new ErrorExcellCust() { TicketID = x.TicketID, ActivityName = x.TicketType, 
                        CognizantID = x.CognizantID, TimeSheetDate = x.TimeSheetDate, Remarks = x.Remarks }).ToList();
                    dtErrorCust = ToDataTableEffortUpload(EffortUploadFinalCust);
                }
                else
                {
                    if (!isinfraproject)
                    {
                        dtError = ToDataTableEffortUpload(EffortUploadFinal);
                    }
                    else
                    {
                        EffortUploadFinalInfra = EffortUploadFinal.Select(x => new ErrorExcellInfra() { TicketID = x.TicketID, ActivityName = x.ActivityName, 
                            CognizantID = x.CognizantID, TimeSheetDate = x.TimeSheetDate, Remarks = x.Remarks }).ToList();
                        dtErrorInfra = ToDataTableEffortUpload(EffortUploadFinalInfra);
                    }
                }
                string isEMailRequired = new AppSettings().AppsSttingsKeyValues["IsEmailRequired"];

                string sourcepath = string.Empty;
                string newpth = string.Empty;

                sourcepath = IsCognizant ? !isinfraproject ? new AppSettings().AppsSttingsKeyValues["ErrorExcelTemplatePath"] : 
                    new AppSettings().AppsSttingsKeyValues["ErrorExcelTemplatePathInfra"] : new AppSettings().AppsSttingsKeyValues["ErrorExcelTemplatePathCustomer"];


                string strExtension = Path.GetExtension(sourcepath);
                string foldername = new AppSettings().AppsSttingsKeyValues["ErrorExcelPath"];
                string orginalfile = Path.GetDirectoryName(sourcepath) + "\\";
                string filename = Path.GetFileName(sourcepath);
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                FileInfo fleInfo = new FileInfo(sourcepath);
                string struserID = Convert.ToString(ProjectID);
                string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss");
                var ext = strExtension;
                orgpath = foldername + string.Concat(fleInfo.Name.Split('.')[0], "_", TrackID, "_", struserID, "_", strTimeStamp, ext);


                DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile);
                if (directoryInfo.Exists)
                {
                    newpth = directoryInfo + string.Concat(fleInfo.Name.Split('.')[0], "_", TrackID, "_", struserID, "_", strTimeStamp, ext);
                    if (File.Exists(newpth))
                    {
                        File.Delete(newpth);
                        fleInfo.CopyTo(newpth, true);
                    }
                    else
                    {
                        fleInfo.CopyTo(newpth, true);
                    }
                }

                if (IsCognizant)
                {
                    if (!isinfraproject)
                    {
                        new OpenXMLOperations().ToExcelSheetByDataTable(dtError, null, newpth, "Sheet1", null);
                    }
                    else
                    {
                        new OpenXMLOperations().ToExcelSheetByDataTable(dtErrorInfra, null, newpth, "Sheet1", null);
                    }
                }
                else
                {
                    new OpenXMLOperations().ToExcelSheetByDataTable(dtErrorCust, null, newpth, "Sheet1", null);
                }
                EfforUploadTracker objtrack = new EfforUploadTracker();
                objtrack = GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, orgpath, "1"
                               , null, null, null, null);
                if (isEMailRequired == "Y")
                {
                    Mail(struserID, Path.GetFileName(objtrack.EffortUploadDumpFileName), TrackID, IsCognizant);
                }

            }
            catch (Exception ex)
            {
                GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, orgpath, "-1"
                            , null, null, null, "Exception On creating excel:" + ex.Message);
            }
            return orgpath;





        }

        private DataTable ToDataTableEffortUpload<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public void GendrateErrorExcell(EffortUploadResultGrid EffortUploadDetails, int ProjectID, bool IsCognizant, bool isinfraproject)
        {
            try
            {
                List<ErrorExcell> exceldata = new List<ErrorExcell>();
                List<EffortUploadDet> LstEffortUploadDetails = new List<EffortUploadDet>();
                LstEffortUploadDetails = EffortUploadDetails.LstErrorLogDetails;
                string ErrorLogPath = "";
                List<string> ticketiddup = new List<string>();
                List<EffortUploadDet> LstDuplicate = new List<EffortUploadDet>();
                List<EffortUploadDet> LstDuplicateFilterData = new List<EffortUploadDet>();



                if (IsCognizant)
                {
                    var tcktNON = LstEffortUploadDetails.AsEnumerable().Select(x => new ErrorExcell { Type = x.Type, TicketID = x.TicketID, Remarks = x.Remarks, 
                        TimeSheetDate = x.TimeSheetDate.ToString("MM/dd/yyyy"), CognizantID = x.CognizantID, ActivityName = x.ActivityName })
                                   .Where(x => x.TicketID == "NONDELIVERY").GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, x.CognizantID, x.ActivityName }).ToList();
                    if (tcktNON != null && tcktNON.Count > 0)
                    {

                        for (int i = 0; i < tcktNON.Count; i++)
                        {
                            ErrorExcell effdupnon = new ErrorExcell();

                            effdupnon.Type = tcktNON[i].Key.Type.ToUpper().Trim() == "ND" ? "NonDelivery" : tcktNON[i].Key.Type;

                            effdupnon.TicketID = tcktNON[i].Key.TicketID;
                            effdupnon.Remarks = tcktNON[i].Key.Remarks;
                            effdupnon.CognizantID = tcktNON[i].Key.CognizantID;
                            effdupnon.ActivityName = tcktNON[i].Key.ActivityName;
                            effdupnon.TimeSheetDate = (tcktNON[i].Key.TimeSheetDate == "01/01/0001" ? " " : tcktNON[i].Key.TimeSheetDate);
                            exceldata.Add(effdupnon);
                        }
                    }
                    if (!isinfraproject)
                    {
                        var tckt = LstEffortUploadDetails.AsEnumerable().Select(x => new ErrorExcell { Type = x.Type, TicketID = x.TicketID, Remarks = x.Remarks, 
                            TimeSheetDate = x.TimeSheetDate.ToString("MM/dd/yyyy"), CognizantID = x.CognizantID })
                                   .Where(x => x.TicketID != "NONDELIVERY").GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, x.CognizantID }).ToList();


                        if (tckt != null && tckt.Count > 0)
                        {
                            for (int i = 0; i < tckt.Count; i++)
                            {
                                ErrorExcell effdup1 = new ErrorExcell();

                                effdup1.Type = tckt[i].Key.Type.ToUpper().Trim() == "T" ? "Ticket" : tckt[i].Key.Type.ToUpper().Trim() == "W" ? "Work Item" : tckt[i].Key.Type;
                                effdup1.TicketID = tckt[i].Key.TicketID;
                                effdup1.Remarks = tckt[i].Key.Remarks;
                                effdup1.CognizantID = tckt[i].Key.CognizantID;
                                effdup1.TimeSheetDate = (tckt[i].Key.TimeSheetDate == "01/01/0001" ? " " : tckt[i].Key.TimeSheetDate);
                                exceldata.Add(effdup1);
                            }
                        }
                    }
                    else
                    {
                        var tckt = LstEffortUploadDetails.AsEnumerable().Select(x => new ErrorExcell { Type = x.Type, TicketID = x.TicketID, Remarks = x.Remarks, 
                            TimeSheetDate = x.TimeSheetDate.ToString("MM/dd/yyyy"), CognizantID = x.CognizantID, ActivityName = x.ActivityName })
                                   .Where(x => x.TicketID != "NONDELIVERY").GroupBy(x => new { x.Type, x.TicketID, x.Remarks, x.TimeSheetDate, x.CognizantID, x.ActivityName }).ToList();


                        if (tckt != null && tckt.Count > 0)
                        {
                            for (int i = 0; i < tckt.Count; i++)
                            {
                                ErrorExcell effdup1 = new ErrorExcell();

                                effdup1.TicketID = tckt[i].Key.TicketID;
                                effdup1.Remarks = tckt[i].Key.Remarks;
                                effdup1.CognizantID = tckt[i].Key.CognizantID;
                                effdup1.TimeSheetDate = (tckt[i].Key.TimeSheetDate == "01/01/0001" ? " " : tckt[i].Key.TimeSheetDate);
                                effdup1.ActivityName = tckt[i].Key.ActivityName;
                                exceldata.Add(effdup1);
                            }
                        }
                    }
                }
                else
                {
                    var tcktNON = LstEffortUploadDetails.AsEnumerable().Select(x => new ErrorExcell { TicketID = x.TicketID, Remarks = x.Remarks, 
                        TimeSheetDate = x.TimeSheetDate.ToString("MM/dd/yyyy"), CognizantID = x.CognizantID, TicketType = x.TicketType })
                                   .Where(x => x.TicketID == "NONDELIVERY").GroupBy(x => new { x.TicketID, x.Remarks, x.TimeSheetDate, x.CognizantID, x.TicketType }).ToList();

                    if (tcktNON != null && tcktNON.Count > 0)
                    {

                        for (int i = 0; i < tcktNON.Count; i++)
                        {
                            ErrorExcell effdupnon = new ErrorExcell();


                            effdupnon.TicketID = tcktNON[i].Key.TicketID;
                            effdupnon.Remarks = tcktNON[i].Key.Remarks;
                            effdupnon.CognizantID = tcktNON[i].Key.CognizantID;
                            effdupnon.TicketType = tcktNON[i].Key.TicketType;
                            effdupnon.TimeSheetDate = (tcktNON[i].Key.TimeSheetDate == "01/01/0001" ? " " : tcktNON[i].Key.TimeSheetDate);
                            exceldata.Add(effdupnon);
                        }
                    }
                    var tckt = LstEffortUploadDetails.AsEnumerable().Select(x => new ErrorExcell { TicketID = x.TicketID, Remarks = x.Remarks, 
                        TimeSheetDate = x.TimeSheetDate.ToString("MM/dd/yyyy"), CognizantID = x.CognizantID })
                               .Where(x => x.TicketID != "NONDELIVERY").GroupBy(x => new { x.TicketID, x.Remarks, x.TimeSheetDate, x.CognizantID }).ToList();


                    if (tckt != null && tckt.Count > 0)
                    {
                        for (int i = 0; i < tckt.Count; i++)
                        {
                            ErrorExcell effdup1 = new ErrorExcell();

                            effdup1.TicketID = tckt[i].Key.TicketID;
                            effdup1.Remarks = tckt[i].Key.Remarks;
                            effdup1.CognizantID = tckt[i].Key.CognizantID;
                            effdup1.TimeSheetDate = (tckt[i].Key.TimeSheetDate == "01/01/0001" ? " " : tckt[i].Key.TimeSheetDate);
                            exceldata.Add(effdup1);
                        }
                    }
                }

                if (exceldata != null && exceldata.Count > 0)
                {

                    ErrorLogPath = CreateExcel(exceldata, ProjectID, EffortUploadDetails.EffortResultCount.TrackID, IsCognizant, isinfraproject);
                    InsertIntoErrorLog(EffortUploadDetails.EffortResultCount.SuccessCount, EffortUploadDetails.EffortResultCount.FailedCount, Path.GetFileName(ErrorLogPath), 
                        EffortUploadDetails.EffortResultCount.TrackID, EffortUploadDetails.EffortResultCount.ProjectID, EffortUploadDetails.EffortResultCount.Status);
                }
                else
                {
                    InsertIntoErrorLog(EffortUploadDetails.EffortResultCount.SuccessCount, EffortUploadDetails.EffortResultCount.FailedCount, "", 
                        EffortUploadDetails.EffortResultCount.TrackID, EffortUploadDetails.EffortResultCount.ProjectID, EffortUploadDetails.EffortResultCount.Status);
                }
            }
            catch (Exception ex)
            {
                GetEffortUploadTracker(Convert.ToInt32(EffortUploadDetails.EffortResultCount.TrackID), null, null, null, null, null, null, null, null, 
                    "Error: At GendrateErrorExcell Metod || Message: " + ex.Message);
            }

        }
    
        public class EffortBulkUploadDetailsCollection : List<EffortUploadDet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(
                    new SqlMetaData("TrackID", SqlDbType.VarChar, 1000),

                                new SqlMetaData("TicketId", SqlDbType.VarChar, 1000),


                                new SqlMetaData("ServiceName", SqlDbType.VarChar, 1000),

                                new SqlMetaData("ActivityName", SqlDbType.VarChar, 1000),

                                new SqlMetaData("SuggestedActivity", SqlDbType.VarChar, 50),

                                new SqlMetaData("Remarks", SqlDbType.VarChar, 1000),

                               new SqlMetaData("TicketType", SqlDbType.VarChar, 1000),
                                 new SqlMetaData("Hours", SqlDbType.VarChar, 1000),
                                new SqlMetaData("CognizantID", SqlDbType.VarChar, 1000),
                                new SqlMetaData("TimesheetDate", SqlDbType.DateTime),

                                    new SqlMetaData("ProjectID", SqlDbType.BigInt),
                                    new SqlMetaData("IsCognizant", SqlDbType.Bit),
                                    new SqlMetaData("Type", SqlDbType.VarChar, 10)
                              );


                foreach (EffortUploadDet obj in this)
                {
                    sqlRow.SetString(0, obj.TrackID != null ? obj.TrackID : "");
                    sqlRow.SetString(1, obj.TicketID != null ? obj.TicketID : "");
                    sqlRow.SetString(2, obj.ServiceName != null ? obj.ServiceName : "");
                    sqlRow.SetString(3, obj.ActivityName != null ? obj.ActivityName : "");
                    sqlRow.SetString(4, obj.SuggestedActivity != null ? obj.SuggestedActivity : "");
                    sqlRow.SetString(5, obj.Remarks != null ? obj.Remarks : "");
                    sqlRow.SetString(6, obj.TicketType != null ? obj.TicketType : "");
                    sqlRow.SetString(7, obj.HoursCheck);
                    sqlRow.SetString(8, obj.CognizantID != null ? obj.CognizantID : "");
                    sqlRow.SetDateTime(9, obj.TimeSheetDate);

                    sqlRow.SetInt64(10, obj.ProjectID != null ? obj.ProjectID : 0);
                    sqlRow.SetBoolean(11, obj.IsCognizant);
                    sqlRow.SetString(12, obj.Type);

                    yield return sqlRow;
                }
            }
        }

        private void Mail(string projectID, string Excelfilename, string TrackID, bool IsCognizant)
        {
            EmailDetail emailDetail = new EmailDetail();
            string Sharepathusers = string.Empty;
            string strSubject = "Effort Upload Status as on " + DateTimeOffset.Now.DateTime;
            string Strprojectname = "Project Name : ";
            string CustomerName = "Customer Name : ";
            string StrESAprojectID = "ESAProject ID : ";
            string StrProcessedFileName = "Processed File Name : " + Excelfilename;
            string StrAutogeneration = "PS :This is an AutoGenerated Mail.Please do not reply to this email. ";
            StringBuilder sb = new StringBuilder();
            string strbody = string.Empty;
            string str = "Regards";
            string strack = "Uploaded Successfully.Please check error log for failed records.";
            string strsign = "App Lens Team";
            string pid = projectID;
            string SMTPIP = new AppSettings().AppsSttingsKeyValues["SMTPADD"];
            string strCc = string.Empty;
            string strTo = string.Empty;
            string strName = "Hi All,";
            string IsMailer = string.Empty;
            try
            {
                SqlParameter[] prm1 = new SqlParameter[1];
                prm1[0] = new SqlParameter("@ProjectID", Convert.ToInt64(pid));
                DataTable dtSharePathUsers = new DataTable();
                dtSharePathUsers.Locale = CultureInfo.InvariantCulture;
                dtSharePathUsers = (new DBHelper()).GetTableFromSP("GetTicketUploadConfigDetails", prm1, ConnectionString);
                for (int i = 0; i < dtSharePathUsers.Rows.Count; i++)
                {
                    Sharepathusers = dtSharePathUsers.Rows[i]["TicketSharePathUsers"].ToString();
                    IsMailer = dtSharePathUsers.Rows[i]["Ismailer"].ToString();
                }

                if (IsMailer == "Y")
                {
                    SqlParameter[] prm = new SqlParameter[2];
                    prm[0] = new SqlParameter("@projectid", pid);
                    prm[1] = new SqlParameter("@MailTo", Sharepathusers);
                    DataTable maildt = new DataTable();
                    maildt.Locale = CultureInfo.InvariantCulture;
                    maildt = (new DBHelper()).GetTableFromSP("sp_GetMailUserDetails_Effort", prm, ConnectionString);
                    if (maildt != null && maildt.Rows.Count > 0)
                    {
                        strTo = maildt.Rows[0]["EmployeeEmail"].ToString();
                        for (int i = 1; i < maildt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(maildt.Rows[i]["EmployeeEmail"].ToString()))
                            {
                                if (i == maildt.Rows.Count - 1)
                                {
                                    strCc += maildt.Rows[i]["EmployeeEmail"].ToString();
                                }
                                else
                                {
                                    strCc += maildt.Rows[i]["EmployeeEmail"].ToString() + "; ";
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(strCc))
                        {
                            strCc = strTo;
                        }
                    }
                    if (IsCognizant)
                    {
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@projectid", pid);
                        DataTable projectdt = new DataTable();
                        projectdt.Locale = CultureInfo.InvariantCulture;
                        projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                        for (int j = 0; j < projectdt.Rows.Count; j++)
                        {
                            Strprojectname += projectdt.Rows[j]["ProjectName"].ToString();
                            StrESAprojectID += projectdt.Rows[j]["EsaProjectID"].ToString();
                        }
                    }
                    else
                    {
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@projectid", pid);
                        DataTable projectdt = new DataTable();
                        projectdt.Locale = CultureInfo.InvariantCulture;
                        projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                        for (int j = 0; j < projectdt.Rows.Count; j++)
                        {
                            CustomerName += projectdt.Rows[j]["Customer Name"].ToString();
                        }
                    }

                    strbody = "Please find the effort upload status during bulk upload process as below";
                    if (IsCognizant)
                    {
                        emailDetail.To = strTo;
                        emailDetail.From = strFrom;
                        emailDetail.CC = Convert.ToString(strCc);
                        emailDetail.Subject = strSubject;
                        emailDetail.Body = "<font face=Arial, Helvetica, Sans-Serif size=2>" + strName + "<BR><BR>" + strbody + "<BR><BR>" + Strprojectname + "<BR><BR>" + 
                            StrESAprojectID + "<BR><BR>" + StrProcessedFileName + "<BR><BR>" + strack + "<BR><BR>" + str + "<BR>" + strsign + "<BR><BR><BR>" + StrAutogeneration + "</font>";
                        bool res = SendMailForEffortUpload(emailDetail);
                    }
                    else
                    {
                        emailDetail.To = strTo;
                        emailDetail.From = strFrom;
                        emailDetail.CC = Convert.ToString(strCc);
                        emailDetail.Subject = strSubject;
                        emailDetail.Body = "<font face=Arial, Helvetica, Sans-Serif size=2>" + strName + "<BR><BR>" + strbody + "<BR><BR>" + CustomerName + "<BR><BR>" + 
                            StrProcessedFileName + "<BR><BR>" + strack + "<BR><BR>" + str + "<BR>" + strsign + "<BR><BR><BR>" + StrAutogeneration + "</font>";
                        bool res = SendMailForEffortUpload(emailDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, null, "-1"
                            , null, null, null, "Error at Mail: " + ex.Message);
            }
        }
        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="emailDetail"></param>
        /// <returns></returns>
        private static void SendEmailviaMiddleware(EmailDetail emailDetail)
        {
            string emailSentStatus = string.Empty;
            try
            {
                SanitizeStringInput apiMiddleware = new AppSettings().AppsSttingsKeyValues["MiddlewareURL"];
                HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true };
                SanitizeStringInput msgfrom = Convert.ToString(new AppSettings().AppsSttingsKeyValues["FromAddress"]);
                EmailDetail objEmailDet = new EmailDetail()
                {
                    From = emailDetail.From,
                    To = emailDetail.To,
                    CC = emailDetail.CC,
                    Subject = emailDetail.Subject,
                    Body = emailDetail.Body
                };
                var emailSerializedData = JsonConvert.SerializeObject(objEmailDet);
                var emailSerializedContent = new StringContent(emailSerializedData, Encoding.UTF8, "application/json");
                var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                using (var client = httpClientFactory.CreateClient())
                {
                    
                    client.BaseAddress = new Uri(apiMiddleware.Value);
                    NameValueCollection config = ConfigurationManager.GetSection("KeyCloakConfig") as NameValueCollection;
                    KeyCloakToken TokenDetails = KeyCloakTokenHelper.GetAccessTokenForJob(Convert.ToBoolean(ConfigurationManager.AppSettings["KeyCloakEnabled"]), config);
                    var responseTask = ApiHelper.Post(objEmailDet, client.BaseAddress.ToString(),
                                    TokenDetails?.AccessToken,
                                    Convert.ToBoolean(ConfigurationManager.AppSettings["KeyCloakEnabled"])).Result;
                }
            }
            catch (Exception ex)
            {
                new ExceptionLogging().LogException(ex, Constants.UnAuthenticatedUser);
            }
        }

        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None)
                {
                    return true;
                }

                var request = sender as HttpWebRequest;
                if (request != null)
                {
                    return commonAPIURL.Contains(request.RequestUri.Host);
                }

                return false;
            };
        }
        /// <summary>
        /// SendMailForEffortUpload
        /// </summary>
        /// <param name="emaildetail"></param>
        /// <returns></returns>

        private bool SendMailForEffortUpload(EmailDetail emaildetail)
        {
            bool result = false;
            SqlParameter[] prms = new SqlParameter[5];
            prms[0] = new SqlParameter("@To", emaildetail.To);
            prms[1] = new SqlParameter("@From", emaildetail.From);
            prms[2] = new SqlParameter("@CC", emaildetail.CC);
            prms[3] = new SqlParameter("@Body", emaildetail.Body);
            prms[4] = new SqlParameter("@Subject", emaildetail.Subject);
            DataSet dsmailresult = new DBHelper().GetDatasetFromSP("[AVL].[SendMailEffortUpload]", prms, ConnectionString);
            result = Convert.ToBoolean(dsmailresult.Tables[0].Rows[0]["Result"]);
            return result;
        }
        public void TemplateMail(string projectID, string Excelfilename, string TrackID, bool IsCognizant)
        {
            // Mail Sending
            EmailDetail emailDetail = new EmailDetail();
            string CustomerName = "Customer Name : ";
            string Sharepathusers = string.Empty;
            string strSubject = "Effort Upload Status as on " + DateTimeOffset.Now.DateTime;
            string Strprojectname = "Project Name : ";
            string StrESAprojectID = "ESAProject ID : ";
            string StrProcessedFileName = "Processed File Name : " + Excelfilename;
            string StrAutogeneration = "PS :This is an AutoGenerated Mail.Please do not reply to this email. ";
            StringBuilder sb = new StringBuilder();
            string strbody = string.Empty;
            string str = "Regards";
            string strsign = "App Lens Team";
            string pid = projectID;
            string SMTPIP = new AppSettings().AppsSttingsKeyValues["SMTPADD"];
            string strCc = string.Empty;
            string strTo = string.Empty;
            string strName = "Hi All,";
            string IsMailer = string.Empty;
            try
            {
                SqlParameter[] prm1 = new SqlParameter[1];
                prm1[0] = new SqlParameter("@ProjectID", Convert.ToInt64(pid));
                DataTable dtSharePathUsers = new DataTable();
                dtSharePathUsers.Locale = CultureInfo.InvariantCulture;
                dtSharePathUsers = (new DBHelper()).GetTableFromSP("GetTicketUploadConfigDetails", prm1, ConnectionString);
                for (int i = 0; i < dtSharePathUsers.Rows.Count; i++)
                {
                    Sharepathusers = dtSharePathUsers.Rows[i]["TicketSharePathUsers"].ToString();
                    IsMailer = dtSharePathUsers.Rows[i]["Ismailer"].ToString();
                }

                if (IsMailer == "Y")
                {
                    SqlParameter[] prm = new SqlParameter[2];
                    prm[0] = new SqlParameter("@projectid", pid);
                    prm[1] = new SqlParameter("@MailTo", Sharepathusers);
                    DataTable maildt = new DataTable();
                    maildt.Locale = CultureInfo.InvariantCulture;
                    maildt = (new DBHelper()).GetTableFromSP("sp_GetMailUserDetails_Effort", prm, ConnectionString);
                    if (maildt != null && maildt.Rows.Count > 0)
                    {
                        strTo = maildt.Rows[0]["EmployeeEmail"].ToString();
                        for (int i = 1; i < maildt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(maildt.Rows[i]["EmployeeEmail"].ToString()))
                            {
                                if (i == maildt.Rows.Count - 1)
                                {
                                    strCc += maildt.Rows[i]["EmployeeEmail"].ToString();
                                }
                                else
                                {
                                    strCc += maildt.Rows[i]["EmployeeEmail"].ToString() + "; ";
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(strCc))
                        {
                            strCc = strTo;
                        }
                    }
                    if (IsCognizant)
                    {
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@projectid", pid);
                        DataTable projectdt = new DataTable();
                        projectdt.Locale = CultureInfo.InvariantCulture;
                        projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                        for (int j = 0; j < projectdt.Rows.Count; j++)
                        {
                            Strprojectname += projectdt.Rows[j]["ProjectName"].ToString();
                            StrESAprojectID += projectdt.Rows[j]["EsaProjectID"].ToString();
                        }
                    }
                    else
                    {
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@projectid", pid);
                        DataTable projectdt = new DataTable();
                        projectdt.Locale = CultureInfo.InvariantCulture;
                        projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                        for (int j = 0; j < projectdt.Rows.Count; j++)
                        {
                            CustomerName += projectdt.Rows[j]["Customer Name"].ToString();
                        }
                    }

                    strbody = "Please Upload Valid Template";
                    if (IsCognizant)
                    {
                        emailDetail.To = strTo;
                        emailDetail.From = strFrom;
                        emailDetail.CC = Convert.ToString(strCc);
                        emailDetail.Subject = strSubject;
                        emailDetail.Body = "<font face=Arial, Helvetica, Sans-Serif size=2>" + strName + "<BR><BR>" + strbody + "<BR><BR>" + Strprojectname + "<BR><BR>" + 
                            StrESAprojectID + "<BR><BR>" + StrProcessedFileName + "<BR><BR>" + str + "<BR>" + strsign + "<BR><BR><BR>" + StrAutogeneration + "</font>";
                        bool res = SendMailForEffortUpload(emailDetail);
                    }
                    else
                    {
                        emailDetail.To = strTo;
                        emailDetail.From = strFrom;
                        emailDetail.CC = Convert.ToString(strCc);
                        emailDetail.Subject = strSubject;
                        emailDetail.Body = "<font face=Arial, Helvetica, Sans-Serif size=2>" + strName + "<BR><BR>" + strbody + "<BR><BR>" + CustomerName + "<BR><BR>" + 
                            StrProcessedFileName + "<BR><BR>" + str + "<BR>" + strsign + "<BR><BR><BR>" + StrAutogeneration + "</font>";
                        bool res = SendMailForEffortUpload(emailDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, null, "-1"
                            , null, null, null, "Error at Mail: " + ex.Message);
            }
        }
        public void Deletefile(string path, Int32 TrackID)
        {
            try
            {
                string dirctoryName = System.IO.Path.GetDirectoryName(path);
                string fName = System.IO.Path.GetFileNameWithoutExtension(path);
                string validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
                validatePath = RemoveLastIndexCharacter(validatePath);

                string directoryPath = Path.GetDirectoryName(validatePath);

                string FileName = Path.GetFileName(validatePath);
                
                DirectoryInfo di = new DirectoryInfo(directoryPath);
                di.Attributes = FileAttributes.Normal;

                string[] oldFiles = System.IO.Directory.GetFiles(directoryPath, FileName);

                foreach (string currFile in oldFiles)
                {
                    System.IO.FileInfo currFileInfo = new System.IO.FileInfo(currFile);

                    File.SetAttributes(currFileInfo.FullName, FileAttributes.Normal);
                    currFileInfo.Delete();
                }
            }
            catch (Exception e)
            {
                GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, null, "-1"
                            , null, null, null, "Error in delete:" + e.Message);
            }
            finally
            {
                //CCAP FIX
            }
        }

        public OpportunityDetail GetOpportunityTicketdetails()
        {
            new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
            try
            {
                OpportunityDetail opportunityDetail = new OpportunityDetail();
                List<OpportunityModel> lstOpportunity = new List<OpportunityModel>();
                List<HealAutomationTicketModel> lstIdea = new List<HealAutomationTicketModel>();
                Dictionary<string, object> prm = new Dictionary<string, object>();
                string encryptionEnabled = "enabled";
                SqlParameter[] prms = new SqlParameter[0];
                DataSet dsOpportunityTicketdetails = new DBHelper().GetDatasetFromSP("[AVL].[ISpaceDataPushtoGateway]", prms, ConnectionString);

                if (dsOpportunityTicketdetails != null && dsOpportunityTicketdetails.Tables.Count > 0)
                {
                    for (int i = 0; i < dsOpportunityTicketdetails.Tables[0].Rows.Count; i++)
                    {
                        OpportunityModel opportunity = new OpportunityModel();

                        opportunity.ApplensOpportunityId = Convert.ToInt32((dsOpportunityTicketdetails.Tables[0].Rows[i]["ApplensOpportunityId"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["ApplensOpportunityId"].ToString() : string.Empty);
                        opportunity.ProjectId = Convert.ToInt32((dsOpportunityTicketdetails.Tables[0].Rows[i]["ProjectId"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["ProjectId"].ToString() : string.Empty);
                        opportunity.PlannedStartDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["PlannedStartDate"].ToString());
                        opportunity.PlannedEndDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["PlannedEndDate"].ToString());
                        opportunity.ActualEndDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ActualEndDate"].ToString());
                        opportunity.ActualStartDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ActualStartDate"].ToString());
                        opportunity.ReleasePlanName = (dsOpportunityTicketdetails.Tables[0].Rows[i]["ReleasePlanName"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["ReleasePlanName"].ToString() : string.Empty;
                        opportunity.IsDeleted = (dsOpportunityTicketdetails.Tables[0].Rows[i]["IsDeleted"] != DBNull.Value) ?
                            Convert.ToBoolean(dsOpportunityTicketdetails.Tables[0].Rows[i]["IsDeleted"].ToString()) : false;
                        opportunity.TicketType = (dsOpportunityTicketdetails.Tables[0].Rows[i]["TicketType"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["TicketType"].ToString() : string.Empty;
                        opportunity.CreatedBy = (dsOpportunityTicketdetails.Tables[0].Rows[i]["CreatedBy"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["CreatedBy"].ToString() : string.Empty;
                        opportunity.CreatedDate = Convert.ToDateTime(dsOpportunityTicketdetails.Tables[0].Rows[i]["CreatedDate"].ToString());
                        opportunity.ModifiedBy = (dsOpportunityTicketdetails.Tables[0].Rows[i]["ModifiedBy"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["ModifiedBy"].ToString() : string.Empty;
                        opportunity.ModifiedDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ModifiedDate"].ToString());
                        opportunity.TriggeredDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["TriggeredDate"].ToString());
                        opportunity.ISpaceJobStatus = GetNullableIntValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ISpaceJobStatus"].ToString());
                        opportunity.ISpaceJobDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ISpaceJobDate"].ToString());
                        opportunity.ISpaceStatus = (dsOpportunityTicketdetails.Tables[0].Rows[i]["ISpaceStatus"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["ISpaceStatus"].ToString() : string.Empty;
                        opportunity.EsaProjectID = (dsOpportunityTicketdetails.Tables[0].Rows[i]["EsaProjectID"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[0].Rows[i]["EsaProjectID"].ToString() : string.Empty;
                        opportunity.ISpaceOpportunityId = GetNullableIntValues(dsOpportunityTicketdetails.Tables[0].Rows[i]["ISpaceOpportunityId"].ToString());

                        lstOpportunity.Add(opportunity);
                    }
                    new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                    for (int i = 0; i < dsOpportunityTicketdetails.Tables[1].Rows.Count; i++)
                    {
                        HealAutomationTicketModel idea = new HealAutomationTicketModel();

                        idea.ApplensIdeaId = Convert.ToInt32(dsOpportunityTicketdetails.Tables[1].Rows[i]["ApplensIdeaId"].ToString());
                        idea.ProjectPatternMapID = Convert.ToInt32(dsOpportunityTicketdetails.Tables[1].Rows[i]["ProjectPatternMapID"].ToString());
                        idea.HealingTicketID = (dsOpportunityTicketdetails.Tables[1].Rows[i]["HealingTicketID"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["HealingTicketID"].ToString() : string.Empty;
                        idea.TicketType = (dsOpportunityTicketdetails.Tables[1].Rows[i]["TicketType"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["TicketType"].ToString() : string.Empty;
                        idea.DARTStatusID = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["DARTStatusID"].ToString());
                        idea.Assignee = (dsOpportunityTicketdetails.Tables[1].Rows[i]["Assignee"] !=DBNull.Value)? 
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["Assignee"].ToString():string.Empty;
                        idea.ApplicationID = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ApplicationID"].ToString());
                        idea.OpenDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["OpenDate"].ToString());
                        idea.PriorityID = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["PriorityID"].ToString());
                        idea.IsManual = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["IsManual"].ToString());
                        idea.IsPushed = (dsOpportunityTicketdetails.Tables[1].Rows[i]["IsPushed"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["IsPushed"].ToString() : string.Empty;
                        idea.CreatedBy = (dsOpportunityTicketdetails.Tables[1].Rows[i]["CreatedBy"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["CreatedBy"].ToString() : string.Empty;
                        idea.CreatedDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["CreatedDate"].ToString());
                        idea.ModifiedBy = (dsOpportunityTicketdetails.Tables[1].Rows[i]["ModifiedBy"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["ModifiedBy"].ToString() : string.Empty;
                        idea.ModifiedDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ModifiedDate"].ToString());
                        idea.IsDeleted = (dsOpportunityTicketdetails.Tables[1].Rows[i]["IsDeleted"] != DBNull.Value) ?
                            Convert.ToBoolean(dsOpportunityTicketdetails.Tables[1].Rows[i]["IsDeleted"].ToString()) : false;
                        idea.IsMappedToProblemTicket = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["IsMappedToProblemTicket"].ToString());
                        idea.PlannedEffort = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["PlannedEffort"].ToString());
                        idea.HealTypeId = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["HealTypeId"].ToString());
                        idea.PlannedStartDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["PlannedStartDate"].ToString());
                        idea.PlannedEndDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["PlannedEndDate"].ToString());
                        idea.ApplensOpportunityId = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ApplensOpportunityId"].ToString());
                        idea.TicketDescription = (dsOpportunityTicketdetails.Tables[1].Rows[i]["TicketDescription"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["TicketDescription"].ToString() : string.Empty;
                        idea.SolutionType = (dsOpportunityTicketdetails.Tables[1].Rows[i]["SolutionType"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["SolutionType"].ToString() : string.Empty;
                        idea.IsDormant = (dsOpportunityTicketdetails.Tables[1].Rows[i]["IsDormant"] != DBNull.Value) ?
                            Convert.ToBoolean(dsOpportunityTicketdetails.Tables[1].Rows[i]["IsDormant"].ToString()) : false;
                        idea.DormantCreatedDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["DormantCreatedDate"].ToString());
                        idea.MarkAsDormant = (dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormant"] != DBNull.Value) ?
                            Convert.ToBoolean(dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormant"].ToString()) : false;
                        idea.MarkAsDormantDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormantDate"].ToString());
                        idea.MarkAsDormantComments = (dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormantComments"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormantComments"].ToString() : string.Empty;
                        idea.MarkAsDormantBy = (dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormantBy"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["MarkAsDormantBy"].ToString() : string.Empty;
                        idea.ReasonForRepetition = (dsOpportunityTicketdetails.Tables[1].Rows[i]["ReasonForRepetition"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["ReasonForRepetition"].ToString() : string.Empty;
                        idea.ReasonForCancellation = (dsOpportunityTicketdetails.Tables[1].Rows[i]["ReasonForCancellation"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["ReasonForCancellation"].ToString() : string.Empty;
                        idea.ActualEffortReduction = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ActualEffortReduction"].ToString());
                        idea.PlannedEffortReduction = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["PlannedEffortReduction"].ToString());
                        idea.Scope = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["Scope"].ToString());
                        idea.ImplementationStatus = (dsOpportunityTicketdetails.Tables[1].Rows[i]["ImplementationStatus"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["ImplementationStatus"].ToString() : string.Empty;
                        idea.SavingsHardDollarActualCognizant = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsHardDollarActualCognizant"].ToString());
                        idea.SavingsHardDollarActualCustomer = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsHardDollarActualCustomer"].ToString());
                        idea.SavingsHardDollarPlannedCognizant = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsHardDollarPlannedCognizant"].ToString());
                        idea.SavingsHardDollarPlannedCustomer = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsHardDollarPlannedCustomer"].ToString());
                        idea.SavingsSoftDollarActualCognizant = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsSoftDollarActualCognizant"].ToString());
                        idea.SavingsSoftDollarActualCustomer = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsSoftDollarActualCustomer"].ToString());
                        idea.SavingsSoftDollarPlannedCognizant = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsSoftDollarPlannedCognizant"].ToString());
                        idea.SavingsSoftDollarPlannedCustomer = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["SavingsSoftDollarPlannedCustomer"].ToString());
                        idea.IsMandatory = (dsOpportunityTicketdetails.Tables[1].Rows[i]["IsMandatory"] != DBNull.Value) ?
                            Convert.ToBoolean(dsOpportunityTicketdetails.Tables[1].Rows[i]["IsMandatory"].ToString()) : false;
                        idea.IncidentReductionMonth = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["IncidentReductionMonth"].ToString());
                        idea.EffortReductionMonth = GetNullableDecimalValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["EffortReductionMonth"].ToString());
                        idea.TriggeredDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["TriggeredDate"].ToString());
                        idea.ISpaceJobDate = GetNullableDateTimetValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ISpaceJobDate"].ToString());
                        idea.SolutionTypeName = (dsOpportunityTicketdetails.Tables[1].Rows[i]["SolutionType"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["SolutionType"].ToString() : string.Empty;
                        idea.ISpaceIdeaId = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ISpaceIdeaId"].ToString());
                        idea.EsaProjectID = (dsOpportunityTicketdetails.Tables[1].Rows[i]["EsaProjectID"] != DBNull.Value) ?
                            dsOpportunityTicketdetails.Tables[1].Rows[i]["EsaProjectID"].ToString() : string.Empty;
                        idea.ProjectID = GetNullableIntValues(dsOpportunityTicketdetails.Tables[1].Rows[i]["ProjectID"].ToString());

                        lstIdea.Add(idea);
                    }
                    new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                    opportunityDetail.Idea = lstIdea;
                    opportunityDetail.Opportunity = lstOpportunity;
                }
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return opportunityDetail;

            }
            catch (Exception)
            {
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return null;
            }


        }

        public string UpdateIdeaOpportunity(IspaceOpportunityDetail lstIspaceOpportunity)
        {
            try
            {
                var dtOpportunity = ToDataTable(lstIspaceOpportunity.Opportunity);
                var dtIdea = ToDataTable(lstIspaceOpportunity.Idea);

                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@TVP_ISpaceIdeaDetails", dtIdea);
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "dbo.TVP_ISpaceIdeaDetails";
                prms[1] = new SqlParameter("@TVP_ISpaceOpportunityDetails", dtOpportunity);
                prms[1].SqlDbType = SqlDbType.Structured;
                prms[1].TypeName = "dbo.TVP_ISpaceOpportunityDetails";
                prms[2] = new SqlParameter("@IspaceJobDate", DateTimeOffset.Now.DateTime);

                var ds = new DBHelper().GetTableFromSP("[AVL].[ISpaceDataImportToApplens]", prms, ConnectionString);

                return "1";
            }
            catch (Exception)
            {
                throw;
            }
        }
        private decimal? GetNullableDecimalValues(string value)
        {

            try
            {
                if (value != null && value != string.Empty)
                {
                    return Convert.ToDecimal(value);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return null;
            }
        }

        private int? GetNullableIntValues(string value)
        {

            try
            {
                if (value != null && value != string.Empty)
                {
                    return Convert.ToInt32(value);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return null;
            }
        }

        private DateTime? GetNullableDateTimetValues(string value)
        {

            try
            {
                if (value != null && value != string.Empty)
                {
                    return Convert.ToDateTime(value);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return null;
            }
        }

        public DataTable JsonStringToDataTable(string jsonString)
        {
            try
            {
                var dtObject = new DataTable();
                dtObject.Locale = CultureInfo.InvariantCulture;
                var jsonStringArray =
                    Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                var columnsName = new List<string>();
                foreach (string jSA in jsonStringArray)
                {
                    var jsonStringData =
                        Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                    foreach (string ColumnsNameData in jsonStringData)
                    {
                        try
                        {
                            int idx = ColumnsNameData.IndexOf(":");
                            var columnsNameString =
                                ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                            if (!columnsName.Contains(columnsNameString))
                            {
                                columnsName.Add(columnsNameString);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    break;
                }
                foreach (string AddColumnName in columnsName)
                {
                    dtObject.Columns.Add(AddColumnName);
                }
                foreach (string jSA in jsonStringArray)
                {
                    var RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                    var nrObject = dtObject.NewRow();
                    foreach (string rowData in RowData)
                    {
                        try
                        {
                            int idx = rowData.IndexOf(":");
                            var RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                            var RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                            nrObject[RowColumns] = RowDataString;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    dtObject.Rows.Add(nrObject);
                }
                return dtObject;
            }
            catch (Exception)
            {
                new ExceptionLogging().LogException(new ArgumentException(), Constants.UnAuthenticatedUser);
                return null;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        /// <summary>
        /// RemoveLastIndexCharacter
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveLastIndexCharacter(string path)
        {
            if (path.Length > 0)
            {
                int place = path.LastIndexOf(@"\");
                string result = path.Remove(place, (@"\").Length);
                return result;
            }
            else
            {
                return "";
            }
        }
        public string TriggerEffortUpload(DataTable finalData, string EmployeeID)
        {                  
            try
            {
                result = "";
                EffortUploadRespository effresp = new EffortUploadRespository();

                Iscognizant = Convert.ToBoolean(finalData.Rows[0]["IsCognizant"]);
                ProjectID = Convert.ToInt32(finalData.Rows[0]["ProjectID"]);
                TrackID = finalData.Rows[0]["TrackID"].ToString();
                IsEffortTrackActivityWise = Convert.ToBoolean(finalData.Rows[0]["IsEffortTrackActivityWise"]);
                IsDaily = Convert.ToBoolean(finalData.Rows[0]["IsDaily"]);

                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(TrackID), null, null, null, null, null, null, null, null, "TriggerEffortUpload called");

                List<EffortUploadDet> effupdet = new List<EffortUploadDet>();
                if (finalData.Rows.Count > 0)

                {
                    if (Iscognizant)
                    {
                        if (finalData.Columns.Contains("ActivityName") && finalData.Columns.Contains("ServiceName") && finalData.Columns.Contains("Type"))
                        {
                            effupdet = finalData.AsEnumerable().Select(r => new EffortUploadDet
                            {
                                TicketID = finalData.Columns.Contains("ID") ? (r["ID"] == DBNull.Value ? "" : r.Field<string>("ID").Trim()) : "",
                                ServiceName = finalData.Columns.Contains("ServiceName") ? (r["ServiceName"] == DBNull.Value ? "" : r.Field<string>("ServiceName").Trim()) : "",
                                ActivityName = finalData.Columns.Contains("ActivityName") ? (r["ActivityName"] == DBNull.Value ? "" : r.Field<string>("ActivityName").Trim()) : "",
                                HoursCheck = finalData.Columns.Contains("Hours") ? (r["Hours"] == DBNull.Value ? "" : r.Field<string>("Hours").Trim()) : "",
                                IsCognizant = Iscognizant,
                                IsEffortTrackActivityWise = IsEffortTrackActivityWise,
                                ProjectID = ProjectID,
                                CognizantID = finalData.Columns.Contains("CognizantID") ? (r["CognizantID"] == DBNull.Value ? "" : r.Field<string>("CognizantID").Trim()) : "",
                                TrackID = TrackID,
                                IsDaily = IsDaily,
                                TimeSheetDateCheck = finalData.Columns.Contains("Timesheet Date") ? (r["Timesheet Date"] == DBNull.Value ? "" : r.Field<string>("Timesheet Date").Trim()) : "",
                                SuggestedActivity = finalData.Columns.Contains("Suggested Activity") ? (r["Suggested Activity"] == 
                                DBNull.Value ? "" : r.Field<string>("Suggested Activity").Trim()) : "",
                                SuggestedRemarks = finalData.Columns.Contains("Remarks") ? (r["Remarks"] == DBNull.Value ? "" : r.Field<string>("Remarks").Trim()) : "",
                                Type = finalData.Columns.Contains("Type") ? (r["Type"] == DBNull.Value ? "" : r.Field<string>("Type").Trim()) : ""
                            }).ToList();
                            isinfraproject = false;
                        }
                        else
                        {
                            effupdet = finalData.AsEnumerable().Select(r => new EffortUploadDet
                            {
                                TicketID = finalData.Columns.Contains("ID") ? (r["ID"] == DBNull.Value ? "" : r.Field<string>("ID").Trim()) : "",
                                ActivityName = finalData.Columns.Contains("Activity/Task") ? (r["Activity/Task"] == DBNull.Value ? "" : r.Field<string>("Activity/Task").Trim()) : "",
                                HoursCheck = finalData.Columns.Contains("Hours") ? (r["Hours"] == DBNull.Value ? "" : r.Field<string>("Hours").Trim()) : "",
                                IsCognizant = Iscognizant,
                                IsEffortTrackActivityWise = IsEffortTrackActivityWise,
                                ProjectID = ProjectID,
                                CognizantID = finalData.Columns.Contains("CognizantID") ? (r["CognizantID"] == DBNull.Value ? "" : r.Field<string>("CognizantID").Trim()) : "",
                                TrackID = TrackID,
                                IsDaily = IsDaily,
                                TimeSheetDateCheck = finalData.Columns.Contains("Timesheet Date") ? (r["Timesheet Date"] == DBNull.Value ? "" : r.Field<string>("Timesheet Date").Trim()) : "",
                                SuggestedActivity = finalData.Columns.Contains("Suggested Activity") ? (r["Suggested Activity"] == 
                                DBNull.Value ? "" : r.Field<string>("Suggested Activity").Trim()) : "",
                                SuggestedRemarks = finalData.Columns.Contains("Remarks") ? (r["Remarks"] == DBNull.Value ? "" : r.Field<string>("Remarks").Trim()) : "",
                                Type = "I"
                            }).ToList();
                            isinfraproject = true;
                        }
                    }
                    else
                    {
                        effupdet = finalData.AsEnumerable().Select(r => new EffortUploadDet
                        {
                            TicketID = finalData.Columns.Contains("TicketID") ? (r["TicketID"] == DBNull.Value ? "" : r.Field<string>("TicketID").Trim()) : "",
                            TicketType = finalData.Columns.Contains("Ticket Type/Non Delivery Activity") ? (r["Ticket Type/Non Delivery Activity"] == 
                            DBNull.Value ? "" : r.Field<string>("Ticket Type/Non Delivery Activity").Trim()) : "",
                            HoursCheck = finalData.Columns.Contains("Hours") ? (r["Hours"] == DBNull.Value ? "" : r.Field<string>("Hours").Trim()) : "",
                            IsCognizant = Iscognizant,
                            IsEffortTrackActivityWise = IsEffortTrackActivityWise,
                            ProjectID = ProjectID,
                            TrackID = TrackID,
                            CognizantID = finalData.Columns.Contains("User ID") ? (r["User ID"] == DBNull.Value ? "" : r.Field<string>("User ID").Trim()) : "",
                            TimeSheetDateCheck = finalData.Columns.Contains("Timesheet Date") ? (r["Timesheet Date"] == DBNull.Value ? "" : r.Field<string>("Timesheet Date").Trim()) : "",
                            IsDaily = IsDaily,
                            SuggestedActivity = finalData.Columns.Contains("Suggested Activity") ? (r["Suggested Activity"] == DBNull.Value ? "" : r.Field<string>("Suggested Activity").Trim()) : "",
                            SuggestedRemarks = finalData.Columns.Contains("Remarks") ? (r["Remarks"] == DBNull.Value ? "" : r.Field<string>("Remarks").Trim()) : "",
                            Type = finalData.Columns.Contains("Type") ? (r["Type"] == DBNull.Value ? "" : r.Field<string>("Type").Trim()) : "T"

                        }).ToList();
                        isinfraproject = false;

                    }


                    string resultTemp = CheckTemplateFormat(finalData);

                    if (resultTemp == "")
                    {

                        BulkInsertEffortRepo(effupdet, ProjectID, IsDaily, EmployeeID);
                        UpdateTicketIsAttributeFlagRepo(ProjectID);
                    }
                    else
                    {

                        EffortUploadRespository resp = new EffortUploadRespository();

                        if (resultTemp == "duplicate")
                        {
                            result = "Template MisMatch:There is duplicate column in the Template (" + resultTemp + ")";
                            objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                                objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                              , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Template MisMatch:There is duplicate column in the Template (" + resultTemp + ")");


                        }
                        else if (resultTemp == "extra")
                        {
                            result = "Template MisMatch:There is invalid column in the Template ";
                            objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                                objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                             , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Template MisMatch:There is invalid column in the Template ");
                        }
                        else
                        {
                            result = "Upload a valid template in the same format & file name as downloaded";
                            objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                                objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                             , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Template MisMatch:Invalid Template," + resultTemp + "is missing");
                        }
                        resp.TemplateMail(effupdet[0].ProjectID.ToString(), objTrack.EffortUploadDumpFileName, effupdet[0].TrackID, Iscognizant);

                    }
                }
                else
                {
                    result = "There is no data in dump";
                    objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                        objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                            , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "There is no data in dump");
                }

                return result;
            }
            catch (Exception)
            {
               
                return "Error in upload";
            }
        }
        public void BulkInsertEffortRepo(
            List<EffortUploadDet> LstEffortUploadDetails, int ProjectID, bool IsDaily, string EmployeeID)
        {
            bool isCognizant = true;
            EffortUploadRespository effresp = new EffortUploadRespository();
            List<EffortUploadDet> EffErrorList = new List<EffortUploadDet>();

            LstEffortUploadDetails = RemoveEmptyRows(LstEffortUploadDetails);

            List<ErrorExcell> ExcelData = new List<ErrorExcell>();
            try
            {

                if (LstEffortUploadDetails != null && LstEffortUploadDetails.Count > 0)
                {
                    foreach (var final in LstEffortUploadDetails)
                    {
                        if (final.Type.ToUpper().ToUpper().Trim() == "NONDELIVERY" && final.IsCognizant)
                        {
                            final.TicketID = "NONDELIVERY";
                            final.Type = "ND";
                        }
                        if ((isinfraproject || !final.IsCognizant) && final.TicketID.ToUpper().Trim() == "NONDELIVERY")
                        {
                            final.TicketID = "NONDELIVERY";
                            final.Type = "ND";
                        }
                        if (final.Type.Trim() == "Work Item")
                        {
                            final.Type = "W";
                        }
                        if (final.Type.Trim() == "Ticket")
                        {
                            final.Type = "T";
                        }
                        if (!final.IsCognizant)
                        {
                            final.TicketID = final.TicketID.ToUpper().Trim() == "NONDELIVERY" ? final.TicketID.ToUpper().Trim() : final.TicketID;
                        }
                        final.Type = final.Type.ToUpper().Trim();
                        final.HoursCheck = ConvertDecimal(final.HoursCheck);
                        final.ServiceName = final.TicketID == "NONDELIVERY" ? "0" : final.ServiceName;
                        final.SuggestedActivity = (final.TicketID == "NONDELIVERY" && final.SuggestedActivity.Trim().Length >= 50)
                               ? final.SuggestedActivity.Trim().Substring(0, 50) : (final.TicketID != "NONDELIVERY") ? string.Empty : final.SuggestedActivity.Trim();
                        final.SuggestedRemarks = final.SuggestedRemarks != null ? final.SuggestedRemarks.Trim() : final.SuggestedRemarks;


                    }




                    if (LstEffortUploadDetails[0].IsCognizant)
                    {
                        LstEffortUploadDetails = TypeValidation(LstEffortUploadDetails);
                    }

                    //TicketID,Service,....nulll check
                    LstEffortUploadDetails = CheckForNullValues(LstEffortUploadDetails, LstEffortUploadDetails[0].IsCognizant);

                    ///duplicate entryy
                    LstEffortUploadDetails = CheckForDuplicateValues(LstEffortUploadDetails);

                    ///HourCheck
                    LstEffortUploadDetails = CheckHoursNumeric(LstEffortUploadDetails);

                    if (LstEffortUploadDetails[0].IsCognizant)
                    {
                        for (int i = 0; i < LstEffortUploadDetails.Count; i++)
                        {
                            LstEffortUploadDetails[i].Hours =

                                (LstEffortUploadDetails[i].HoursCheck == "null"

                                || (LstEffortUploadDetails[i].Remarks != null ?

                                LstEffortUploadDetails[i].Remarks.Contains("Hours is mandatory and can accept only numeric values from 0.01 to 24.00") : false)) ?

                                Convert.ToDecimal(0.00) : Convert.ToDecimal(LstEffortUploadDetails[i].HoursCheck);



                        }
                    }
                    else
                    {
                        for (int i = 0; i < LstEffortUploadDetails.Count; i++)
                        {
                            LstEffortUploadDetails[i].Hours =

                                (LstEffortUploadDetails[i].HoursCheck == "null"

                                || (LstEffortUploadDetails[i].Remarks != null ?

                                LstEffortUploadDetails[i].Remarks.Contains("Capture numeric values in 'Hours' column") : false)) ?

                                Convert.ToDecimal(0.00) : Convert.ToDecimal(LstEffortUploadDetails[i].HoursCheck);



                        }
                    }
                    LstEffortUploadDetails = CheckDateFormat(LstEffortUploadDetails);
                    if (LstEffortUploadDetails[0].IsCognizant)
                    {
                        for (int i = 0; i < LstEffortUploadDetails.Count; i++)
                        {

                            LstEffortUploadDetails[i].TimeSheetDate = LstEffortUploadDetails[i].TimeSheetDateCheck == "null" ||
                                (LstEffortUploadDetails[i].Remarks != null ?
                                LstEffortUploadDetails[i].Remarks.Contains("Timesheet Date is mandatory and should be in the format MM/DD/YYYY") : false) ? 
                                new DateTime() : Convert.ToDateTime(LstEffortUploadDetails[i].TimeSheetDateCheck);

                        }

                    }
                    else
                    {
                        for (int i = 0; i < LstEffortUploadDetails.Count; i++)
                        {

                            LstEffortUploadDetails[i].TimeSheetDate = LstEffortUploadDetails[i].TimeSheetDateCheck == "null" ||
                                (LstEffortUploadDetails[i].Remarks != null ?
                                LstEffortUploadDetails[i].Remarks.Contains("Timesheet Date should be in MM/dd/yyyy format") : false) ? 
                                    new DateTime() : Convert.ToDateTime(LstEffortUploadDetails[i].TimeSheetDateCheck);

                        }
                    }

                    //different service same date check
                    if (LstEffortUploadDetails[0].IsCognizant && !isinfraproject)
                    {
                        LstEffortUploadDetails = CheckForDifferentServicesBySameDate(LstEffortUploadDetails);
                    }


                    //TimeSheetDateCheck

                    LstEffortUploadDetails = CheckTimesheetDate(LstEffortUploadDetails);

                    LstEffortUploadDetails = CheckTimesheetDateForNonDelivery(LstEffortUploadDetails.ToList());



                    LstEffortUploadDetails = CheckNegativeEfforts(LstEffortUploadDetails);
                    if (LstEffortUploadDetails[0].IsCognizant)
                    {
                        LstEffortUploadDetails = CheckTicketType(LstEffortUploadDetails.ToList());
                    }
                    if (!LstEffortUploadDetails[0].IsCognizant)
                    {
                        isCognizant = LstEffortUploadDetails[0].IsCognizant;
                    }

                    /// Check for timesheet freeze day

                    LstEffortUploadDetails = CheckTimesheetFreezeDays(LstEffortUploadDetails, IsDaily);

                    // SuggestedActivity
                    LstEffortUploadDetails = Specialcharacter(LstEffortUploadDetails);

                    LstEffortUploadDetails = SuggestedActivitycheck(LstEffortUploadDetails);

                    LstEffortUploadDetails = SuggestedRemarkscheck(LstEffortUploadDetails);
                    //Filitering Error List
                    EffFinaldata = LstEffortUploadDetails.Where(e => e.Remarks == null).ToList();

                    EffErrorList = LstEffortUploadDetails.Where(e => e.Remarks != null).ToList();


                    /// Sp call to perform validation and insertion

                    EffortUploadResultGrid efferrorList = effresp.InsertEffortBulkUploadDetails(EffFinaldata, EffErrorList, objTrack, EmployeeID);


                    if (efferrorList != null)
                    {
                        effresp.GendrateErrorExcell(efferrorList, ProjectID, isCognizant, isinfraproject);
                    }
                    result = efferrorList.Status;
                }
                else
                {

                    result = "Upload failed.Template should not be empty";

                }
            }
            catch (Exception ex)
            {
                result = "Error in upload";
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                    , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At BulkInsertEffort Metod || Message: " + ex.Message);

            }
        }
        private List<EffortUploadDet> CheckHoursNumeric(List<EffortUploadDet> LstEffortUploadDetails)
        {

            EffortUploadRespository effresp = new EffortUploadRespository();

            List<EffortUploadDet> LstInvalidHoursFilterData = new List<EffortUploadDet>();
            List<EffortUploadDet> LstInvalidHoursFilterDatavalue = new List<EffortUploadDet>();
            try
            {
                decimal n;
                LstInvalidHoursFilterData = LstEffortUploadDetails.Where(e => !Decimal.TryParse(e.HoursCheck, out n)).ToList();

                if (LstEffortUploadDetails.Count > 0)
                {
                    if (!LstEffortUploadDetails[0].IsCognizant)
                    {
                        UpdateRemarks(LstInvalidHoursFilterData, "Capture numeric values in 'Hours' column");
                        LstInvalidHoursFilterDatavalue = LstEffortUploadDetails.Where(e => e.Remarks == null
                                                         || !e.Remarks.Contains("Capture numeric values in 'Hours' column")).ToList();
                        UpdateRemarks(LstInvalidHoursFilterDatavalue.Where(e => Math.Round((Double)Convert.ToDecimal(e.HoursCheck), 2) <= 0).ToList(), 
                            "Hours is mandatory and can accept only numeric values from 0.01 to 24.00");
                    }
                    else
                    {
                        UpdateRemarks(LstInvalidHoursFilterData, "Hours is mandatory and can accept only numeric values from 0.01 to 24.00");
                        LstInvalidHoursFilterDatavalue = LstEffortUploadDetails.Where(e => e.Remarks == null
                                                        || !e.Remarks.Contains("Hours is mandatory and can accept only numeric values from 0.01 to 24.00")).ToList();
                        UpdateRemarks(LstInvalidHoursFilterDatavalue.Where(e => Math.Round((Double)Convert.ToDecimal(e.HoursCheck), 2) <= 0).ToList(), 
                            "Hours is mandatory and can accept only numeric values from 0.01 to 24.00");
                    }
                }
            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                   , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CheckHoursNumeric Metod || Message: " + ex.Message);
                throw ex;
            }
            return LstEffortUploadDetails;


        }
        private List<EffortUploadDet> CheckDateFormat(List<EffortUploadDet> LstEffortUploadDetails)
        {

            EffortUploadRespository effresp = new EffortUploadRespository();

            List<EffortUploadDet> LstInvalidDateFilterData = new List<EffortUploadDet>();
            try
            {
                DateTime n;
                LstInvalidDateFilterData = LstEffortUploadDetails.Where(e => !DateTime.TryParse(e.TimeSheetDateCheck, out n)).ToList();
                if (LstEffortUploadDetails.Count > 0)
                {
                    if (!LstEffortUploadDetails[0].IsCognizant)
                    {
                        UpdateRemarks(LstInvalidDateFilterData, "Timesheet Date should be in MM/dd/yyyy format");
                    }
                    else
                    {
                        UpdateRemarks(LstInvalidDateFilterData, "Timesheet Date is mandatory and should be in the format MM/DD/YYYY");
                    }
                }
            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                   , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CheckHoursNumeric Metod || Message: " + ex.Message);
                throw ex;
            }
            return LstEffortUploadDetails;


        }
        private List<EffortUploadDet> CheckForDifferentServicesBySameDate(List<EffortUploadDet> LstEffortUploadDetails)
        {

            List<string> ticketiddup = new List<string>();
            List<EffortUploadDet> LstDuplicate = new List<EffortUploadDet>();
            List<EffortUploadDet> LstDuplicateFilterData = new List<EffortUploadDet>();
            EffortUploadRespository effresp = new EffortUploadRespository();

            try
            {
                var tckt = LstEffortUploadDetails.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, 
                    TimeSheetDate = x.TimeSheetDate }).GroupBy(x => new { x.TicketID, x.CognizantID, 
                        x.TimeSheetDate }).Where(grp => grp.Select(x => x.ServiceName).Distinct().Count() > 1).ToList();

                List<effdup> newtckt = new List<effdup>();
                for (int i = 0; i < tckt.Count; i++)
                {
                    if (tckt[i].Key.TicketID != null && tckt[i].Key.TicketID != "" && tckt[i].Key.TicketID != "null")
                    {
                        effdup effdup1 = new effdup();
                        effdup1.TicketID = tckt[i].Key.TicketID;
                        effdup1.TimeSheetDate = tckt[i].Key.TimeSheetDate;
                        effdup1.CognizantID = tckt[i].Key.CognizantID;
                        newtckt.Add(effdup1);
                    }

                }
                ticketiddup = newtckt.Select(x => x.TicketID).ToList();
                LstDuplicate = (from post in LstEffortUploadDetails
                                join meta in newtckt on new { a = post.TicketID, post.ServiceName, post.ActivityName, post.CognizantID, post.TimeSheetDate } equals new { 
                                    a = meta.TicketID, meta.ServiceName, meta.ActivityName, meta.CognizantID, meta.TimeSheetDate }


                                select new EffortUploadDet
                                {
                                    TicketID = post.TicketID,
                                    ServiceName = post.ServiceName,
                                    ActivityName = post.ActivityName,
                                    CognizantID = post.CognizantID,
                                    TimeSheetDate = post.TimeSheetDate,
                                    IsCognizant = post.IsCognizant
                                ,
                                    IsEffortTrackActivityWise = post.IsEffortTrackActivityWise,
                                    ProjectID = post.ProjectID,
                                    Hours = post.Hours
                                }).ToList();

                UpdateRemarks(LstDuplicate, "TicketID Contains DifferentServices By SameDate");
                List<effdup> lstdup = LstDuplicate.Select(x => new effdup { TicketID = x.TicketID, TimeSheetDate = x.TimeSheetDate, ServiceName = x.ServiceName, 
                    ActivityName = x.ActivityName, CognizantID = x.CognizantID, Remarks = x.Remarks }).Distinct().ToList();
                
                foreach (var x in lstdup.Distinct().ToList())
                {

                    (from EffortData in LstEffortUploadDetails
                     where (EffortData.TicketID == x.TicketID) && (EffortData.ServiceName == x.ServiceName) && (EffortData.ActivityName == x.ActivityName) && 
                     (EffortData.CognizantID == x.CognizantID) && (EffortData.TimeSheetDate == x.TimeSheetDate) && (EffortData.Remarks != "TicketID Contains DifferentServices By SameDate")
                     select EffortData).ToList().ForEach((EffortData) =>
                     {
                         EffortData.Remarks = (EffortData.Remarks == null ? x.Remarks : EffortData.Remarks + "," + x.Remarks);
                     });

                }

            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                   , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CheckForDifferentServicesBySameDate Metod || Message: " + ex.Message);
                throw ex;
            }
            return LstEffortUploadDetails;

        }
        private List<EffortUploadDet> CheckForDuplicateValues(List<EffortUploadDet> LstEffortUploadDetails)
        {


            EffortUploadRespository effresp = new EffortUploadRespository();

            List<string> ticketiddup = new List<string>();
            List<EffortUploadDet> LstTickets = new List<EffortUploadDet>();
            List<EffortUploadDet> LstNonDeliveryTickets = new List<EffortUploadDet>();
            List<EffortUploadDet> LstDuplicate = new List<EffortUploadDet>();
            List<EffortUploadDet> LstDuplicateFilterData = new List<EffortUploadDet>();
            List<effdup> newtckt = new List<effdup>();
            try
            {



                LstTickets = LstEffortUploadDetails.AsEnumerable().Where(x => x.TicketID != "NONDELIVERY").ToList();

                LstNonDeliveryTickets = LstEffortUploadDetails.AsEnumerable().Where(x => x.TicketID == "NONDELIVERY").ToList();



                if (!LstEffortUploadDetails[0].IsCognizant)
                {

                    var tckt = LstTickets.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                        TicketType = x.TicketType }).GroupBy(x => new { x.TicketID, x.CognizantID, x.TimeSheetDateCheck, x.TicketType }).Where(grp => grp.Count() > 1).ToList();


                    newtckt = new List<effdup>();
                    for (int i = 0; i < tckt.Count; i++)
                    {
                        effdup effdup1 = new effdup();
                        if (tckt[i].Key.TicketID != null && tckt[i].Key.TicketID != "" && tckt[i].Key.TicketID != "null")
                        {
                            effdup1.TicketID = tckt[i].Key.TicketID;
                            effdup1.TicketType = tckt[i].Key.TicketType;
                            effdup1.TimeSheetDateCheck = tckt[i].Key.TimeSheetDateCheck;
                            effdup1.CognizantID = tckt[i].Key.CognizantID;
                            newtckt.Add(effdup1);
                        }

                    }
                    //NON Delivery
                    if (LstNonDeliveryTickets != null && LstNonDeliveryTickets.Any())
                    {

                        var Nontckt = LstNonDeliveryTickets.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            TicketType = x.TicketType }).GroupBy(x => new { x.TicketID, x.CognizantID, x.TimeSheetDateCheck, x.TicketType }).Where(grp => grp.Count() > 1).ToList();

                        for (int i = 0; i < Nontckt.Count; i++)
                        {
                            effdup effdup1 = new effdup();
                            if (Nontckt[i].Key.TicketID != null && Nontckt[i].Key.TicketID != "" && Nontckt[i].Key.TicketID != "null")
                            {
                                effdup1.Type = "ND";
                                effdup1.TicketID = Nontckt[i].Key.TicketID;
                                effdup1.TicketType = Nontckt[i].Key.TicketType;
                                effdup1.TimeSheetDateCheck = Nontckt[i].Key.TimeSheetDateCheck;
                                effdup1.CognizantID = Nontckt[i].Key.CognizantID;
                                newtckt.Add(effdup1);
                            }

                        }
                    }



                }
                else
                {

                    if (!isinfraproject)
                    {
                        var tckt = LstTickets.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, ServiceName = x.ServiceName, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            ActivityName = x.ActivityName }).GroupBy(x => new { x.TicketID, x.CognizantID, x.ServiceName, x.TimeSheetDateCheck, x.ActivityName }).Where(grp => grp.Count() > 1).ToList();


                        newtckt = new List<effdup>();
                        for (int i = 0; i < tckt.Count; i++)
                        {
                            effdup effdup1 = new effdup();
                            if (tckt[i].Key.TicketID != null && tckt[i].Key.TicketID != "" && tckt[i].Key.TicketID != "null")
                            {
                                effdup1.Type = "T";
                                effdup1.TicketID = tckt[i].Key.TicketID;
                                effdup1.ServiceName = tckt[i].Key.ServiceName;
                                effdup1.ActivityName = tckt[i].Key.ActivityName;
                                effdup1.TimeSheetDateCheck = tckt[i].Key.TimeSheetDateCheck;
                                effdup1.CognizantID = tckt[i].Key.CognizantID;
                                newtckt.Add(effdup1);
                            }

                        }
                    }
                    else
                    {
                        var tckt = LstTickets.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            ActivityName = x.ActivityName }).GroupBy(x => new { x.TicketID, x.CognizantID, x.TimeSheetDateCheck, x.ActivityName }).Where(grp => grp.Count() > 1).ToList();


                        newtckt = new List<effdup>();
                        for (int i = 0; i < tckt.Count; i++)
                        {
                            effdup effdup1 = new effdup();
                            if (tckt[i].Key.TicketID != null && tckt[i].Key.TicketID != "" && tckt[i].Key.TicketID != "null")
                            {
                                effdup1.Type = "I";
                                effdup1.TicketID = tckt[i].Key.TicketID;
                                effdup1.ServiceName = "0";
                                effdup1.ActivityName = tckt[i].Key.ActivityName;
                                effdup1.TimeSheetDateCheck = tckt[i].Key.TimeSheetDateCheck;
                                effdup1.CognizantID = tckt[i].Key.CognizantID;
                                newtckt.Add(effdup1);
                            }

                        }
                    }

                    //NON Delivery
                    if (LstNonDeliveryTickets != null && LstNonDeliveryTickets.Any())
                    {
                        var Nontckt = LstNonDeliveryTickets.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID, CognizantID = x.CognizantID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            ActivityName = x.ActivityName }).GroupBy(x => new { x.TicketID, x.CognizantID, x.TimeSheetDateCheck, x.ActivityName }).Where(grp => grp.Count() > 1).ToList();

                        for (int i = 0; i < Nontckt.Count; i++)
                        {
                            effdup effdup1 = new effdup();
                            if (Nontckt[i].Key.TicketID != null && Nontckt[i].Key.TicketID != "" && Nontckt[i].Key.TicketID != "null")
                            {
                                effdup1.Type = "ND";
                                effdup1.TicketID = Nontckt[i].Key.TicketID;
                                effdup1.ServiceName = "0";
                                effdup1.ActivityName = Nontckt[i].Key.ActivityName;
                                effdup1.TimeSheetDateCheck = Nontckt[i].Key.TimeSheetDateCheck;
                                effdup1.CognizantID = Nontckt[i].Key.CognizantID;
                                newtckt.Add(effdup1);
                            }

                        }
                    }

                    ticketiddup = newtckt.Select(x => x.TicketID).ToList();

                }

                if (!LstEffortUploadDetails[0].IsCognizant)
                {
                    LstDuplicate = (from post in LstEffortUploadDetails
                                    join meta in newtckt on new
                                    {
                                        a = post.Type,
                                        post.TicketID,
                                        post.TicketType,
                                        post.CognizantID,
                                        post.TimeSheetDateCheck
                                    } equals new { a = meta.Type, meta.TicketID, meta.TicketType, meta.CognizantID, meta.TimeSheetDateCheck }

                                    select new EffortUploadDet
                                    {
                                        Type = post.Type,
                                        TicketID = post.TicketID,
                                        TicketType = post.TicketType,
                                        CognizantID = post.CognizantID,
                                        TimeSheetDateCheck = post.TimeSheetDateCheck,
                                        IsCognizant = post.IsCognizant,
                                        IsEffortTrackActivityWise = post.IsEffortTrackActivityWise,
                                        ProjectID = post.ProjectID,
                                        Hours = post.Hours
                                    }).ToList();
                }
                else
                {
                    if (!isinfraproject)
                    {
                        LstDuplicate = (from post in LstEffortUploadDetails
                                        join meta in newtckt on new
                                        {
                                            a = post.Type,
                                            post.TicketID,
                                            post.ServiceName,
                                            post.ActivityName,
                                            post.CognizantID,
                                            post.TimeSheetDateCheck
                                        } equals new { a = meta.Type, meta.TicketID, meta.ServiceName, meta.ActivityName, meta.CognizantID, meta.TimeSheetDateCheck }

                                        select new EffortUploadDet
                                        {
                                            Type = post.Type,
                                            TicketID = post.TicketID,
                                            ServiceName = post.ServiceName,
                                            ActivityName = post.ActivityName,
                                            CognizantID = post.CognizantID,
                                            TimeSheetDateCheck = post.TimeSheetDateCheck,
                                            IsCognizant = post.IsCognizant,
                                            IsEffortTrackActivityWise = post.IsEffortTrackActivityWise,
                                            ProjectID = post.ProjectID,
                                            Hours = post.Hours
                                        }).ToList();
                    }
                    else
                    {
                        LstDuplicate = (from post in LstEffortUploadDetails
                                        join meta in newtckt on new
                                        {
                                            a = post.Type,
                                            post.TicketID,
                                            post.ActivityName,
                                            post.CognizantID,
                                            post.TimeSheetDateCheck
                                        } equals new { a = meta.Type, meta.TicketID, meta.ActivityName, meta.CognizantID, meta.TimeSheetDateCheck }

                                        select new EffortUploadDet
                                        {
                                            Type = post.Type,
                                            TicketID = post.TicketID,
                                            ActivityName = post.ActivityName,
                                            CognizantID = post.CognizantID,
                                            TimeSheetDateCheck = post.TimeSheetDateCheck,
                                            IsCognizant = post.IsCognizant,
                                            IsEffortTrackActivityWise = post.IsEffortTrackActivityWise,
                                            ProjectID = post.ProjectID,
                                            Hours = post.Hours
                                        }).ToList();
                    }

                }

                if (!LstEffortUploadDetails[0].IsCognizant)
                {
                    UpdateRemarks(LstDuplicate.Where(X => X.TicketID != "NONDELIVERY").ToList(), Constants.TicketIDDuplicate);
                    UpdateRemarks(LstDuplicate.Where(X => X.TicketID == "NONDELIVERY").Distinct().ToList(), Constants.duplicateEntry);
                }
                else
                {
                    UpdateRemarks(LstDuplicate.Where(X => X.TicketID != "NONDELIVERY" && isinfraproject).ToList(), Constants.DuplicaterecordsInfra);
                    UpdateRemarks(LstDuplicate.Where(X => X.TicketID != "NONDELIVERY" && !isinfraproject).ToList(), Constants.Duplicaterecords);
                    UpdateRemarks(LstDuplicate.Where(X => X.TicketID == "NONDELIVERY").Distinct().ToList(), Constants.Duplicaterecords);
                }
                ////.Distinct().ToList();
                if (!LstEffortUploadDetails[0].IsCognizant)
                {

                    List<effdup> lstdup = LstDuplicate.Select(x => new effdup { Type = x.Type, TicketID = x.TicketID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                        TicketType = x.TicketType, CognizantID = x.CognizantID, Remarks = x.Remarks }).Distinct().ToList();

                    foreach (var x in lstdup.Distinct().ToList())
                    {

                        (from EffortData in LstEffortUploadDetails
                         where (EffortData.TicketID == x.TicketID) && (EffortData.TicketType == x.TicketType) && 
                         (EffortData.CognizantID == x.CognizantID) && (EffortData.TimeSheetDateCheck == x.TimeSheetDateCheck)
                         && ((EffortData.Remarks != Constants.TicketIDDuplicate) && (EffortData.Remarks != Constants.duplicateEntry))
                         select EffortData).ToList().ForEach((EffortData) =>
                         {
                             EffortData.Remarks = (EffortData.Remarks == null ? x.Remarks : EffortData.Remarks + "," + x.Remarks);
                         });

                    }

                }
                else
                {

                    if (!isinfraproject)

                    {
                        List<effdup> lstdup = LstDuplicate.Select(x => new effdup { Type = x.Type, TicketID = x.TicketID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            ServiceName = x.ServiceName, ActivityName = x.ActivityName, CognizantID = x.CognizantID, Remarks = x.Remarks }).Distinct().ToList();

                        foreach (var x in lstdup.Distinct().ToList())
                        {

                            (from EffortData in LstEffortUploadDetails
                             where (EffortData.Type == x.Type) && (EffortData.TicketID == x.TicketID) && (EffortData.ServiceName == x.ServiceName) && 
                             (EffortData.ActivityName == x.ActivityName) && (EffortData.CognizantID == x.CognizantID) && (EffortData.TimeSheetDateCheck == x.TimeSheetDateCheck)
                             && (EffortData.Remarks == null || !EffortData.Remarks.Contains(Constants.Duplicaterecords))
                             select EffortData).ToList().ForEach((EffortData) =>
                             {
                                 EffortData.Remarks = (EffortData.Remarks == null ? x.Remarks : EffortData.Remarks + "," + x.Remarks);
                             });

                        }
                    }
                    else
                    {
                        List<effdup> lstdup = LstDuplicate.Select(x => new effdup { Type = x.Type, TicketID = x.TicketID, TimeSheetDateCheck = x.TimeSheetDateCheck, 
                            ActivityName = x.ActivityName, CognizantID = x.CognizantID, Remarks = x.Remarks }).Distinct().ToList();

                        foreach (var x in lstdup.Distinct().ToList())
                        {

                            (from EffortData in LstEffortUploadDetails
                             where (EffortData.TicketID == x.TicketID) && (EffortData.ActivityName == x.ActivityName) && (EffortData.CognizantID == x.CognizantID) && 
                             (EffortData.TimeSheetDateCheck == x.TimeSheetDateCheck)
                             && (EffortData.Remarks == null || !EffortData.Remarks.Contains(Constants.DuplicaterecordsInfra))
                             select EffortData).ToList().ForEach((EffortData) =>
                             {
                                 EffortData.Remarks = (EffortData.Remarks == null ? x.Remarks : EffortData.Remarks.Contains(x.Remarks) ? EffortData.Remarks : EffortData.Remarks + "," + x.Remarks);
                             });

                        }
                    }

                }


            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CheckForDuplicateValues Metod || Message: " + ex.Message);
                throw ex;
            }
            return LstEffortUploadDetails;

        }
        private List<EffortUploadDet> CheckForNullValues(List<EffortUploadDet> LstEffortUploadDetails, bool IsCognizant)
        {

            EffortUploadRespository effresp = new EffortUploadRespository();
            var tickectid = LstEffortUploadDetails[0].TicketID;
            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            try

            {
                //Select the Null value Tickets

                CategoresNULLError(LstEffortUploadDetails, IsCognizant);

            }

            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CheckForNullValues Metod || Message: " + ex.Message);
                throw ex;

            }

            return LstEffortUploadDetails;
        }
        public void CategoresNULLError(List<EffortUploadDet> LstEffortUploadDetails, bool IsCognizant)
        {
            // CheckForNullValues
            EffortUploadRespository effresp = new EffortUploadRespository();
            try
            {
                if (LstEffortUploadDetails != null && LstEffortUploadDetails.Count > 0)
                {

                    List<EffortUploadDet> TicketIDNull = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.TicketID == null || e.TicketID.Trim() == "" || e.TicketID == "null").ToList());

                    UpdateRemarks(TicketIDNull, "ID is mandatory and it should already been uploaded in Work Profiler");

                    if (IsCognizant)
                    {
                        List<EffortUploadDet> CognizantIDNull = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.CognizantID == null || 
                        e.CognizantID.Trim() == "" || e.CognizantID == "null").ToList());

                        UpdateRemarks(CognizantIDNull, "Cognizant ID is mandatory and the user should be part of User Management for the respective project");
                    }
                    else
                    {
                        List<EffortUploadDet> UserIDNull = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.CognizantID == null || 
                        e.CognizantID.Trim() == "" || e.CognizantID == "null").ToList());

                        UpdateRemarks(UserIDNull, "User Not Configured for that ProjectID");
                    }

                    if (!isinfraproject)
                    {
                        List<EffortUploadDet> ServiceNull = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.IsCognizant && 
                        (e.ServiceName == null || e.ServiceName == "null" || e.ServiceName.Trim() == "" && e.TicketID != "NONDELIVERY")).ToList());

                        UpdateRemarks(ServiceNull, "Service Name is mandatory and should have only 1 of the values configured in AppLens ");
                    }
                    List<EffortUploadDet> ActivityNull = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.IsCognizant && e.IsEffortTrackActivityWise && 
                    (e.ActivityName == null || e.ActivityName == "null" || e.ActivityName.Trim() == "" && e.TicketID == "NONDELIVERY")).ToList());

                    UpdateRemarks(ActivityNull, "For NonDelivery, the activity should be from the following list: 'Leave/Holiday, " +
                        "Organization Activity, Meeting, Idle, Team building, Training, Comp Off and Others'");

                    List<EffortUploadDet> ActivityforTicket = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.IsCognizant && e.IsEffortTrackActivityWise && 
                    (e.ActivityName == null || e.ActivityName == "null" || e.ActivityName.Trim() == "" && e.TicketID != "NONDELIVERY")).ToList());

                    UpdateRemarks(ActivityforTicket, "Activity Name is mandatory and should have only 1 of the project specific values configured in " +
                        "AppLens corresponding to the captured Service Name");

                    List<EffortUploadDet> CustomerTicketTypeCheck = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => (!e.IsCognizant && 
                    e.TicketType == null || e.TicketType == "null" || e.TicketType == "")).ToList());

                    UpdateRemarks(CustomerTicketTypeCheck, "TicketType Should not Empty");

                    List<EffortUploadDet> ProjectIDZero = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.ProjectID == 0).ToList());

                    UpdateRemarks(ProjectIDZero, "ProjectID Should not be Zero");

                    List<EffortUploadDet> TimesheetDate = new List<EffortUploadDet>(LstEffortUploadDetails.Where(e => e.TimeSheetDate == null).ToList());

                    UpdateRemarks(TimesheetDate, "Timesheet Date is mandatory and should be in the format MM/DD/YYYY");

                }
            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At CategoresNULLError Metod || Message: " + ex.Message);
            }
        }
        public void UpdateRemarks(List<EffortUploadDet> EffErrorLogTemp, string Message)
        {
            EffortUploadRespository effresp = new EffortUploadRespository();

            try
            {
                if (EffErrorLogTemp != null && EffErrorLogTemp.Count > 0)
                {
                    (from EffortData in EffErrorLogTemp
                     select EffortData).ToList().ForEach((EffortData) =>
                     {
                         EffortData.Remarks = (EffortData.Remarks == null ? Message : EffortData.Remarks + "," + Message);
                         EffortData.ActivityName = (EffortData.TicketID == "NONDELIVERY" && EffortData.IsCognizant == false
                                            ? EffortData.TicketType : EffortData.ActivityName);
                         EffortData.TicketType = EffortData.TicketID == "NONDELIVERY" && EffortData.IsCognizant == false
                                            ? "" : EffortData.TicketType;
                     });

                }
            }
            catch (Exception ex)
            {
                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt16(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
              , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: At UpdateRemarks Metod || Message: " + ex.Message);

            }
        }
        private string CheckTemplateFormat(DataTable dtEffortUpload)
        {

            bool IsCognizant = Convert.ToBoolean(dtEffortUpload.Rows[0]["IsCognizant"]);
            string columnindex = "";
            try

            {
                if (IsCognizant)
                {
                    if (!isinfraproject)
                    {

                        DataTable dtTest = new DataTable();
                        dtTest.Locale = CultureInfo.InvariantCulture;
                        columnindex = dtEffortUpload.Columns.Contains("ID") ? (dtEffortUpload.Columns.Contains("ServiceName") ? (dtEffortUpload.Columns.Contains("ActivityName") ? 
                            (dtEffortUpload.Columns.Contains("Hours") ? (dtEffortUpload.Columns.Contains("CognizantID") ? (dtEffortUpload.Columns.Contains("Timesheet Date") ? 
                            (dtEffortUpload.Columns.Contains("Suggested Activity") ? (dtEffortUpload.Columns.Contains("Remarks") ? (dtEffortUpload.Columns.Contains("Type") ? "" : 
                            "Type") : "Remarks") : "Suggested Activity") : "Timesheet Date") : "CognizantID") : "Hours") : "ActivityName") : "ServiceName") : "ID";

                        if (columnindex == "")
                        {
                            dtTest = dtEffortUpload;
                            dtTest.Columns.Remove("ID");
                            dtTest.Columns.Remove("Type");
                            dtTest.Columns.Remove("ServiceName");
                            dtTest.Columns.Remove("ActivityName");
                            dtTest.Columns.Remove("Hours");
                            dtTest.Columns.Remove("Timesheet Date");
                            dtTest.Columns.Remove("CognizantID");
                            dtTest.Columns.Remove("IsCognizant");
                            dtTest.Columns.Remove("IsEffortTrackActivityWise");
                            dtTest.Columns.Remove("ProjectID");
                            dtTest.Columns.Remove("TrackID");
                            dtTest.Columns.Remove("IsDaily");
                            dtTest.Columns.Remove("Suggested Activity");
                            dtTest.Columns.Remove("Remarks");

                            //check the checkin process

                            if (dtTest.Columns.Count > 0)
                            {
                                bool x1 = dtTest.Columns.Contains("ID") || dtTest.Columns.Contains("ServiceName");
                                bool x2 = dtTest.Columns.Contains("ActivityName")
                                    || dtTest.Columns.Contains("Hours")
                                    || dtTest.Columns.Contains("CognizantID") 
                                    || dtTest.Columns.Contains("Timesheet Date");
                                bool x3 = dtTest.Columns.Contains("Suggested Activity")
                                    || dtTest.Columns.Contains("Remarks") 
                                    || dtTest.Columns.Contains("Remarks");
                                if ( x1 || x2 || x3 )
                                {
                                    columnindex = "duplicate";

                                }
                                else
                                {
                                    columnindex = "extra";
                                }

                            }

                        }

                    }
                    else
                    {
                        DataTable dtTest = new DataTable();
                        dtTest.Locale = CultureInfo.InvariantCulture;
                        columnindex = dtEffortUpload.Columns.Contains("ID") ? (dtEffortUpload.Columns.Contains("Activity/Task") ? 
                            (dtEffortUpload.Columns.Contains("Hours") ? (dtEffortUpload.Columns.Contains("CognizantID") ? (dtEffortUpload.Columns.Contains("Timesheet Date") ? 
                            (dtEffortUpload.Columns.Contains("Suggested Activity") ? (dtEffortUpload.Columns.Contains("Remarks") ? "" : 
                            "Remarks") : "Suggested Activity") : "Timesheet Date") : "CognizantID") : "Hours") : "Activity/Task") : "ID";

                        if (columnindex == "")
                        {
                            dtTest = dtEffortUpload;
                            dtTest.Columns.Remove("ID");
                            dtTest.Columns.Remove("Activity/Task");
                            dtTest.Columns.Remove("Hours");
                            dtTest.Columns.Remove("Timesheet Date");
                            dtTest.Columns.Remove("CognizantID");
                            dtTest.Columns.Remove("IsCognizant");
                            dtTest.Columns.Remove("IsEffortTrackActivityWise");
                            dtTest.Columns.Remove("ProjectID");
                            dtTest.Columns.Remove("TrackID");
                            dtTest.Columns.Remove("IsDaily");
                            dtTest.Columns.Remove("Suggested Activity");
                            dtTest.Columns.Remove("Remarks");


                            if (dtTest.Columns.Count > 0)
                            {
                                bool y1 = dtTest.Columns.Contains("Activity/Task") || dtTest.Columns.Contains("Hours") || dtTest.Columns.Contains("CognizantID") ||
                                    dtTest.Columns.Contains("Timesheet Date");
                                if (dtTest.Columns.Contains("ID") ||
                                    y1 || dtTest.Columns.Contains("Suggested Activity") || dtTest.Columns.Contains("Remarks"))
                                {
                                    columnindex = "duplicate";

                                }
                                else
                                {
                                    columnindex = "extra";
                                }

                            }

                        }

                    }
                }
                else
                {
                    DataTable dtTest = new DataTable();
                    dtTest.Locale = CultureInfo.InvariantCulture;
                    columnindex = dtEffortUpload.Columns.Contains("TicketID") ? (dtEffortUpload.Columns.Contains
                        ("Ticket Type/Non Delivery Activity") ? (dtEffortUpload.Columns.Contains("Hours") ? (dtEffortUpload.Columns.Contains
                        ("User ID") ? (dtEffortUpload.Columns.Contains("Timesheet Date") ? (dtEffortUpload.Columns.Contains("Suggested Activity") ? 
                        (dtEffortUpload.Columns.Contains("Remarks") ? "" : "Remarks") : "Suggested Activity") : 
                        "Timesheet Date") : "User ID") : "Hours") : "Ticket Type/Non Delivery Activity") : "TicketID";

                    if (columnindex == "")
                    {
                        dtTest = dtEffortUpload;
                        dtTest.Columns.Remove("TicketID");
                        dtTest.Columns.Remove("Ticket Type/Non Delivery Activity");
                        dtTest.Columns.Remove("Hours");
                        dtTest.Columns.Remove("Timesheet Date");
                        dtTest.Columns.Remove("IsCognizant");
                        dtTest.Columns.Remove("User ID");
                        dtTest.Columns.Remove("ProjectID");
                        dtTest.Columns.Remove("IsEffortTrackActivityWise");
                        dtTest.Columns.Remove("TrackID");
                        dtTest.Columns.Remove("IsDaily");
                        dtTest.Columns.Remove("Suggested Activity");
                        dtTest.Columns.Remove("Remarks");

                        if (dtTest.Columns.Count > 0)
                        {
                            bool y2 = dtTest.Columns.Contains("Ticket Type/Non Delivery Activity") ||
                                dtTest.Columns.Contains("Hours") || dtTest.Columns.Contains("CognizantID") || dtTest.Columns.Contains("Timesheet Date");
                            if (dtTest.Columns.Contains("TicketID") || y2 ||
                                dtTest.Columns.Contains("Suggested Activity") || dtTest.Columns.Contains("Remarks"))
                            {
                                columnindex = "duplicate";

                            }
                            else
                            {
                                columnindex = "extra";
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return columnindex;
        }
        private List<EffortUploadDet> CheckTimesheetDate(List<EffortUploadDet> LstEffortUploadDetails1)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            List<EffortUploadDet> LstFilteredEffortNonDelivery = new List<EffortUploadDet>();
            try
            {

                LstFilteredEffortData = LstEffortUploadDetails1.Where(e => e.TimeSheetDate > Convert.ToDateTime(DateTimeOffset.Now.DateTime.Date.ToString("MM/dd/yyyy"))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            UpdateRemarks(LstFilteredEffortData.Where(e => e.TicketID != "NONDELIVERY").ToList(), Constants.Timesheetfuturedates);

            return LstEffortUploadDetails1;
        }
        private List<EffortUploadDet> CheckTimesheetDateForNonDelivery(List<EffortUploadDet> LstEffortUploadDetails1)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            List<EffortUploadDet> LstFilteredEffortNonDelivery = new List<EffortUploadDet>();
            try
            {

                LstFilteredEffortData = LstEffortUploadDetails1.Where(e => e.TimeSheetDate > Convert.ToDateTime(DateTimeOffset.Now.DateTime.Date.ToString("MM/dd/yyyy"))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (LstFilteredEffortData.Count() > 0)
            {
                if (!LstFilteredEffortData[0].IsCognizant)
                {
                    UpdateRemarks(LstFilteredEffortData.Where(e => e.TicketID == "NONDELIVERY" && e.TicketType.Trim().ToUpper() != "LEAVE/HOLIDAY").ToList(), Constants.Timesheetfuturedates);
                }
                else
                {
                    UpdateRemarks(LstFilteredEffortData.Where(e => e.TicketID == "NONDELIVERY" && e.ActivityName.Trim().ToUpper() != "LEAVE/HOLIDAY").ToList(), Constants.Timesheetfuturedates);

                }
            }
            return LstEffortUploadDetails1;
        }
        private List<EffortUploadDet> CheckTimesheetFreezeDays(List<EffortUploadDet> LstEffortUploadDetails1, bool IsDaily)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            List<EffortUploadDet> LstFilteredEffortNonDelivery = new List<EffortUploadDet>();
            int TimesheetFreezeDay = Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["TimesheetFreezeDay"]);
            int EnableTimesheetMonthFreeze = Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["EnableTimesheetMonthFreeze"]);
            int TimesheerFreezeMonthCount = EnableTimesheetMonthFreeze == 0 ? 1 :
                                            Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["TimesheetFreezeMonthCount"]);
            int TodayDate = DateTimeOffset.Now.DateTime.Day;
            DateTimeOffset StartDate = DateTimeOffset.Now.DateTime;
            DateTimeOffset EndDate = DateTimeOffset.Now.DateTime;
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            DateTimeOffset now = DateTimeOffset.Now.DateTime;
            var thisMonthStart = today.AddDays(1 - today.Day);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            DateTime TimesheetStartDate = Convert.ToDateTime("01/01/0001 12:00:00 AM");
            try
            {
                if (IsDaily == true)
                {
                    if (TodayDate <= TimesheetFreezeDay)
                    {
                        StartDate = month.AddMonths(-TimesheerFreezeMonthCount);
                        EndDate = today;
                    }
                    else if (TodayDate > TimesheetFreezeDay)
                    {
                        StartDate = new DateTime(now.Year, now.Month, 1);
                        EndDate = today;
                    }
                    else
                    {
                        //CCAP Fix
                    }
                }
                else if (IsDaily == false)
                {
                    if (TodayDate <= TimesheetFreezeDay)
                    {
                        StartDate = month.AddMonths(-TimesheerFreezeMonthCount);
                        StartDate = StartDate.AddDays(-(int)StartDate.DayOfWeek);
                        EndDate = thisWeekStart.AddDays(7).AddSeconds(-1);
                    }
                    else if (TodayDate > TimesheetFreezeDay)
                    {
                        StartDate = new DateTime(now.Year, now.Month, 1);
                        StartDate = StartDate.AddDays(-(int)StartDate.DayOfWeek);
                        EndDate = thisWeekStart.AddDays(7).AddSeconds(-1);
                    }
                    else
                    {
                        //CCAP Fix
                    }
                }
                else
                {
                    //CCAP Fix
                }
                UpdateRemarks(LstEffortUploadDetails1.Where(e => e.TimeSheetDate < StartDate && e.TimeSheetDate > TimesheetStartDate).ToList(),
                    Constants.TimesheetFreezeDays);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstEffortUploadDetails1;
        }
        private List<EffortUploadDet> CheckTicketType(List<EffortUploadDet> LstEffortUploadDetails1)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            try
            {

                var tckt = LstEffortUploadDetails1.AsEnumerable().Select(x => new effdup { TicketID = x.TicketID }).GroupBy(
                    x => new { x.TicketID }).Where(grp => grp.Select(x => x.TicketType).Distinct().Count() > 1).ToList();
                List<effdup> newtckt = new List<effdup>();
                for (int i = 0; i < tckt.Count; i++)
                {
                    effdup effdup1 = new effdup();
                    effdup1.TicketID = tckt[i].Key.TicketID;

                    newtckt.Add(effdup1);
                }

                List<string> TicketID = newtckt.Select(e => e.TicketID).Distinct().ToList();

                LstFilteredEffortData = LstEffortUploadDetails1.Where(e => TicketID.Contains(e.TicketID)).ToList();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            UpdateRemarks(LstFilteredEffortData.Where(x => x.TicketID != "NONDELIVERY").ToList(), Constants.TicketType);

            return LstEffortUploadDetails1;
        }
        private List<EffortUploadDet> CheckNegativeEfforts(List<EffortUploadDet> LstEffortUploadDetails1)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            try
            {

                LstFilteredEffortData = LstEffortUploadDetails1.Where(e => e.Hours < Convert.ToDecimal(0.00)).ToList();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (LstFilteredEffortData.Count > 0)
            {
                if (!LstFilteredEffortData[0].IsCognizant)
                {
                    UpdateRemarks(LstFilteredEffortData, "Efforts cannot be Negative");
                }
                else
                {
                    UpdateRemarks(LstFilteredEffortData, Constants.NegativeEfforts);
                }
            }
            return LstEffortUploadDetails1;
        }
        private List<EffortUploadDet> RemoveEmptyRows(List<EffortUploadDet> LstEffortUploadDetails1)
        {

            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();
            try
            {

                if (LstEffortUploadDetails1 != null && LstEffortUploadDetails1.Count > 0)
                {
                    LstEffortUploadDetails1 = LstEffortUploadDetails1.Where(e => (e.TicketID != "" || e.CognizantID != "" || e.HoursCheck != "" && e.TimeSheetDateCheck != "")).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstEffortUploadDetails1;
        }
        //Specialcharacter validation for the Suggested Activity
        private List<EffortUploadDet> Specialcharacter(List<EffortUploadDet> LstEffortUploadDetails1)
        {
            List<EffortUploadDet> LstFilteredEffortData = new List<EffortUploadDet>();

            Regex RgxUrl = new Regex("[^a-z0-9A-Z ]");

            UpdateRemarks(LstEffortUploadDetails1[0].IsCognizant ?
                          LstEffortUploadDetails1.Where(e => (e.TicketID == "NONDELIVERY" && e.ActivityName.Trim().ToUpper() == "OTHERS" && RgxUrl.IsMatch(e.SuggestedActivity))
                          || e.TicketID == "NONDELIVERY" && e.ActivityName.Trim().ToUpper() == "OTHERS" && e.SuggestedActivity.Length < 5 && 
                          e.SuggestedActivity != "" && e.SuggestedRemarks != "").ToList() :
                          LstEffortUploadDetails1.Where(e => (e.TicketID == "NONDELIVERY" && e.TicketType.Trim().ToUpper() == "OTHERS" && RgxUrl.IsMatch(e.SuggestedActivity))
                          || e.TicketID == "NONDELIVERY" && e.TicketType.Trim().ToUpper() == "OTHERS" && 
                          e.SuggestedActivity.Length < 5 && e.SuggestedActivity != "" && e.SuggestedRemarks != "").ToList(),
                           Constants.CommonforSuggestedActivity);

            return LstEffortUploadDetails1;
        }
        //Suggested Activity validation 
        private List<EffortUploadDet> SuggestedActivitycheck(List<EffortUploadDet> LstEffortUploadDetails1)
        {
            UpdateRemarks(LstEffortUploadDetails1[0].IsCognizant ?
                          LstEffortUploadDetails1.Where(e => e.TicketID == "NONDELIVERY" && e.ActivityName.Trim().ToUpper() == "OTHERS" &&
                                                       (e.SuggestedActivity == "")).ToList() :
                          LstEffortUploadDetails1.Where(e => e.TicketID == "NONDELIVERY" && e.TicketType.Trim().ToUpper() == "OTHERS" &&
                                                       (e.SuggestedActivity == "")).ToList(),
                                                       Constants.SuggestedActivitycheck
                            );

            return LstEffortUploadDetails1;
        }

        //Suggested Remarks validation 
        private List<EffortUploadDet> SuggestedRemarkscheck(List<EffortUploadDet> LstEffortUploadDetails1)
        {
            UpdateRemarks(LstEffortUploadDetails1[0].IsCognizant ?
                          LstEffortUploadDetails1.Where(e => e.TicketID == "NONDELIVERY" && e.ActivityName.Trim().ToUpper() == "OTHERS" &&
                                                       (e.SuggestedRemarks == "")).ToList() :
                          LstEffortUploadDetails1.Where(e => e.TicketID == "NONDELIVERY" && e.TicketType.Trim().ToUpper() == "OTHERS" &&
                                                       (e.SuggestedRemarks == "")).ToList(),
                                                       Constants.SuggestedRemarkscheck
                            );

            return LstEffortUploadDetails1;
        }       
        private string DecimalConverion(string value)
        {
            int indexValue = value.IndexOf('-');
            int snumber = 0;
            double enumber = 0;
            double dnumber = 0;
            decimal Finalvalue = 0;
            if (value.Contains('-') && value.Contains('E'))
            {
                snumber = Convert.ToInt16(value[0].ToString());
                enumber = Convert.ToInt16(value.Substring(indexValue + 1));
                dnumber = ZeroGendrater(enumber);
                Finalvalue = Convert.ToDecimal(Convert.ToInt16(snumber) / dnumber);
                value = Finalvalue.ToString();
            }
            return value;
        }
        private static double ZeroGendrater(double value)
        {
            string dnumber = "1";
            for (int i = 0; i < value; i++)
            {
                dnumber += "0";
            }
            return Convert.ToDouble(dnumber);
        }
        public string UpdateTicketIsAttributeFlagRepo(Int64 ProjectID)
        {
            EffortUploadRespository effresp = new EffortUploadRespository();
            string IsUpdated = "";
            try
            {
                /// Sp call to perform validation and insertion
                IsUpdated = effresp.UpdateTicketIsAttributeFlagDetails(ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return IsUpdated;
        }
        private List<EffortUploadDet> TypeValidation(List<EffortUploadDet> LstEffortUploadDetails1)
        {
            List<EffortUploadDet> LstFilteredEffortType = new List<EffortUploadDet>();
            try
            {

                LstFilteredEffortType = !isinfraproject ? LstEffortUploadDetails1.Where(e => e.Type != "ND" && e.Type != "T" && e.Type != "W").ToList() :
                                                  LstEffortUploadDetails1.Where(e => e.Type != "I" && e.Type != "ND").ToList();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            UpdateRemarks(LstFilteredEffortType.ToList(), "Type is mandatory and can only accept either Work Item (or) Ticket (or) NonDelivery");
            return LstEffortUploadDetails1;
        }
        private static string ConvertDecimal(object Inboundvalue)
        {
            string result = string.Empty;
            string value = Inboundvalue.ToString();
            int ExponentialIndex = value.IndexOf("E");
            bool FoundExponential = false;
            if (ExponentialIndex >= 0)
            {
                FoundExponential = true;
                value = value.Substring(0, (ExponentialIndex));

            }
            if (value.Contains("."))
            {
                int afterDeimal = value.Length - value.IndexOf(".");
                afterDeimal = afterDeimal >= 8 ? 8 : afterDeimal;
                result = value.Substring(0, value.IndexOf(".")) +
                    value.Substring(value.IndexOf("."), (afterDeimal));
                if (FoundExponential)
                {
                    result = Convert.ToString(Convert.ToDecimal(result) / 100);
                }
            }
            else
            {
                result = value;
            }
            return result;
        }
        public string TriggerSharepath(inputparam finalData, string EmployeeID)
        {
            try
            {
                string sWSName = "";
                DataSet ds;
                string result = "";
                string[] dataarray = new string[1];
                dataarray[0] = "TimeSheet Date";
                EffortUploadRespository effresp = new EffortUploadRespository();
                sWSName = new AppSettings().AppsSttingsKeyValues["SheetName"];
                if (finalData.Path != "")
                {
                    objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(finalData.TrackID), objTrack.ProjectID, objTrack.EmployeeID, 
                        objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                                , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "API recived the File" + finalData.Path);

                    ds = new DataSet();
                    ds.Locale = CultureInfo.InvariantCulture;
                    ds.Tables.Add(new OpenXMLOperations().ToDataTableBySheetName(finalData.Path, sWSName, 0, 1, dataarray).Copy());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!ds.Tables[0].Columns.Contains("ProjectID"))
                        {
                            ds.Tables[0].Columns.Add("ProjectID");
                        }

                        if (!ds.Tables[0].Columns.Contains("IsCognizant"))
                        {
                            ds.Tables[0].Columns.Add("IsCognizant");
                        }

                        if (!ds.Tables[0].Columns.Contains("IsEffortTrackActivityWise"))
                        {
                            ds.Tables[0].Columns.Add("IsEffortTrackActivityWise");
                        }
                        if (!ds.Tables[0].Columns.Contains("IsDaily"))
                        {
                            ds.Tables[0].Columns.Add("IsDaily");
                        }
                        if (!ds.Tables[0].Columns.Contains("TrackID"))
                        {
                            ds.Tables[0].Columns.Add("TrackID");
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[0].Rows[0]["ProjectID"] = finalData.ProjectID;
                            ds.Tables[0].Rows[0]["IsCognizant"] = finalData.IsCognizant;
                            ds.Tables[0].Rows[0]["IsEffortTrackActivityWise"] = finalData.IsEffortTrackActivityWise;
                            ds.Tables[0].Rows[0]["IsDaily"] = finalData.IsDaily;
                            ds.Tables[0].Rows[0]["TrackID"] = finalData.TrackID;
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                ds.Tables[0].Rows[i]["Hours"] = DecimalConverion(ds.Tables[0].Rows[i]["Hours"].ToString());
                            }
                            catch (Exception ex)
                            {
                                objTrack = effresp.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID, objTrack.EmployeeID, 
                                    objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                                , objTrack.FilePickedTime, objTrack.APIRequestedTime, DateTime.Now.ToString(), "Error: ProcessEffort upload method - Conversion Decimal || Message :" + ex.Message);
                            }
                        }
                        // dataset to JSON conversion 
                        DataTable finalDataValue = ds.Tables[0].Rows.Cast<DataRow>()
                      .Where(row => !row.ItemArray.All(field => field is System.DBNull))
                      .CopyToDataTable();

                        foreach (DataColumn column in ds.Tables[0].Columns)
                        {
                            if (column.ColumnName.Contains("Column"))
                            {
                                finalDataValue.Columns.Remove(column.ColumnName);

                            }
                        }

                        result = TriggerEffortUpload(finalDataValue, EmployeeID);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
