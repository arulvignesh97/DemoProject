using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Common.Common;
using CTS.Applens.WorkProfiler.Common.Extensions;
using CTS.Applens.WorkProfiler.DAL.BaseDetails;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TicketingModuleUtilsLib.ExportImport.OpenXML;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;
using CTS.Applens.WorkProfiler.Models.EffortUpload;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.DAL
{

    /// <summary>
    /// TicketUploadRepository
    /// </summary>
    public class TicketUploadRepository : DBContext
    {
        private Int64 ticketUploadTrackID = 0;
        private readonly string strFrom = new AppSettings().AppsSttingsKeyValues["SDSupport"];
        private readonly string commonAPIURL = new AppSettings().AppsSttingsKeyValues["CommonAPIURL"];
        readonly string apiKeyHandler = new AppSettings().AppsSttingsKeyValues["ApiKeyHandler"];
        readonly string apiValueHandler = new AppSettings().AppsSttingsKeyValues["APIValueHandler"];
        readonly string apiAuthKeyHandler = new AppSettings().AppsSttingsKeyValues["APIAuthKeyHandler"];
        readonly string apiAuthValueHandler = new AppSettings().AppsSttingsKeyValues["APIAuthValueHandler"];
        private readonly string EffortBulkUploadPath = new AppSettings().AppsSttingsKeyValues["EffortBulkUpload"];
        private static string EffortBulkUploadAPIURL = new AppSettings().AppsSttingsKeyValues["EffortBulkUploadAPIURL"];
        private static string conn = new AppSettings().AppsSttingsKeyValues["ConnectionStrings:AppLensConnection"];
        private static EfforUploadTracker objTrack = new EfforUploadTracker();
        private static EffortUploadRespository Repo = new EffortUploadRespository();

        private static string strresult;
        public static string StrResult
        {
            get
            {
                return strresult;
            }

            set
            {
                strresult = value;
            }
        }
        private string stremployeeID;
        public string StrEmployeeID
        {
            get
            {
                return stremployeeID;
            }

            set
            {
                stremployeeID = value;
            }
        }
        private string intprojectID;
        public string IntProjectID
        {
            get
            {
                return intprojectID;
            }

            set
            {
                intprojectID = value;
            }
        }
        private static string mailstrResult;
        public static string MailstrResult
        {
            get
            {
                return mailstrResult;
            }

            set
            {
                mailstrResult = value;
            }
        }
        private bool flgTurntime = false;
        private bool flgmoddate = false;
        private bool flgplaneffort = false;
        private bool flgplaneff = false;
        private bool flgActaleff = false;
        private bool flgActwor = false;
        private bool flgEstworks = false;
        private bool flgESTwor = false;
        private bool flgrestime = false;
        private bool flgresptime = false;
        private bool flgplanduration = false;
        private bool flgoutageduration = false;
        private bool flgexpcomdate = false;
        private bool flgactdur = false;
        private bool flgreopendate = false;
        private bool flgclosedate = false;
        private bool flgplandate = false;
        private bool flgplanstrdate = false;
        private bool flgnewstrdate = false;
        private bool flgresdate = false;
        private bool flgrejtimdate = false;
        private bool flgreldate = false;
        private bool flgActstrdate = false;
        private bool flgActenddate = false;
        private bool flgAssigndate = false;
        private bool flgsSLAErr = false;
        private bool flgssMetresSLAErr = false;
        private bool flgssMetAckSLAErr = false;
        private bool flgssMetreslSLAErr = false;
        private bool flgelvErr = false;
        private bool flgkedbup = false;
        private bool flgNatoftic = false;
        private bool flgEsc = false;
        private bool flgout = false;
        private bool flgwarIss = false;
        private bool flgreltype = false;
        private bool flgstrErrRequestedResolutionDateTime = false;
        private bool flgstrCSATScore = false;
        private bool flgstrErrstrErrApprovedDateTime = false;
        private bool flgstrErrOpenDate = false;
        private bool flgstrErrReviewedDateTime = false;
        private bool flgstrErrStartedDateTime = false;
        private bool flgstrErrWIPDateTime = false;
        private bool flgOnHoldDateTime = false;
        private bool flgCompletedDateTime = false;
        private bool flgstrErrCancelledDateTime = false;
        private bool flgopendate = false;
        private bool flgITSMffort = false;
        private string excelfilename = "";
        /// <summary>
        /// This Method Is Used To DownloadTicketDumpTemplate
        /// </summary>
        /// <param name="EmployeeID">This parameter holds EmployeeID value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <returns>Method returns Column mapping</returns>
        public string DownloadTicketDumpTemplate(string EmployeeID, string ProjectID)
        {
            StringBuilder sPath = new StringBuilder();

            try
            {
                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@employeeid", EmployeeID);
                prms[1] = new SqlParameter("@mode", "GetfinalcolumnMapping");
                prms[2] = new SqlParameter("@projectid", ProjectID);
                DataTable dt = new DataTable();
                dt.Locale = CultureInfo.InvariantCulture;
                dt = (new DBHelper()).GetTableFromSP("[AVL].[TKT_DownloadTicketDumpDetails]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StringBuilder newpth = new StringBuilder();
                    StringBuilder orgpath = new StringBuilder();
                    StringBuilder orginalfile = new StringBuilder();
                    string sourcepath = "";
                    string foldername = "";
                    sourcepath = new ApplicationConstants().TicketDownloadExcelPath;

                    foldername = new ApplicationConstants().DownloadExcelTemp;

                    string strExtension = Path.GetExtension(sourcepath);
                    orginalfile.Append(Path.GetDirectoryName(sourcepath)).Append("\\");
                    string filename = Path.GetFileName(sourcepath);
                    DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                    FileInfo fleInfo = new FileInfo(sourcepath);
                    string struserID = EmployeeID;
                    string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss");
                    var ext = strExtension;
                    orgpath.Append(foldername).Append(fleInfo.Name.Split('.')[0]).Append("_").
                        Append(struserID).Append("_").Append(strTimeStamp).Append(ext);
                    DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalfile.ToString());
                    if (directoryInfo.Exists)
                    {
                        newpth.Append(directoryInfo).Append(fleInfo.Name.Split('.')[0]).Append("_").
                            Append(struserID).Append("_").Append(strTimeStamp).Append(ext);

                        string dirctoryName = Path.GetDirectoryName(newpth.ToString());
                        string fName = Path.GetFileNameWithoutExtension(newpth.ToString());
                        string validatePath = Path.Combine(dirctoryName, fName, ".xlsx");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        if (File.Exists(validatePath))
                        {
                            File.Delete(validatePath);
                            fleInfo.CopyTo(validatePath, true);
                        }
                        else
                        {
                            fleInfo.CopyTo(validatePath, true);
                        }
                    }
                    StringBuilder sExcelPath = new StringBuilder();
                    StringBuilder sExcelFileName = new StringBuilder();
                    StringBuilder sPathfornotexist = new StringBuilder();
                    sExcelPath.Append(new ApplicationConstants().DownloadExcelTemp);
                    StringBuilder TicketDownloadBasePath = new StringBuilder();
                    TicketDownloadBasePath.Append(new ApplicationConstants().DownloadExcelTemp);

                    sExcelFileName.Append(EmployeeID).Append("_").Append(DateTimeOffset.Now.DateTime.Month.ToString()).
                        Append(DateTimeOffset.Now.DateTime.Day.ToString()).Append(DateTimeOffset.Now.DateTime.Year.ToString()).
                        Append(DateTimeOffset.Now.DateTime.Hour.ToString()).Append(DateTimeOffset.Now.DateTime.Minute.ToString()).
                        Append(DateTimeOffset.Now.DateTime.Second.ToString()).Append(".xlsx");
                    sPath.Append(sExcelPath).Append(sExcelFileName);
                    sPathfornotexist.Append(sExcelPath.Replace("..", "").Append(sExcelFileName.ToString()));
                    if (Directory.Exists(TicketDownloadBasePath.ToString()) && !File.Exists(sPathfornotexist.ToString()))
                    {

                        //CCAP FIX

                    }
                    else
                    {
                        //Veracode fix
                        TicketDownloadBasePath.Append("\\TempTicketUpload.xlsx");
                        string dirctoryName = System.IO.Path.GetDirectoryName(TicketDownloadBasePath.ToString());
                        string fName = System.IO.Path.GetFileNameWithoutExtension(TicketDownloadBasePath.ToString());
                        string validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsx");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(validatePath));

                    }
                    var tblPivot = new DataTable();
                    tblPivot.Locale = CultureInfo.InvariantCulture;
                    for (int col = 0; col < 1; col++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            tblPivot.Columns.Add(dt.Rows[j][col].ToString());
                        }
                    }

                    object[] columnNames = tblPivot.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToArray();
                    tblPivot.Rows.Add(columnNames);
                    sPath = new StringBuilder();
                    sPath.Append(new OpenXMLOperations().ToExcelSheetByDataTable(tblPivot, null, newpth.ToString(),
                        "BulkUpload", null));

                }
                else
                {
                    sPath.Append("Column Mapping has not been done in ITSM Configuration.");
                }
                return sPath.ToString();
            }

            catch (Exception ex)
            {
                //  Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace, "Download Template", 0);
                return "Problem With Download";
                throw ex;
            }
        }

        /// <summary>
        /// 
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

        public string ProcessFileforEffortUpload(string Filename, string filePath, string isCognizant, int projectID, string isEffortTrackActivityWise, string isDaily, string isApp, string EmployeeID)
        {
            DataSet ds;
            string SheetName = "EffortUpload";
            string Result = String.Empty;
            Boolean IsCognizant = (isCognizant.Trim() == "" || isCognizant.Trim() == "0") ? false : true;
            Boolean IsEffortTrackActivityWise = (!string.IsNullOrEmpty(isEffortTrackActivityWise) && isEffortTrackActivityWise.Trim().ToUpper() == "TRUE") ? true : false;
            Boolean IsDaily = (!string.IsNullOrEmpty(isDaily) && isDaily.Trim().ToUpper() == "TRUE") ? true : false;
            Boolean IsApp = (!string.IsNullOrEmpty(isApp) && isApp.Trim().ToUpper() == "TRUE") ? true : false;

            try
            {
                string[] dataarray = new string[1];
                dataarray[0] = "TimeSheet Date";


                if (Filename != "")
                {
                    try
                    {
                        ds = new DataSet();
                        ds.Locale = CultureInfo.InvariantCulture;
                        ds.Tables.Add(new OpenXMLOperations().ToDataTableBySheetName(EffortBulkUploadPath + "\\" + Filename, SheetName, 0, 1, dataarray).Copy());
                    }
                    catch (Exception ex)
                    {
                        objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                            objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                       , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: Excel invalid format || Message :" + ex.Message);
                        return "Please upload Valid template, valid file is .xlsx";
                    }
                    if (ds.Tables[0].Rows.Count > 2000)
                    {
                        return "Timesheet with more than 2000 records cannot be uploaded through UI";
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable finalData = ds.Tables[0].Rows.Cast<DataRow>()
                            .Where(row => !row.ItemArray.All(field => field is System.DBNull))
                            .CopyToDataTable();

                        try
                        {
                            objTrack = Repo.GetEffortUploadTracker(null, Convert.ToString(projectID), string.Empty,
                                EffortBulkUploadPath + "\\" + Filename, string.Empty, "0", null, null, null, "New Record created");

                            inputparam finalDatavalue = new inputparam();
                            finalDatavalue.Path = EffortBulkUploadPath + "\\" + Filename;
                            finalDatavalue.ProjectID = projectID;
                            finalDatavalue.IsCognizant = IsCognizant;
                            finalDatavalue.IsEffortTrackActivityWise = IsEffortTrackActivityWise;
                            finalDatavalue.TrackID = objTrack.ID;

                            if (IsCognizant && !ValidateApporInfraTemplate(finalData, IsApp))
                            {
                                return "Please upload Valid template, valid file is .xlsx";
                            }

                            Result = new EffortUploadRespository().TriggerSharepath(finalDatavalue, EmployeeID);

                            objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                                objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                                , objTrack.FilePickedTime, objTrack.APIRequestedTime, DateTimeOffset.Now.DateTime.ToString(), null);
                        }
                        catch (Exception ex)
                        {

                            objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                                objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                            , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: ProcessEffort upload method || Message :" + ex.Message);

                        }
                    }
                    else
                    {
                        return "Upload failed.Template should not be empty";
                    }
                }


            }
            catch (Exception ex)
            {

                objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                    objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                     , objTrack.FilePickedTime, objTrack.APIRequestedTime, objTrack.APIRespondedTime, "Error: ProcessEffort upload method || Message :" + ex.Message);
                throw ex;

            }
            return Result;
        }

        bool ValidateApporInfraTemplate(DataTable finalData, Boolean isApp)
        {
            Boolean _flag = false;
            if (isApp)
            {
                _flag = finalData.Columns.Contains("ServiceName") ? true : false;
            }
            else
            {
                _flag = finalData.Columns.Contains("Activity/Task") ? true : false;
            }
            return _flag;
        }

        static bool IsHidden(string p)
        {
            return p.Contains("Hidden");
        }

        private async Task<string> API(EffortuploadProjectDetails finalData)
        {
            objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "0"
                             , objTrack.FilePickedTime, objTrack.APIRequestedTime, DateTimeOffset.Now.DateTime.ToString(), "Is ready to call API");

            string Responcestatus = "";
            try
            {
                HttpClientHandler authtHandlerClient = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };
                using (HttpClient client = new HttpClient(authtHandlerClient))

                {
                    client.BaseAddress = new Uri(EffortBulkUploadAPIURL);
                    HttpResponseMessage responseProj = await client.PostAsJsonAsync("EffortUploadAPI/TriggerSharepath?finalData=" + finalData, finalData).ConfigureAwait(false);
                    responseProj.EnsureSuccessStatusCode();
                    Responcestatus = RemoveSpecialCharacters(responseProj.Content.ReadAsStringAsync().Result);
                    objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                        objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "1"
                         , objTrack.FilePickedTime, objTrack.APIRequestedTime, DateTimeOffset.Now.DateTime.ToString(), "API Called completed in Exe : " + Responcestatus);
                }
            }
            catch (Exception Ex)
            {
                objTrack = Repo.GetEffortUploadTracker(Convert.ToInt32(objTrack.ID), objTrack.ProjectID,
                    objTrack.EmployeeID, objTrack.EffortUploadDumpFileName, objTrack.EffortUploadErrorDumpFile, "-1"
                             , objTrack.FilePickedTime, objTrack.APIRequestedTime, DateTimeOffset.Now.DateTime.ToString(), "Error in APi method : " + Ex.Message);
                throw Ex;
            }
            return Responcestatus;
        }
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                bool z1 = (c >= '0' && c <= '9');
                bool z2 = (c >= 'A' && c <= 'Z');
                bool z3 = (c >= 'a' && c <= 'z');
                bool z4 = z1 || z2 || z3;
                bool z5 = c == '_' || c == '/' || c == '-' || c == '&';

                if (c == ' ' || z4 || c == '.' || z5)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        private static string DecimalConverion(string value)
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
        /// <summary>
        /// This Method Is Used To ProcessFileforTicketUpload
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ExcelExportPath"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="EmployeeName"></param>
        /// <param name="CustomerId"></param>
        /// <param name="ProjectID"></param>
        /// <param name="flgUpload"></param>
        /// <returns></returns>
        public string ProcessFileforTicketUpload(string filename, string ExcelExportPath, string EmployeeID,
            string EmployeeName, string CustomerId, string ProjectID, string flgUpload, string IsCognizant,
            string accountname, List<HcmSupervisorList> supervisorLists, string esaprojectid, string access, bool allowEncrypt)
        {
            TicketUploadTrack ticketUploadtrack = new TicketUploadTrack();
            ticketUploadtrack.EmployeeId = EmployeeID;
            ticketUploadtrack.FileName = filename;
            ticketUploadtrack.Mode = 1;
            ticketUploadtrack.ProjectId = !string.IsNullOrEmpty(ProjectID.Trim()) ? Convert.ToInt64(ProjectID) : 0;
            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
            ticketUploadTrackID = SaveTicketUploadTrack(ticketUploadtrack, 0, 0);
            InsertTicketUploadTrackDetails(
              new StackTrace().GetFrame(1).GetMethod().Name,
              new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
             new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
             "Process Started",
             ticketUploadTrackID);
            string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
            AESEncryption aesMod = new AESEncryption();
            //Checking isTicketDescriptionOpted field
            var isTicketDescriptionOpted = CheckIsTicketDescriptionOpted(ProjectID);
            try
            {
                StrEmployeeID = EmployeeID;
                IntProjectID = ProjectID;
                string strPath = ExcelExportPath + filename;
                string path = ExcelExportPath;
                excelfilename = filename;
                bool s = File.Exists(strPath);
                List<ColumnMappting> LstColumn = new List<ColumnMappting>();
                if (HasWriteAccessToFolder(Path.GetDirectoryName(strPath)))
                {
                    if (flgUpload == "TicketeDumptoTemp")
                    {
                        string Strmailid = "";
                        string StrSharePathUsers = "";
                        try
                        {

                            StrResult = "";
                            MailstrResult = "";
                            Strmailid = StrEmployeeID;
                            StrSharePathUsers = "";
                            StringBuilder esasb = new StringBuilder();
                            List<string> strSource = new List<string>();
                            List<string> strDes = new List<string>();
                            List<string> strDes1 = new List<string>();
                            string[] strMandateColumns = { "Ticket ID" };
                            StringBuilder sbMessage = new StringBuilder();

                            foreach (string arrItem in strMandateColumns)
                            {
                                strDes1.Add(arrItem);
                            }

                            SqlParameter[] prm = new SqlParameter[2];
                            prm[0] = new SqlParameter("@mode", "GetColumns");
                            prm[1] = new SqlParameter("@projectid", ProjectID);
                            DataTable dt = new DataTable();
                            dt.Locale = CultureInfo.InvariantCulture;
                            dt = (new DBHelper()).GetTableFromSP("[AVL].[TK_GetTicketUploadDetails]", prm, ConnectionString);

                            InsertTicketUploadTrackDetails(
                           new StackTrace().GetFrame(1).GetMethod().Name,
                           new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                           new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                           "Stored Procedure [AVL].[TK_GetTicketUploadDetails] is successfully executed",
                           ticketUploadTrackID);

                            if (dt.Rows.Count > 0)
                            {
                                ColumnMappting TempColumn = new ColumnMappting();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    strDes.Add(dt.Rows[i]["ServiceDartColumn"].ToString());
                                    strSource.Add(dt.Rows[i]["ProjectColumn"].ToString().ToUpper().TrimStart().
                                        TrimEnd());

                                    TempColumn.ServiceDartColumn = dt.Rows[i]["ServiceDartColumn"].ToString();
                                    TempColumn.ProjectColumn = dt.Rows[i]["ProjectColumn"].ToString().ToUpper().
                                        TrimStart().TrimEnd();

                                    LstColumn.Add(TempColumn);
                                    TempColumn = new ColumnMappting();
                                }

                                InsertTicketUploadTrackDetails(
                                 new StackTrace().GetFrame(1).GetMethod().Name,
                                 new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                 new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                 "Column Mapping Loop is completed.",
                                 ticketUploadTrackID);

                            }
                            else
                            {
                                InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                "Column Mapping has not been done in ITSM Configuration",
                                ticketUploadTrackID);
                                MailstrResult = "";
                                MailstrResult = "Column Mapping has not been done in ITSM Configuration";
                                return MailstrResult;
                            }

                            string[] strSourceAddition = { "EMPLOYEEID", "EMPLOYEENAME", "PROJECTID" };
                            string[] strdesAddition = { "EmployeeID", "EmployeeName", "ProjectID" };
                            strSource.AddRange(strSourceAddition);
                            strDes.AddRange(strdesAddition);
                            DataTable dtBulkUpload = new DataTable();
                            dtBulkUpload.Locale = CultureInfo.InvariantCulture;
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                "Going to Convert the Excel into DataSet Using Open XML ",
                                ticketUploadTrackID);
                            dtBulkUpload = ExcelToDataSet(strPath, EmployeeName);
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                "Successfully Converted the Excel to DataSet and the Total Number of Records: ",
                                ticketUploadTrackID);
                            if (IsCognizant == "1")
                            {
                                if (MailstrResult != "")
                                {

                                    Mail(ProjectID, StrEmployeeID, StrSharePathUsers, MailstrResult, CustomerId);
                                    string MyActivityNeededKey = new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"];
                                    if (MyActivityNeededKey == "true" && dtBulkUpload == null)
                                    {
                                        string Workitemcode = new AppSettings().AppsSttingsKeyValues["TicketuploadFailedCode"];
                                        bool CheckSourceidstatus = new MyActivity().CheckexistingActivity(Convert.ToInt64(ProjectID), Workitemcode, access);

                                        if (!CheckSourceidstatus)
                                        {
                                            foreach (var item in supervisorLists)
                                            {
                                                MyActivitySourceDto myActivitySource = new MyActivitySourceDto();
                                                myActivitySource.ActivityDescription = "Ticket upload failed for the project " + esaprojectid + " - " + accountname + " due to incorrect template." +
                                                " Uploaded template should match with the column mapping defined for your project.";
                                                myActivitySource.WorkItemCode = Workitemcode;
                                                myActivitySource.SourceRecordID = Convert.ToInt64(ProjectID);
                                                myActivitySource.RaisedOnDate = DateTime.Today;
                                                myActivitySource.RequestorJson = "";
                                                myActivitySource.CreatedBy = "System";
                                                myActivitySource.ActivityTo = item.HcmSupervisorID;
                                                string st = new MyActivity().MySaveActivity(myActivitySource, access);
                                            }
                                        }
                                    }
                                    return "Dump Upload Failed.Please check e-mail.";
                                }
                            }
                            else
                            {
                                if (MailstrResult != "")
                                {
                                    return MailstrResult;
                                }
                            }
                            var dtColumnNames = (from DataColumn x in dtBulkUpload.Columns
                                                 select x.ColumnName.ToUpper()).ToArray();

                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                "Going to Start the Column Validation ",
                                ticketUploadTrackID);
                            dtBulkUpload = ColumnValidation(ref dtBulkUpload, dtColumnNames, strSource, strDes, ProjectID, isTicketDescriptionOpted, accountname, supervisorLists, esaprojectid, access);
                            sbMessage.Clear();
                            sbMessage.Append(ApplicationConstants.ColumnValidationMessage)
                                .Append(dtBulkUpload.Rows.Count.ToString());
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                sbMessage.ToString(), ticketUploadTrackID);

                            ticketUploadtrack.IsColumnMappingValidated = true;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack, 0, ticketUploadTrackID);

                            MailstrResult = "";
                            ticketUploadtrack.MndColValBeginTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack, 0, ticketUploadTrackID);
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                 "Going to Start the ValidateBulkData - Mandatory Columns ",
                                ticketUploadTrackID);
                            ValidateBulkData param = new ValidateBulkData();
                            param.dtBulk = dtBulkUpload;
                            param.strDest = strDes;
                            param.strSource = strDes1;
                            MailstrResult = ValidateBulkData(param);

                            sbMessage.Clear();
                            sbMessage.Append(ApplicationConstants.BulkDataValidationMessage)
                                     .Append(MailstrResult);
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                 sbMessage.ToString(), ticketUploadTrackID);

                            ticketUploadtrack.MndColValEndTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack, (short)TicketUploadTrackScenarios.
                                MandatoryColumnValidation, ticketUploadTrackID);

                            esasb.Append(MailstrResult);

                            ticketUploadtrack.NonMndColValBeginTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack, 0, ticketUploadTrackID);

                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                  "Going to Start the ValidateNoNManBulkData - Non Mandatory Columns ",
                                ticketUploadTrackID);
                            MailstrResult = ValidateNoNManBulkData(ref dtBulkUpload, strDes, strDes);
                            if(IsCognizant!="1")
                            {
                                if(MailstrResult!="")
                                {
                                    int pos = MailstrResult.IndexOf("<BR>");
                                    return MailstrResult.Remove(pos);
                                }
                            }
                            sbMessage.Clear();
                            sbMessage.Append("Successfully Executed the ValidateNoNManBulkData - ")
                                .Append("Non Mandatory Columns,")
                                .Append(" the result is :").Append(MailstrResult);
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                sbMessage.ToString(), ticketUploadTrackID);
                            ticketUploadtrack.NonMndColValEndTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack, (short)TicketUploadTrackScenarios.
                                NonMandatoryColumnValidation, ticketUploadTrackID);

                            esasb.Append(MailstrResult);

                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                   "Going to Start the Validatelength - All the Length Validation ",
                                ticketUploadTrackID);

                            MailstrResult = Validatelength(ref dtBulkUpload, strDes, strDes);

                            sbMessage.Clear();
                            sbMessage.Append(ApplicationConstants.SuccessfullLengthValidation).Append(MailstrResult);
                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                   sbMessage.ToString(), ticketUploadTrackID);
                            esasb.Append(MailstrResult);
                            foreach (DataRow dr in dtBulkUpload.Rows)
                            {
                                MailstrResult = DataRowValidations(dr);
                                if (!MailstrResult.Equals(""))
                                {
                                    esasb.Append(MailstrResult);
                                }
                            }
                            MailstrResult = esasb.ToString();
                            if (IsCognizant == "1")
                            {
                                if (MailstrResult != "")
                                {

                                    Mail(ProjectID, StrEmployeeID, StrSharePathUsers, MailstrResult, CustomerId);

                                    return "Dump Upload Failed.Please check e-mail.";
                                }
                            }
                            bool isMultilingualEnabled = CheckIfMultilingualEnabled(ProjectID, EmployeeID);
                            List<TicketDescriptionSummary> ticketDescriptionSummary =
                                new List<TicketDescriptionSummary>();
                            List<string> lstColumns = new List<string>();
                            List<TicketSupportTypeMapping> lstColumnsMapping = new List<TicketSupportTypeMapping>();
                            dtBulkUpload.Columns.Add("IsTicketSummaryModified", typeof(string));
                            dtBulkUpload.Columns.Add("IsTicketDescriptionModified", typeof(string));
                            dtBulkUpload.Columns.Add("SupportType", typeof(int));
                            dtBulkUpload.AcceptChanges();
                            if (isMultilingualEnabled)
                            {
                                lstColumns = GetColumnsEncrypted(ProjectID, EmployeeID);
                                if (lstColumns.Count > 0)
                                {
                                    if (!dtBulkUpload.Columns.Contains("Tower"))
                                    {
                                        foreach (DataRow dr in dtBulkUpload.Rows)
                                        {
                                            TicketSupportTypeMapping tktSupport = new TicketSupportTypeMapping();
                                            tktSupport.TicketId = dr["Ticket ID"].ToString();
                                            tktSupport.SupportType = 1;
                                            lstColumnsMapping.Add(tktSupport);
                                            dr["SupportType"] = 1;
                                        }
                                    }
                                    else if (!dtBulkUpload.Columns.Contains("Application"))
                                    {
                                        foreach (DataRow dr in dtBulkUpload.Rows)
                                        {
                                            TicketSupportTypeMapping tktSupport = new TicketSupportTypeMapping();
                                            tktSupport.TicketId = dr["Ticket ID"].ToString();
                                            tktSupport.SupportType = 2;
                                            lstColumnsMapping.Add(tktSupport);
                                            dr["SupportType"] = 2;
                                        }
                                    }
                                    else
                                    {
                                        int projectSupportType = GetSupportType(ProjectID).FirstOrDefault().
                                            SupportTypeId;
                                        foreach (DataRow dr in dtBulkUpload.Rows)
                                        {
                                            TicketSupportTypeMapping tktSupport = new TicketSupportTypeMapping();
                                            tktSupport.TicketId = dr["Ticket ID"].ToString();
                                            if (string.IsNullOrEmpty(dr["Application"].ToString()))
                                            {
                                                if (!string.IsNullOrEmpty(Convert.ToString(dr["Tower"])))
                                                {
                                                    tktSupport.SupportType = 2;
                                                }
                                                else
                                                {
                                                    tktSupport.SupportType = 3;
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(Convert.ToString(dr["Tower"])))
                                                {
                                                    tktSupport.SupportType = projectSupportType;
                                                }
                                                else
                                                {
                                                    tktSupport.SupportType = 1;
                                                }
                                            }
                                            lstColumnsMapping.Add(tktSupport);
                                            dr["SupportType"] = tktSupport.SupportType;
                                        }
                                    }
                                    ticketDescriptionSummary = GetTicketValues(lstColumnsMapping, ProjectID,
                                        EmployeeID);
                                }
                            }
                            DataColumnCollection columns = dtBulkUpload.Columns;
                            string strTicketDescvalue = "";
                            string strTicketSumvalue = "";
                            string strResolutionremarksvalue = "";
                            var EnTicketDescriptionwithoutTag = "";
                            var DeTicketDescriptionwithoutTag = "";
                            var EnTicketSummarywithoutTag = "";
                            var DeTicketSummarywithoutTag = "";
                            var ResolutionRemarkswithoutTag = "";

                            if (columns.Contains("Ticket Description"))
                            {
                                int index = dtBulkUpload.Columns["Ticket Description"].Ordinal;
                                if (isMultilingualEnabled && lstColumns.Contains("Ticket Description"))
                                {
                                    dtBulkUpload = ModifyTicketDescription(dtBulkUpload, ticketDescriptionSummary,
                                        encryptionEnabled, index);
                                }
                                else
                                {
                                    for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
                                    {
                                        if (encryptionEnabled == "Enabled")
                                        {
                                            EnTicketDescriptionwithoutTag = Regex.Replace(string.
                                                IsNullOrEmpty(dtBulkUpload.Rows[i][index].ToString()) ? string.Empty
                                                : dtBulkUpload.Rows[i][index].ToString(), @"(\s+|<|>)", " ");
                                            strTicketDescvalue = string.IsNullOrEmpty(EnTicketDescriptionwithoutTag) ?
                                                "" : Convert.ToBase64String(aesMod.EncryptStringAsBytes
                                                (EnTicketDescriptionwithoutTag, AseKeyDetail.AesKeyConstVal));
                                            dtBulkUpload.Rows[i][index] = strTicketDescvalue;
                                        }
                                        else
                                        {
                                            DeTicketDescriptionwithoutTag = Regex.Replace(dtBulkUpload.Rows[i][index].
                                                ToString(), @"(\s+|<|>)", " ");
                                            strTicketDescvalue = DeTicketDescriptionwithoutTag;
                                            dtBulkUpload.Rows[i][index] = strTicketDescvalue;
                                        }
                                    }
                                }
                            }

                            if (columns.Contains("Ticket Summary"))
                            {
                                int index = dtBulkUpload.Columns["Ticket Summary"].Ordinal;
                                if (isMultilingualEnabled && lstColumns.Contains("Ticket Summary"))
                                {
                                    dtBulkUpload = ModifyTicketSummary(dtBulkUpload, ticketDescriptionSummary
                                        , encryptionEnabled, index);
                                }
                                else
                                {
                                    for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
                                    {
                                        if (encryptionEnabled == "Enabled")
                                        {
                                            EnTicketSummarywithoutTag = Regex.Replace(string.IsNullOrEmpty(dtBulkUpload.
                                                Rows[i][index].ToString()) ? "" : dtBulkUpload.Rows[i][index].
                                                ToString(), @"(\s+|<|>)", " ");
                                            strTicketSumvalue = string.IsNullOrEmpty(EnTicketSummarywithoutTag)
                                                ? string.Empty : Convert.ToBase64String(aesMod.EncryptStringAsBytes(
                                                    EnTicketSummarywithoutTag, AseKeyDetail.AesKeyConstVal));
                                            dtBulkUpload.Rows[i][index] = strTicketSumvalue;
                                        }
                                        else
                                        {
                                            DeTicketSummarywithoutTag = Regex.Replace(dtBulkUpload.Rows[i][index].
                                                ToString(), @"(\s+|<|>)", " ");
                                            strTicketSumvalue = DeTicketSummarywithoutTag;
                                            dtBulkUpload.Rows[i][index] = strTicketSumvalue;
                                        }
                                    }
                                }
                            }
                            dtBulkUpload.Columns.Remove("SupportType");
                            dtBulkUpload.AcceptChanges();
                            if (columns.Contains("Cause code"))
                            {
                                dtBulkUpload.Columns["Cause Code"].ColumnName = "TicketLocation";

                            }

                            if (columns.Contains("Resolution Code"))
                            {
                                dtBulkUpload.Columns["Resolution Code"].ColumnName = "Reviewer";

                            }


                            if (columns.Contains("Resolution Remarks"))
                            {
                                int index = dtBulkUpload.Columns["Resolution Remarks"].Ordinal;
                                for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
                                {
                                    ResolutionRemarkswithoutTag = Regex.Replace(dtBulkUpload.Rows[i][index].
                                        ToString(), @"(\s+|<|>)", " ");
                                    strResolutionremarksvalue = ResolutionRemarkswithoutTag;
                                    dtBulkUpload.Rows[i][index] = strResolutionremarksvalue;
                                }
                                dtBulkUpload.Columns["Resolution Remarks"].ColumnName = "Resolution Method";
                            }
                            if (columns.Contains("Ticket source"))
                            {
                                dtBulkUpload.Columns["Ticket source"].ColumnName = "Ticket Source";

                            }
                            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                            keyValuePairs = GendrateKeyValue(LstColumn.Where(x => x.ServiceDartColumn.Trim().ToUpper()
                            != ("CAUSE CODE") && x.ServiceDartColumn.Trim().ToUpper() != ("RESOLUTION REMARKS")
                                            && x.ServiceDartColumn.Trim().ToUpper() != ("RESOLUTION CODE") &&
                                            x.ServiceDartColumn.Trim().ToUpper() != ("TICKET SOURCE")).ToList());

                            keyValuePairs.Add("Causecode", "TicketLocation");
                            keyValuePairs.Add("ResolutionCode", "Reviewer");
                            keyValuePairs.Add("ResolutionRemarks", "Resolution Method");
                            keyValuePairs.Add("Ticketsource", "Ticket Source");

                            keyValuePairs.Add("EmployeeID", "EmployeeID");
                            keyValuePairs.Add("EmployeeName", "EmployeeName");
                            keyValuePairs.Add("ProjectID", "ProjectID");
                            keyValuePairs.Add("IsTicketSummaryModified", "IsTicketSummaryModified");
                            keyValuePairs.Add("IsTicketDescriptionModified", "IsTicketDescriptionModified");

                            List<TicketDetail> Ticket = new List<TicketDetail>();

                            if (isTicketDescriptionOpted)
                            {
                                Ticket = ListExtensions.ConvertTicketDetails<TicketDetail>(dtBulkUpload, keyValuePairs);
                            }
                            else
                            {
                                WorkPatternColumns workpaterncol = GetWorkPatternColumns(ProjectID);
                                Ticket = ListExtensions.ConvertTicketDetailsWorkPattern<TicketDetail>(dtBulkUpload, keyValuePairs, workpaterncol);
                            }

                            ticketUploadtrack.StoredProcedureStartTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack,
                                (short)TicketUploadTrackScenarios.StoredProcedureStartTime, ticketUploadTrackID);

                            InsertTicketUploadTrackDetails(
                                new StackTrace().GetFrame(1).GetMethod().Name,
                                new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                                new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                   "Going to call the Stored Procedure",
                                ticketUploadTrackID);
                            StrResult = InsertTicketDumpUpload(EmployeeID, ProjectID, filename, Ticket,
                                ticketUploadTrackID, accountname, supervisorLists, esaprojectid, access, allowEncrypt);
                            InsertTicketUploadTrackDetails(
                               new StackTrace().GetFrame(1).GetMethod().Name,
                               new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                               new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                                    "Successfully called the Stored Procedure",
                               ticketUploadTrackID);

                            ticketUploadtrack.StoredProcedureEndTime = DateTimeOffset.Now.DateTime;
                            UpdateTicketUploadtrackCommonFields(EmployeeID, ticketUploadtrack);
                            SaveTicketUploadTrack(ticketUploadtrack,
                                (short)TicketUploadTrackScenarios.StoredProcedureEndTime, ticketUploadTrackID);
                            if (StrResult.Trim().ToUpper() == "PROBLEM IN UPLOAD")
                            {
                                if (IsCognizant == "1")
                                {
                                    Mail(ProjectID, StrEmployeeID, StrSharePathUsers, StrResult, CustomerId);
                                    return "Dump Upload Failed.Please check e-mail.";
                                }
                                else
                                {
                                    return "PROBLEM IN UPLOAD";
                                }
                            }
                            return StrResult;
                        }

                        catch (Exception ex)
                        {
                            StringBuilder exceptionMessage = new StringBuilder();
                            DebtFieldsApprovalRepository objDebtFieldsApprovalRepository =
                                new DebtFieldsApprovalRepository();
                            objDebtFieldsApprovalRepository.ErrorLOG(ex.Message,
                                "Reached untill mail functionality for ticket upload", Convert.ToInt64(ProjectID));
                            exceptionMessage.Append("Template is not matching with ITSM configuration Column mapping.")
                              .Append(" Please upload valid template.");
                            if (IsCognizant == "1")
                            {
                                Mail(ProjectID, StrEmployeeID, StrSharePathUsers, exceptionMessage.ToString(), CustomerId);
                            }

                            string MyActivityNeededKey = new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"];
                            if (MyActivityNeededKey == "true")
                            {
                                string Workitemcode = new AppSettings().AppsSttingsKeyValues["TicketuploadFailedCode"];
                                bool CheckSourceidstatus = new MyActivity().CheckexistingActivity(Convert.ToInt64(ProjectID), Workitemcode, access);

                                if (!CheckSourceidstatus)
                                {
                                    foreach (var item in supervisorLists)
                                    {
                                        MyActivitySourceDto myActivitySource = new MyActivitySourceDto();
                                        myActivitySource.ActivityDescription = "Ticket upload failed for the project " + esaprojectid + " - " + accountname + " due to incorrect template." +
                                            " Uploaded template should match with the column mapping defined for your project.";
                                        myActivitySource.WorkItemCode = Workitemcode;
                                        myActivitySource.SourceRecordID = Convert.ToInt64(ProjectID);
                                        myActivitySource.RaisedOnDate = DateTime.Today;
                                        myActivitySource.RequestorJson = "";
                                        myActivitySource.CreatedBy = "System";
                                        myActivitySource.ActivityTo = item.HcmSupervisorID;
                                        string st = new MyActivity().MySaveActivity(myActivitySource, access);
                                    }
                                }
                            }
                            if (IsCognizant == "1")
                            {
                                return "Dump Upload Failed.Please check e-mail.";
                            }
                            else
                            {
                                return "Template is not matching with ITSM configuration Column mapping. Please upload valid template.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // StringBuilder sbException = new StringBuilder();
                // sbException.Append("Exception:").Append(ex.Message).Append(" Stack Trace:").Append(ex.StackTrace);
                // Utility.ErrorLOG(sbException.ToString(), "Upload Process Not Done", 0);
                throw ex;
            }
            return StrResult;
        }

        /// <summary>
        /// Method to modify ticket summary
        /// </summary>
        /// <param name="dtBulkUpload"></param>
        /// <param name="ticketDescriptionSummary"></param>
        /// <param name="encryptionEnabled"></param>
        /// <param name="index"></param>
        /// <returns>Method returns Ticket summary</returns>

        private DataTable ModifyTicketSummary(DataTable dtBulkUpload, List<TicketDescriptionSummary>
            ticketDescriptionSummary, string encryptionEnabled, int index)
        {
            string strTicketSumvalue = "";
            var EnTicketSummarywithoutTag = "";
            var DeTicketSummarywithoutTag = "";
            AESEncryption aesMod = new AESEncryption();
            for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
            {
                EnTicketSummarywithoutTag = Regex.Replace(string.IsNullOrEmpty(dtBulkUpload.Rows[i][index]
                    .ToString()) ? "" : dtBulkUpload.Rows[i][index].ToString(), @"(\s+|<|>)", " ");
                dtBulkUpload.Rows[i]["IsTicketSummaryModified"] = (dtBulkUpload.Rows[i]["Ticket Summary"]
                    == null || dtBulkUpload.Rows[i]["Ticket Summary"].ToString() == string.Empty ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["Ticket ID"].ToString()
                && sd.SupportType == Convert.ToInt16(dtBulkUpload.Rows[i]["SupportType"]))
                       && ((sd.TicketSummary.Trim().Equals(EnTicketSummarywithoutTag.Trim())) ||
                       (Convert.ToInt16(dtBulkUpload.Rows[i]["SupportType"]) == 3))))
                       ) == true ? "0" : "1";
                if (encryptionEnabled == "Enabled")
                {
                    strTicketSumvalue = string.IsNullOrEmpty(EnTicketSummarywithoutTag) ? "" :
                        Convert.ToBase64String(aesMod.EncryptStringAsBytes(EnTicketSummarywithoutTag,
                        AseKeyDetail.AesKeyConstVal));
                    dtBulkUpload.Rows[i][index] = strTicketSumvalue;
                }
                else
                {
                    DeTicketSummarywithoutTag = Regex.Replace(dtBulkUpload.Rows[i][index].
                        ToString(), @"(\s+|<|>)", " ");
                    strTicketSumvalue = DeTicketSummarywithoutTag;
                    dtBulkUpload.Rows[i][index] = strTicketSumvalue;
                }
            }
            return dtBulkUpload;
        }

        /// <summary>
        /// Method to modify Ticket Description
        /// </summary>
        /// <param name="dtBulkUpload"></param>
        /// <param name="ticketDescriptionSummary"></param>
        /// <param name="encryptionEnabled"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataTable ModifyTicketDescription(DataTable dtBulkUpload, List<TicketDescriptionSummary>
            ticketDescriptionSummary, string encryptionEnabled, int index)
        {
            var EnTicketDescriptionwithoutTag = "";
            var DeTicketDescriptionwithoutTag = "";
            string strTicketDescvalue = "";
            AESEncryption aesMod = new AESEncryption();
            for (int i = 0; i < dtBulkUpload.Rows.Count; i++)
            {
                EnTicketDescriptionwithoutTag = Regex.Replace(string.IsNullOrEmpty(dtBulkUpload.Rows[i][index].
                    ToString()) ? "" : dtBulkUpload.Rows[i][index].ToString(), @"(\s+|<|>)", " ");
                dtBulkUpload.Rows[i]["IsTicketDescriptionModified"] = (dtBulkUpload.Rows[i]["Ticket Description"]
                    == null || dtBulkUpload.Rows[i]["Ticket Description"].ToString() == string.Empty ||
                    (ticketDescriptionSummary.Any(sd => (sd.TicketId == dtBulkUpload.Rows[i]["Ticket ID"].ToString()
                && sd.SupportType == Convert.ToInt16(dtBulkUpload.Rows[i]["SupportType"]))
                       && ((sd.TicketDescription.Trim().Equals(EnTicketDescriptionwithoutTag.Trim())) ||
                       (Convert.ToInt16(dtBulkUpload.Rows[i]["SupportType"]) == 3))))
                       ) == true ? "0" : "1";
                if (encryptionEnabled == "Enabled")
                {
                    strTicketDescvalue = string.IsNullOrEmpty(EnTicketDescriptionwithoutTag) ? ""
                        : Convert.ToBase64String(aesMod.EncryptStringAsBytes
                        (EnTicketDescriptionwithoutTag, AseKeyDetail.AesKeyConstVal));
                    dtBulkUpload.Rows[i][index] = strTicketDescvalue;
                }
                else
                {
                    DeTicketDescriptionwithoutTag = Regex.Replace(dtBulkUpload.Rows[i][index].
                        ToString(), @"(\s+|<|>)", " ");
                    strTicketDescvalue = DeTicketDescriptionwithoutTag;
                    dtBulkUpload.Rows[i][index] = strTicketDescvalue;
                }
            }
            return dtBulkUpload;
        }

        /// <summary>
        /// This Method Is Used To UpdateTicketUploadtrackCommonFields
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ticketUploadtrack"></param>
        private static void UpdateTicketUploadtrackCommonFields(string EmployeeID,
            TicketUploadTrack ticketUploadtrack)
        {
            ticketUploadtrack.CreatedBy = EmployeeID;
            ticketUploadtrack.CreatedDate = DateTimeOffset.Now.DateTime;
            ticketUploadtrack.ModifiedBy = EmployeeID;
            ticketUploadtrack.ModifiedDate = DateTimeOffset.Now.DateTime;
        }
        /// <summary>
        /// This Method Is Used To GendrateKeyValue
        /// </summary>
        /// <param name="LstColumnMappting"></param>
        /// <returns></returns>
        private Dictionary<string, string> GendrateKeyValue(List<ColumnMappting> LstColumnMappting)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var x in LstColumnMappting)
            {
                keyValuePairs.Add(x.ServiceDartColumn.Replace(" ", "").Replace("(", "").Replace(")", ""),
                    x.ServiceDartColumn);
            }
            return keyValuePairs;

        }
        /// <summary>
        /// This Method Is Used To Mail
        /// </summary>
        /// <param name="projectID">This parameter holds projectID value </param>
        /// <param name="CogID">This parameter holds CogID value</param>
        /// <param name="Sharepathusers">This parameter holds Sharepathusers value</param>
        /// <param name="Uploadedcount">This parameter holds Uploadedcount value</param>
        /// <param name="CustomerID">This parameter holds CustomerID value</param>
        private void Mail(string projectID, string CogID, string Sharepathusers, string Uploadedcount,
            string CustomerID)
        {

            EmailDetail emailDetail = new EmailDetail();
            SaveTicketUploadErrors(projectID, CogID, Uploadedcount, CustomerID, excelfilename);

            string strSubject = "Ticket Upload Status as on " + DateTimeOffset.Now.DateTime;
            StringBuilder sbProjectName = new StringBuilder();
            StringBuilder sbProjectId = new StringBuilder();
            string strProcessedFileName = "Processed File Name : " + excelfilename;
            string strAutogeneration = "PS :This is an AutoGenerated Mail.Please do not reply to this mail. ";
            StringBuilder sb = new StringBuilder();
            StringBuilder strbody = new StringBuilder();
            StringBuilder strContent = new StringBuilder();
            string str = "Regards";
            string strsign = "App Lens Team";
            string pid = projectID;
            string isMailer = "";
            StringBuilder strCc = new StringBuilder();
            StringBuilder strName = new StringBuilder();
            string strTo = "";
            try
            {
                string cogid = CogID;
                StringBuilder toUser = new StringBuilder();
                SqlParameter[] prm1 = new SqlParameter[2];
                prm1[0] = new SqlParameter("@EmployeeID", cogid);
                prm1[1] = new SqlParameter("@ProjectID", Convert.ToInt64(pid));
                DataTable dtSharePathUsers = new DataTable();
                dtSharePathUsers.Locale = CultureInfo.InvariantCulture;
                dtSharePathUsers = (new DBHelper()).GetTableFromSP("GetTicketUploadConfigDetails", prm1, ConnectionString);
                toUser.Append(cogid).Append(";");
                string Path = string.Empty;
                for (int i = 0; i < dtSharePathUsers.Rows.Count; i++)
                {
                    Path = dtSharePathUsers.Rows[i]["TicketSharePathUsers"].ToString();
                    isMailer = dtSharePathUsers.Rows[i]["Ismailer"].ToString();
                }
                if (!string.IsNullOrEmpty(Path))
                {
                    toUser.Append(Path);
                }
                if (isMailer.Trim().ToUpper() == "Y")
                {
                    SqlParameter[] prm = new SqlParameter[3];
                    prm[0] = new SqlParameter("@Empid", cogid);
                    prm[1] = new SqlParameter("@projectid", pid);
                    prm[2] = new SqlParameter("@MailTo", toUser.ToString());
                    DataTable maildt = new DataTable();
                    maildt.Locale = CultureInfo.InvariantCulture;
                    maildt = (new DBHelper()).GetTableFromSP("sp_GetMailUserDetails", prm, ConnectionString);
                    if (maildt != null && maildt.Rows.Count > 0)
                    {
                        strName = new StringBuilder();
                        strTo = maildt.Rows[0]["EmployeeEmail"].ToString();
                        strName.Append("Hello ").Append(maildt.Rows[0]["EmployeeName"].ToString()).Append(",");

                        for (int i = 1; i < maildt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(maildt.Rows[i]["EmployeeEmail"].ToString()))
                            {
                                if (i == maildt.Rows.Count - 1)
                                {
                                    strCc.Append(maildt.Rows[i]["EmployeeEmail"].ToString());
                                    strName.Append(maildt.Rows[i]["EmployeeName"].ToString());
                                }
                                else
                                {
                                    strCc.Append(maildt.Rows[i]["EmployeeEmail"].ToString()).Append("; ");
                                    strName.Append(maildt.Rows[i]["EmployeeName"].ToString()).Append(", ");
                                }
                            }
                        }
                        if (strCc.Length == 0)
                        {
                            strCc = new StringBuilder();
                            strCc.Append(strTo);
                        }
                    }

                    SqlParameter[] prms = new SqlParameter[1];
                    prms[0] = new SqlParameter("@projectid", pid);
                    DataTable projectdt = new DataTable();
                    projectdt.Locale = CultureInfo.InvariantCulture;
                    projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                    for (int j = 0; j < projectdt.Rows.Count; j++)
                    {
                        sbProjectName.Append("Project Name : ");
                        sbProjectName.Append(projectdt.Rows[j]["ProjectName"].ToString());
                        sbProjectId.Append("Project ID : ");
                        sbProjectId.Append(projectdt.Rows[j]["EsaProjectID"].ToString());

                    }
                    strbody.Append("Please find the ticket(s) Uploaded status during bulk upload process as below.<BR>")
                        .Append(strProcessedFileName).Append("<BR>");
                    sb.Append(Uploadedcount);

                    if (string.IsNullOrEmpty(strCc.ToString()) || isMailer.ToUpper() == "N")
                    {
                        strCc = new StringBuilder();
                        strCc.Append(strTo);
                    }

                    strContent.Append("<font face=Arial, Helvetica, Sans-Serif size=2>").Append(strName).
                        Append("<BR><BR>").Append(sbProjectName).Append("<BR><BR>").Append(sbProjectId).
                        Append("<BR><BR>").Append(strbody).Append("<BR>").Append(Uploadedcount).Append("<BR><BR>").
                        Append(strAutogeneration).Append("<BR><BR>").Append(str).Append("<BR>").Append(strsign).
                        Append("</font>");
                    emailDetail.To = strTo;
                    emailDetail.From = strFrom;
                    emailDetail.CC = Convert.ToString(strCc);
                    emailDetail.Subject = strSubject;
                    emailDetail.Body = strContent.ToString();
                    SendEmail(emailDetail);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                sbException.Append(ex.Message).Append(strCc).Append(strTo).Append(strFrom);
                objDebtFieldsApprovalRepository.ErrorLOG(sbException.ToString(), "Reached mail",
                    Convert.ToInt64(projectID));
                throw ex;
            }
            AddTask(projectID, CogID, CustomerID, 1);
        }

        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="emailDetail"></param>
        /// <returns></returns>
        public void SendEmail(EmailDetail emailDetail)
        {
            
            try
            {
                    SqlParameter[] prms = new SqlParameter[5];
                    prms[0] = new SqlParameter("@To", emailDetail.To);
                    prms[1] = new SqlParameter("@From", emailDetail.From);
                    prms[2] = new SqlParameter("@CC", emailDetail.CC);
                    prms[3] = new SqlParameter("@Subject", emailDetail.Subject);
                    prms[4] = new SqlParameter("@Body", emailDetail.Body);

                    DataTable dt = (new DBHelper()).GetTableFromSP(StoredProcedure.SendMailSP, prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// AddTask
        /// </summary>
        /// <param name="projectID"></param>
        /// /// <param name="employeeID"></param>
        /// /// <param name="Option"></param>
        /// <returns></returns>
        public void AddTask(string projectID, string CogID, string CustomerID, int Option)
        {
            try
            {
                SqlParameter[] prmsTasks = new SqlParameter[5];
                prmsTasks[0] = new SqlParameter("@ProjectID", projectID);
                prmsTasks[1] = new SqlParameter("@CustomerID", CustomerID);
                prmsTasks[2] = new SqlParameter("@EmployeeID", StrEmployeeID);
                prmsTasks[3] = new SqlParameter("@IsSharepath", false);
                prmsTasks[4] = new SqlParameter("@Option", Option);
                DataTable prjTasks = new DataTable();
                prjTasks.Locale = CultureInfo.InvariantCulture;
                prjTasks = (new DBHelper()).GetTableFromSP("AVL.TaskListUserTicketUploadFailure", prmsTasks, ConnectionString);
                if (prjTasks != null && prjTasks.Rows.Count > 0)
                {
                    List<TaskDetailsList> taskList = new List<TaskDetailsList>();
                    taskList = prjTasks.AsEnumerable().Select(dailyrow => new TaskDetailsList
                    {
                        UserId = dailyrow["UserID"].ToString(),
                        AccountId = Convert.ToInt64(projectID),
                        Application = dailyrow["Application"].ToString(),
                        CreatedBy = StrEmployeeID,
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
                        ModifiedBy = StrEmployeeID,
                        ModifiedTime = DateTimeOffset.Now.DateTime.ToString()


                    }).ToList();

                    MyTaskRepository taskRep = new MyTaskRepository();
                    JArray taskdetailsObj = JArray.FromObject(taskList);

                }

            }
            catch (Exception ex)
            {
                // MyTaskRepository taskRep = new MyTaskRepository();
                // taskRep.ErrorLOG(ex.Message + " Stack trace: " + ex.StackTrace, "Reached My task Upload Rep",
                //   Convert.ToInt64(projectID));
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To Mail1
        /// </summary>
        /// <param name="projectID"> This parameter holds projectID value</param>
        /// <param name="CogID">This parameter holds CogID value</param>
        /// <param name="Sharepathusers">This parameter holds Sharepathusers value</param>
        /// <param name="Uploadedcount">This parameter holds Uploadedcount value</param>
        private void Mail1(string projectID, string CogID, string Sharepathusers, string Uploadedcount)
        {
            EmailDetail emailDetail = new EmailDetail();
            SaveTicketUploadErrors(projectID, CogID, Uploadedcount, "0", excelfilename);

            string strSubject = "Ticket Upload Status as on " + DateTimeOffset.Now.DateTime;

            StringBuilder sbProjectName = new StringBuilder();
            StringBuilder sbProjectId = new StringBuilder();
            string strProcessedFileName = "Processed File Name : " + excelfilename;
            string strAutogeneration = "PS :This is an AutoGenerated Mail.Please do not reply to this mail. ";
            StringBuilder sb = new StringBuilder();
            StringBuilder strbody = new StringBuilder();
            StringBuilder strbodyContent = new StringBuilder();
            string str = "Regards";
            string strsign = "App Lens Team";
            string pid = projectID;
            StringBuilder strCc = new StringBuilder();
            StringBuilder strName = new StringBuilder();
            string strTo = "";
            string isMailer = "";
            try
            {
                string cogid = CogID;
                StringBuilder toUser = new StringBuilder();
                toUser.Append(cogid).Append(";");
                SqlParameter[] prm1 = new SqlParameter[2];
                prm1[0] = new SqlParameter("@EmployeeID", cogid);
                prm1[1] = new SqlParameter("@ProjectID", Convert.ToInt64(pid));
                DataTable dtSharePathUsers = new DataTable();
                dtSharePathUsers.Locale = CultureInfo.InvariantCulture;
                dtSharePathUsers = (new DBHelper()).GetTableFromSP("GetTicketUploadConfigDetails", prm1, ConnectionString);
                for (int i = 0; i < dtSharePathUsers.Rows.Count; i++)
                {
                    Sharepathusers = dtSharePathUsers.Rows[i]["TicketSharePathUsers"].ToString();
                    isMailer = dtSharePathUsers.Rows[i]["Ismailer"].ToString();
                }
                if (!string.IsNullOrEmpty(Sharepathusers))
                {
                    toUser.Append(Sharepathusers);
                }
                if (isMailer.Trim().ToUpper() == "Y")
                {
                    SqlParameter[] prm = new SqlParameter[3];
                    prm[0] = new SqlParameter("@Empid", cogid);
                    prm[1] = new SqlParameter("@projectid", pid);
                    prm[2] = new SqlParameter("@MailTo", toUser.ToString());
                    DataTable maildt = new DataTable();
                    maildt.Locale = CultureInfo.InvariantCulture;
                    maildt = (new DBHelper()).GetTableFromSP("sp_GetMailUserDetails", prm, ConnectionString);
                    if (maildt != null && maildt.Rows.Count > 0)
                    {
                        strTo = maildt.Rows[0]["EmployeeEmail"].ToString();
                        strName.Append("Hello ").Append(maildt.Rows[0]["EmployeeName"].ToString()).Append(",");
                        for (int i = 1; i < maildt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(maildt.Rows[i]["EmployeeEmail"].ToString()))
                            {
                                if (i == maildt.Rows.Count - 1)
                                {
                                    strCc.Append(maildt.Rows[i]["EmployeeEmail"].ToString());
                                    strName.Append(maildt.Rows[i]["EmployeeName"].ToString());
                                }
                                else
                                {
                                    strCc.Append(maildt.Rows[i]["EmployeeEmail"].ToString()).Append("; ");
                                    strName.Append(maildt.Rows[i]["EmployeeName"].ToString()).Append(", ");
                                }
                            }
                        }
                        if (strCc.Length == 0)
                        {
                            strCc = new StringBuilder();
                            strCc.Append(strTo);
                        }
                    }
                    if (maildt != null && maildt.Rows.Count == 1)
                    {
                        strCc = new StringBuilder();
                    }
                    SqlParameter[] prms = new SqlParameter[1];
                    prms[0] = new SqlParameter("@projectid", pid);
                    DataTable projectdt = new DataTable();
                    projectdt.Locale = CultureInfo.InvariantCulture;
                    projectdt = (new DBHelper()).GetTableFromSP("sp_GetProjectDetails", prms, ConnectionString);
                    for (int j = 0; j < projectdt.Rows.Count; j++)
                    {
                        sbProjectName.Append("Project Name : ");
                        sbProjectName.Append(projectdt.Rows[j]["ProjectName"].ToString());
                        sbProjectId.Append("Project ID : ");
                        sbProjectId.Append(projectdt.Rows[j]["EsaProjectID"].ToString());
                    }
                    strbody.Append("Please find the ticket(s) Uploaded status " +
                        "during bulk upload process as below.<BR>").
                        Append(strProcessedFileName).Append("<BR>");
                    sb.Append(Uploadedcount);

                    if (string.IsNullOrEmpty(strCc.ToString()) || isMailer.ToUpper() == "N")
                    {
                        strCc = new StringBuilder();
                        strCc.Append(strTo);
                    }


                    strbodyContent.Append("<font face=Arial, Helvetica, Sans-Serif size=2>").Append(strName).
                     Append("<BR><BR>").Append(sbProjectName).Append("<BR><BR>").Append(sbProjectId).
                     Append("<BR><BR>").Append(strbody).Append("<BR>").Append(Uploadedcount).
                     Append("<BR><BR>").Append(strAutogeneration).
                     Append("<BR><BR>").Append(str).Append("<BR>").Append(strsign).Append("</font>");
                    emailDetail.To = strTo;
                    emailDetail.From = strFrom;
                    emailDetail.CC = Convert.ToString(strCc);
                    emailDetail.Subject = strSubject;
                    emailDetail.Body = strbodyContent.ToString();
                    SendEmail(emailDetail);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                DebtFieldsApprovalRepository objDebtFieldsApprovalRepository = new DebtFieldsApprovalRepository();
                sbException.Append(ex.Message).Append(strCc).Append(strTo).Append(strFrom);
                objDebtFieldsApprovalRepository.ErrorLOG(sbException.ToString(), "Reached mail1",
                    Convert.ToInt64(projectID));
                throw ex;
            }
            AddTask(projectID, CogID, "1", 2);
        }

        /// <summary>
        /// This Method Is Used To DataRowValidations
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string DataRowValidations(DataRow dr)
        {
            StrResult = "";
            StringBuilder sbdate = new StringBuilder();
            if (dr["Open Date"].ToString() != "")
            {
                if (!flgstrErrOpenDate)
                {
                    string strResultopen = "";
                    if (StrResult.Equals(""))
                    {
                        strResultopen = IsDateTime(dr, "Open Date");
                        if (!flgopendate)
                        {
                            sbdate.Append(strResultopen);
                            if (strResultopen != "")
                            {
                                flgopendate = true;
                            }
                        }
                    }
                }
            }

            StrResult = sbdate.ToString();
            return StrResult;
        }
        /// <summary>
        /// This Method Is Used To Validatelength
        /// </summary>
        /// <param name="dtBulk"></param>
        /// <param name="strDest"></param>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private string Validatelength(ref DataTable dtBulk, List<string> strDest, List<string> strSource)
        {
            StringBuilder strValid = new StringBuilder();
            StringBuilder strErr = new StringBuilder();
            bool flgsource = false;
            bool flgsourcedep = false;
            bool flgrootcause = false;
            bool flgrbc = false;
            bool flgsec = false;
            bool flgseverity = false;
            bool flgReleaseType = false;
            bool flgKEDBAvailableIndicator = false;
            bool flgKEDBUpdated = false;
            bool flgElevateFlagInternal = false;
            bool flgRCAID = false;
            bool flgResolvedby = false;
            bool flgTicketID = false;
            bool flgTicketType = false;
            bool flgAssignee = false;
            bool flgPriority = false;
            bool flgStatus = false;
            bool flgApplication = false;
            bool flgClientUserID = false;
            bool flgTicketSummary = false;
            bool flgNatureOfTheTicket = false;
            bool flgTechnology = false;
            bool flgBusinessImpact = false;
            bool flgJobProcessName = false;
            bool flgServerName = false;
            bool flgComments = false;
            bool flgRequesterCustomerId = false;
            bool flgRequesterFirstName = false;
            bool flgRequesterInternetEmail = false;
            bool flgRequesterContactNumber = false;
            bool flgRepeatedIncident = false;
            bool flgRelatedTickets = false;
            bool flgTicketCreatedBy = false;
            bool flgKEDBPath = false;
            bool flgApprovedBy = false;
            bool flgReviewedBy = false;
            bool flgReasonForRejection = false;
            bool flgReasonForCancel = false;
            bool flgReasonForOnHold = false;
            bool flgResponseSLAOverriddenReason = false;
            bool flgResolutionSLAOverriddenReason = false;
            bool flgAcknowledgementSLAOverriddenReason = false;
            bool flgType = false;
            bool flgItem = false;
            bool flgResolutionmethod = false;
            StringBuilder strnonman = new StringBuilder();
            try
            {
                foreach (DataRow dr in dtBulk.Rows)
                {
                    foreach (string str in strSource)
                    {
                        switch (str)
                        {
                            case "Ticket ID":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenTicketID = new StringBuilder();
                                    strlenTicketID.Append("Ticket Upload unsuccessful: ")
                                    .Append("Please use  less than 50 characters for [ ")
                                    .Append(str).Append(" ] Column. <BR>");
                                    if (!flgTicketID)
                                    {
                                        strnonman.Append(strlenTicketID);
                                        if (strlenTicketID.ToString() != "")
                                        {
                                            flgTicketID = true;
                                        }
                                    }
                                }
                                break;

                            case "Ticket Type":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenTicketType = new StringBuilder();
                                    strlenTicketType.Append("Ticket Upload unsuccessful: ")
                                        .Append("Please use  less than 50 characters for [ ")
                                        .Append(str).Append(" ] Column. <BR>");
                                    if (!flgTicketType)
                                    {
                                        strnonman.Append(strlenTicketType);
                                        if (strlenTicketType.ToString() != "")
                                        {
                                            flgTicketType = true;
                                        }
                                    }
                                }
                                break;

                            case "Assignee":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenAssignee = new StringBuilder();
                                    strlenAssignee.Append("Ticket Upload unsuccessful: Please use  less than ")
                                        .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgAssignee)
                                    {
                                        strnonman.Append(strlenAssignee);
                                        if (strlenAssignee.ToString() != "")
                                        {
                                            flgAssignee = true;
                                        }
                                    }
                                }
                                break;

                            case "Priority":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenPriority = new StringBuilder();
                                    strlenPriority.Append("Ticket Upload unsuccessful: Please use  less than ")
                                        .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgPriority)
                                    {
                                        strnonman.Append(strlenPriority);
                                        if (strlenPriority.ToString() != "")
                                        {
                                            flgPriority = true;
                                        }
                                    }
                                }
                                break;

                            case "Status":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenStatus = new StringBuilder();
                                    strlenStatus.Append("Ticket Upload unsuccessful: Please use  less than ")
                                        .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgStatus)
                                    {
                                        strnonman.Append(strlenStatus);
                                        if (strlenStatus.ToString() != "")
                                        {
                                            flgStatus = true;
                                        }
                                    }
                                }
                                break;

                            case "Application":
                                if (dr[str].ToString().Length > 100)
                                {
                                    StringBuilder strlenApplication = new StringBuilder();
                                    strlenApplication.Append("Ticket Upload unsuccessful: Please use  less than ")
                                        .Append("100 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgApplication)
                                    {
                                        strnonman.Append(strlenApplication);
                                        if (strlenApplication.ToString() != "")
                                        {
                                            flgApplication = true;
                                        }
                                    }
                                }
                                break;

                            case "External Login ID":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenClientUserID = new StringBuilder();
                                    strlenClientUserID.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgClientUserID)
                                    {
                                        strnonman.Append(strlenClientUserID);
                                        if (strlenClientUserID.ToString() != "")
                                        {
                                            flgClientUserID = true;
                                        }
                                    }
                                }
                                break;

                            case "Ticket Source":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenSource = new StringBuilder();
                                    strlenSource.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgsource)
                                    {
                                        strnonman.Append(strlenSource);
                                        if (strlenSource.ToString() != "")
                                        {
                                            flgsource = true;
                                        }
                                    }
                                }
                                break;

                            case "Source Department":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenSourcedep = new StringBuilder();
                                    strlenSourcedep.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgsourcedep)
                                    {
                                        strnonman.Append(strlenSourcedep);
                                        if (strlenSourcedep.ToString() != "")
                                        {
                                            flgsourcedep = true;
                                        }
                                    }
                                }
                                break;

                            case "Root Cause":
                                if (dr[str].ToString().Length > 200)
                                {
                                    StringBuilder strlenrootcause = new StringBuilder();
                                    strlenrootcause.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("200 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgrootcause)
                                    {
                                        strnonman.Append(strlenrootcause);
                                        if (strlenrootcause.ToString() != "")
                                        {
                                            flgrootcause = true;
                                        }
                                    }
                                }
                                break;

                            case "Raised By Customer":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenrbc = new StringBuilder();
                                    strlenrbc.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgrbc)
                                    {
                                        strnonman.Append(strlenrbc);
                                        if (strlenrbc.ToString() != "")
                                        {
                                            flgrbc = true;
                                        }
                                    }
                                }
                                break;

                            case "Sec Assignee":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlensec = new StringBuilder();
                                    strlensec.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgsec)
                                    {
                                        strnonman.Append(strlensec);
                                        if (strlensec.ToString() != "")
                                        {
                                            flgsec = true;
                                        }
                                    }
                                }
                                break;

                            case "Severity":
                                if (dr[str].ToString().Length > 20)
                                {

                                    StringBuilder strlenseverity = new StringBuilder();
                                    strlenseverity.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("20 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgseverity)
                                    {
                                        strnonman.Append(strlenseverity);
                                        if (strlenseverity.ToString() != "")
                                        {
                                            flgseverity = true;
                                        }
                                    }
                                }
                                break;

                            case "Release Type":
                                if (dr[str].ToString().Length > 20)
                                {
                                    StringBuilder strlenReleaseType = new StringBuilder();
                                    strlenReleaseType.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("20 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgReleaseType)
                                    {
                                        strnonman.Append(strlenReleaseType);
                                        if (strlenReleaseType.ToString() != "")
                                        {
                                            flgReleaseType = true;
                                        }
                                    }
                                }
                                break;

                            case "KEDB Available Indicator":
                                if (dr[str].ToString().Length > 30)
                                {
                                    StringBuilder strlenKEDBAvailableIndicator = new StringBuilder();
                                    strlenKEDBAvailableIndicator.
                                        Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("30 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgKEDBAvailableIndicator)
                                    {
                                        strnonman.Append(strlenKEDBAvailableIndicator);
                                        if (strlenKEDBAvailableIndicator.ToString() != "")
                                        {
                                            flgKEDBAvailableIndicator = true;
                                        }
                                    }
                                }
                                break;

                            case "KEDB updated":
                                if (dr[str].ToString().Length > 20)
                                {
                                    StringBuilder strlenKEDBUpdated = new StringBuilder();
                                    strlenKEDBUpdated.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 20 characters for [ ").Append(str).Append(" ]Column. <BR>");
                                    if (!flgKEDBUpdated)
                                    {
                                        strnonman.Append(strlenKEDBUpdated);
                                        if (strlenKEDBUpdated.ToString() != "")
                                        {
                                            flgKEDBUpdated = true;
                                        }
                                    }
                                }
                                break;
                            case "RCA ID":
                                if (dr[str].ToString().Length > 20)
                                {
                                    StringBuilder strlenRCAID = new StringBuilder();
                                    strlenRCAID.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("20 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRCAID)
                                    {
                                        strnonman.Append(strlenRCAID);
                                        if (strlenRCAID.ToString() != "")
                                        {
                                            flgRCAID = true;
                                        }
                                    }
                                }
                                break;

                            case "Resolved by":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenResolvedby = new StringBuilder();
                                    strlenResolvedby.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgResolvedby)
                                    {
                                        strnonman.Append(strlenResolvedby);
                                        if (strlenResolvedby.ToString() != "")
                                        {
                                            flgResolvedby = true;
                                        }
                                    }
                                }
                                break;

                            case "Ticket Summary":
                                if (dr[str].ToString().Length > 2000)
                                {
                                    StringBuilder strlenTicketSummary = new StringBuilder();
                                    strlenTicketSummary.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("2000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgTicketSummary)
                                    {
                                        strnonman.Append(strlenTicketSummary);
                                        if (strlenTicketSummary.ToString() != "")
                                        {
                                            flgTicketSummary = true;
                                        }
                                    }
                                }
                                break;

                            case "Nature Of The Ticket":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenNatureOfTheTicket = new StringBuilder();
                                    strlenNatureOfTheTicket.Append("Ticket Upload unsuccessful: Please use less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgNatureOfTheTicket)
                                    {
                                        strnonman.Append(strlenNatureOfTheTicket);
                                        if (strlenNatureOfTheTicket.ToString() != "")
                                        {
                                            flgNatureOfTheTicket = true;
                                        }
                                    }
                                }
                                break;

                            case "Technology":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenTechnology = new StringBuilder();
                                    strlenTechnology.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgTechnology)
                                    {
                                        strnonman.Append(strlenTechnology);
                                        if (strlenTechnology.ToString() != "")
                                        {
                                            flgTechnology = true;
                                        }
                                    }
                                }
                                break;

                            case "Business Impact":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenBusinessImpact = new StringBuilder();
                                    strlenBusinessImpact.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgBusinessImpact)
                                    {
                                        strnonman.Append(strlenBusinessImpact);
                                        if (strlenBusinessImpact.ToString() != "")
                                        {
                                            flgBusinessImpact = true;
                                        }
                                    }
                                }
                                break;

                            case "Job Process Name":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenJobProcessName = new StringBuilder();
                                    strlenJobProcessName.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgJobProcessName)
                                    {
                                        strnonman.Append(strlenJobProcessName);
                                        if (strlenJobProcessName.ToString() != "")
                                        {
                                            flgJobProcessName = true;
                                        }
                                    }
                                }
                                break;

                            case "Server Name":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenServerName = new StringBuilder();
                                    strlenServerName.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgServerName)
                                    {
                                        strnonman.Append(strlenServerName);
                                        if (strlenServerName.ToString() != "")
                                        {
                                            flgServerName = true;
                                        }
                                    }
                                }
                                break;

                            case "Comments":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenComments = new StringBuilder();
                                    strlenComments.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgComments)
                                    {
                                        strnonman.Append(strlenComments);
                                        if (strlenComments.ToString() != "")
                                        {
                                            flgComments = true;
                                        }
                                    }
                                }
                                break;

                            case "Requester Customer Id":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenRequesterCustomerId = new StringBuilder();
                                    strlenRequesterCustomerId.Append("Ticket Upload unsuccessful: Please use  ")
                                    .Append("less than 50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRequesterCustomerId)
                                    {
                                        strnonman.Append(strlenRequesterCustomerId);
                                        if (strlenRequesterCustomerId.ToString() != "")
                                        {
                                            flgRequesterCustomerId = true;
                                        }
                                    }
                                }
                                break;

                            case "Requester First Name":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenRequesterFirstName = new StringBuilder();
                                    strlenRequesterFirstName.Append("Ticket Upload unsuccessful: Please use  less ")
                                    .Append("than 250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRequesterFirstName)
                                    {
                                        strnonman.Append(strlenRequesterFirstName);
                                        if (strlenRequesterFirstName.ToString() != "")
                                        {
                                            flgRequesterFirstName = true;
                                        }
                                    }
                                }
                                break;

                            case "Requester Internet Email":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenRequesterInternetEmail = new StringBuilder();
                                    strlenRequesterInternetEmail.Append("Ticket Upload unsuccessful: Please use  ")
                                    .Append("less than 250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRequesterInternetEmail)
                                    {
                                        strnonman.Append(strlenRequesterInternetEmail);
                                        if (strlenRequesterInternetEmail.ToString() != "")
                                        {
                                            flgRequesterInternetEmail = true;
                                        }
                                    }
                                }
                                break;

                            case "Requester Contact Number":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenRequesterContactNumber = new StringBuilder();
                                    strlenRequesterContactNumber.Append("Ticket Upload unsuccessful: Please use  ")
                                    .Append("less than 50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRequesterContactNumber)
                                    {
                                        strnonman.Append(strlenRequesterContactNumber);
                                        if (strlenRequesterContactNumber.ToString() != "")
                                        {
                                            flgRequesterContactNumber = true;
                                        }
                                    }
                                }
                                break;

                            case "Repeated Incident":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenRepeatedIncident = new StringBuilder();
                                    strlenRepeatedIncident.Append("Ticket Upload unsuccessful: Please use  ")
                                    .Append("less than 50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRepeatedIncident)
                                    {
                                        strnonman.Append(strlenRepeatedIncident);
                                        if (strlenRepeatedIncident.ToString() != "")
                                        {
                                            flgRepeatedIncident = true;
                                        }
                                    }
                                }
                                break;

                            case "Related Tickets":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenRelatedTickets = new StringBuilder();
                                    strlenRelatedTickets.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgRelatedTickets)
                                    {
                                        strnonman.Append(strlenRelatedTickets);
                                        if (strlenRelatedTickets.ToString() != "")
                                        {
                                            flgRelatedTickets = true;
                                        }
                                    }
                                }
                                break;

                            case "Ticket Created By":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenTicketCreatedBy = new StringBuilder();
                                    strlenTicketCreatedBy.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgTicketCreatedBy)
                                    {
                                        strnonman.Append(strlenTicketCreatedBy);
                                        if (strlenTicketCreatedBy.ToString() != "")
                                        {
                                            flgTicketCreatedBy = true;
                                        }
                                    }
                                }
                                break;

                            case "KEDB Path":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenKEDBPath = new StringBuilder();
                                    strlenKEDBPath.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgKEDBPath)
                                    {
                                        strnonman.Append(strlenKEDBPath);
                                        if (strlenKEDBPath.ToString() != "")
                                        {
                                            flgKEDBPath = true;
                                        }
                                    }
                                }
                                break;

                            case "Approved By":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenApprovedBy = new StringBuilder();
                                    strlenApprovedBy.Append("Ticket Upload unsuccessful: Please use  less than")
                                        .Append(" 50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgApprovedBy)
                                    {
                                        strnonman.Append(strlenApprovedBy);
                                        if (strlenApprovedBy.ToString() != "")
                                        {
                                            flgApprovedBy = true;
                                        }
                                    }
                                }
                                break;

                            case "Reviewed By":
                                if (dr[str].ToString().Length > 50)
                                {
                                    StringBuilder strlenReviewedBy = new StringBuilder();
                                    strlenReviewedBy.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 50 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgReviewedBy)
                                    {
                                        strnonman.Append(strlenReviewedBy);
                                        if (strlenReviewedBy.ToString() != "")
                                        {
                                            flgReviewedBy = true;
                                        }
                                    }
                                }
                                break;

                            case "Reason For Rejection":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenReasonForRejection = new StringBuilder();
                                    strlenReasonForRejection.Append("Ticket Upload unsuccessful: Please use  less")
                                    .Append(" than 1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgReasonForRejection)
                                    {
                                        strnonman.Append(strlenReasonForRejection);
                                        if (strlenReasonForRejection.ToString() != "")
                                        {
                                            flgReasonForRejection = true;
                                        }
                                    }
                                }
                                break;

                            case "Reason For Cancel":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenReasonForCancel = new StringBuilder();
                                    strlenReasonForCancel.Append("Ticket Upload unsuccessful: Please use  less ")
                                    .Append("than 1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgReasonForCancel)
                                    {
                                        strnonman.Append(strlenReasonForCancel);
                                        if (strlenReasonForCancel.ToString() != "")
                                        {
                                            flgReasonForCancel = true;
                                        }
                                    }
                                }
                                break;

                            case "Reason For On Hold":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenReasonForOnHold = new StringBuilder();
                                    strlenReasonForOnHold.Append("Ticket Upload unsuccessful: Please use  less ")
                                    .Append("than 1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgReasonForOnHold)
                                    {
                                        strnonman.Append(strlenReasonForOnHold);
                                        if (strlenReasonForOnHold.ToString() != "")
                                        {
                                            flgReasonForOnHold = true;
                                        }
                                    }
                                }
                                break;

                            case "Response SLA Overridden Reason":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenResponseSLAOverriddenReason = new StringBuilder();
                                    strlenResponseSLAOverriddenReason.Append("Ticket Upload unsuccessful: Please use ")
                                    .Append(" less than 1000 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgResponseSLAOverriddenReason)
                                    {
                                        strnonman.Append(strlenResponseSLAOverriddenReason);
                                        if (strlenResponseSLAOverriddenReason.ToString() != "")
                                        {
                                            flgResponseSLAOverriddenReason = true;
                                        }
                                    }
                                }
                                break;

                            case "Resolution SLA Overridden Reason":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenResolutionSLAOverriddenReason = new StringBuilder();
                                    strlenResolutionSLAOverriddenReason.Append("Ticket Upload unsuccessful: Please ")
                                    .Append("use  less than 1000 characters for [ ").Append(str)
                                    .Append(" ] Column. <BR>");
                                    if (!flgResolutionSLAOverriddenReason)
                                    {
                                        strnonman.Append(strlenResolutionSLAOverriddenReason);
                                        if (strlenResolutionSLAOverriddenReason.ToString() != "")
                                        {
                                            flgResolutionSLAOverriddenReason = true;
                                        }
                                    }
                                }
                                break;

                            case "Acknowledgement SLA Overridden Reason":
                                if (dr[str].ToString().Length > 1000)
                                {
                                    StringBuilder strlenAcknowledgementSLAOverriddenReason = new StringBuilder();
                                    strlenAcknowledgementSLAOverriddenReason.Append("Ticket Upload unsuccessful:")
                                    .Append(" Please use  less than 1000 characters for [ ").Append(str)
                                    .Append(" ] Column. <BR>");
                                    if (!flgAcknowledgementSLAOverriddenReason)
                                    {
                                        strnonman.Append(strlenAcknowledgementSLAOverriddenReason);
                                        if (strlenAcknowledgementSLAOverriddenReason.ToString() != "")
                                        {
                                            flgAcknowledgementSLAOverriddenReason = true;
                                        }
                                    }
                                }
                                break;

                            case "Type":
                            case "Category":
                            case "Flex Field (1)":
                            case "Flex Field (2)":
                            case "Flex Field (3)":
                            case "Flex Field (4)":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenType = new StringBuilder();
                                    strlenType.Append("Ticket Upload unsuccessful: Please use  less than ")
                                    .Append("250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgType)
                                    {
                                        strnonman.Append(strlenType);
                                        if (strlenType.ToString() != "")
                                        {
                                            flgType = true;
                                        }
                                    }
                                }
                                break;

                            case "Item":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenItem = new StringBuilder();
                                    strlenItem.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgItem)
                                    {
                                        strnonman.Append(strlenItem);
                                        if (strlenItem.ToString() != "")
                                        {
                                            flgItem = true;
                                        }
                                    }
                                }
                                break;
                            case "Resolution Method":
                                if (dr[str].ToString().Length > 250)
                                {
                                    StringBuilder strlenItem = new StringBuilder();
                                    strlenItem.Append("Ticket Upload unsuccessful: Please use  less than")
                                    .Append(" 250 characters for [ ").Append(str).Append(" ] Column. <BR>");
                                    if (!flgResolutionmethod)
                                    {
                                        strnonman.Append(strlenItem);
                                        if (strlenItem.ToString() != "")
                                        {
                                            flgResolutionmethod = true;
                                        }
                                    }
                                }
                                break;

                            default:
                                //CCAP FIX
                                break;
                        }
                    }
                }
                strValid = strnonman;
                return strValid.ToString();
            }
            catch (SqlException ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                MailstrResult = "Bulk Upload Failed with Error: " + ex.Message;
                return StrResult;
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To ValidateNoNManBulkData
        /// </summary>
        /// <param name="dtBulk"></param>
        /// <param name="strDest"></param>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private string ValidateNoNManBulkData(ref DataTable dtBulk, List<string> strDest, List<string> strSource)
        {
            StringBuilder strErr = new StringBuilder();
            StringBuilder strValid = new StringBuilder();
            StringBuilder strnonman = new StringBuilder();
            if (dtBulk.Columns.Contains("Sla Miss"))
            {
                dtBulk.Columns["Sla Miss"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("Met Response SLA"))
            {
                dtBulk.Columns["Met Response SLA"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("Met Acknowledgement SLA"))
            {
                dtBulk.Columns["Met Acknowledgement SLA"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("Met Resolution"))
            {
                dtBulk.Columns["Met Resolution"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("KEDB Available Indicator"))
            {
                dtBulk.Columns["KEDB Available Indicator"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("Residual Debt"))
            {
                dtBulk.Columns["Residual Debt"].DataType = typeof(string);
            }
            if (dtBulk.Columns.Contains("Avoidable Flag"))
            {
                dtBulk.Columns["Avoidable Flag"].DataType = typeof(string);
            }
            try
            {
                foreach (DataRow dr in dtBulk.Rows)
                {
                    foreach (string str in strSource)
                    {
                        switch (str)
                        {
                            case "Turn around Time":
                                if (dr[str].ToString() != "")
                                {
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgTurntime)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive")
                                                .Append(" decimal (+ve) in the[").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgTurntime = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgTurntime)
                                        {
                                            strErr.Append("Ticket Upload unsuccessful: Please use positive")
                                             .Append(" decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strErr);
                                            flgTurntime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Modified Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strResultmod = "";
                                    strResultmod = IsDateTime(dr, str);
                                    if (!flgmoddate)
                                    {
                                        strnonman.Append(strResultmod);
                                        if (strResultmod != "")
                                        {
                                            flgmoddate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Expected Completion Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strExptCompldate = "";
                                    strExptCompldate = IsDateTime(dr, str);
                                    if (!flgexpcomdate)
                                    {
                                        strnonman.Append(strExptCompldate);
                                        if (strExptCompldate != "")
                                        {
                                            flgexpcomdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Planned Effort":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strPlanEff = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgplaneffort)
                                            {
                                                strPlanEff.Append("Ticket Upload unsuccessful: Please use positive ")
                                               .Append("decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strPlanEff);
                                                flgplaneffort = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 8000)
                                        {
                                            if (!flgplaneffort)
                                            {
                                                strPlanEff.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal (+ve) less than 8000  in the [")
                                                .Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strPlanEff);
                                                flgplaneffort = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgplaneff)
                                        {
                                            strPlanEff.Append("Ticket Upload unsuccessful: Please use positive")
                                            .Append(" decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strPlanEff);
                                            flgplaneff = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;


                            case "Actual Work Size":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strActwor = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgActaleff)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive")
                                               .Append(" decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgActaleff = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgActwor)
                                        {
                                            strActwor.Append("Ticket Upload unsuccessful: Please use positive ")
                                            .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strActwor);
                                            flgActwor = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Estimated Work Size":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strESTwor = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgEstworks)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgEstworks = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgESTwor)
                                        {
                                            strESTwor.Append("Ticket Upload unsuccessful: Please use positive ")
                                            .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strESTwor);
                                            flgESTwor = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Response Time":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strrestime = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgrestime)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal  (+ve) in the [").Append(str).
                                                Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgrestime = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgresptime)
                                        {
                                            strrestime.Append("Ticket Upload unsuccessful: Please use positive")
                                            .Append(" decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strrestime);
                                            flgresptime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Planned Duration":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strPlandur = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));

                                        dr[str] = Math.Round(Convert.ToDecimal(ConvertDecimal(dr[str])), 5);
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgplanduration)
                                            {
                                                strPlandur.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strPlandur);
                                                flgplanduration = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 8000)
                                        {
                                            if (!flgplanduration)
                                            {
                                                strPlandur.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal  (+ve) less than 8000  ")
                                                .Append("in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strPlandur);
                                                flgplanduration = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgplanduration)
                                        {
                                            strPlandur.Append("Ticket Upload unsuccessful: ")
                                            .Append("Please use positive decimal  (+ve) in the")
                                            .Append(" [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strPlandur);
                                            flgplanduration = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Outage Duration":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strOutagedur = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgoutageduration)
                                            {
                                                strOutagedur.Append("Ticket Upload unsuccessful: ")
                                                .Append("Please use positive decimal  (+ve) in the")
                                                .Append(" [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strOutagedur);
                                                flgoutageduration = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 8000)
                                        {
                                            if (!flgoutageduration)
                                            {
                                                strOutagedur.Append("Ticket Upload unsuccessful: ")
                                                .Append("Please use positive decimal  (+ve) less than 8000  in ")
                                                .Append("the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strOutagedur);
                                                flgoutageduration = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgoutageduration)
                                        {
                                            strOutagedur.Append("Ticket Upload unsuccessful: Please use positive")
                                            .Append(" decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strOutagedur);
                                            flgoutageduration = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Actual duration":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strActdur = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = Math.Round(Convert.ToDecimal(ConvertDecimal(dr[str])), 5);
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgactdur)
                                            {
                                                strActdur.Append("Ticket Upload unsuccessful: Please use positive")
                                               .Append(" decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strActdur);
                                                flgactdur = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 8000)
                                        {
                                            if (!flgactdur)
                                            {
                                                strActdur.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal  (+ve) less than 8000  in ")
                                                .Append("the [").Append(str).Append("] Column. \n");
                                                strnonman.Append(strActdur);
                                                flgactdur = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgactdur)
                                        {
                                            strActdur.Append("Ticket Upload unsuccessful: Please use positive ")
                                            .Append("decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strActdur);
                                            flgactdur = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Reopen Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrreopen = "";
                                    strErrreopen = IsDateTime(dr, str);
                                    if (!flgreopendate)
                                    {
                                        strnonman.Append(strErrreopen);
                                        if (strErrreopen != "")
                                        {
                                            flgreopendate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Close Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrclosedate = "";
                                    strErrclosedate = IsDateTime(dr, str);
                                    if (!flgclosedate)
                                    {
                                        strnonman.Append(strErrclosedate);
                                        if (strErrclosedate != "")
                                        {
                                            flgclosedate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Planned End Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrplandate = "";
                                    strErrplandate = IsDateTime(dr, str);
                                    if (!flgplandate)
                                    {
                                        strnonman.Append(strErrplandate);
                                        if (strErrplandate != "")
                                        {
                                            flgplandate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Planned Start Date and Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrplanstrdate = "";
                                    strErrplanstrdate = IsDateTime(dr, str);
                                    if (!flgplanstrdate)
                                    {
                                        strnonman.Append(strErrplanstrdate);
                                        if (strErrplanstrdate != "")
                                        {
                                            flgplanstrdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "New Status Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrnewstrdate = "";
                                    strErrnewstrdate = IsDateTime(dr, str);
                                    if (!flgnewstrdate)
                                    {
                                        strnonman.Append(strErrnewstrdate);
                                        if (strErrnewstrdate != "")
                                        {
                                            flgnewstrdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Resolved date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrresdate = "";
                                    strErrresdate = IsDateTime(dr, str);
                                    if (!flgresdate)
                                    {
                                        strnonman.Append(strErrresdate);
                                        if (strErrresdate != "")
                                        {
                                            flgresdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Rejected Time Stamp":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrrejtimdate = "";
                                    strErrrejtimdate = IsDateTime(dr, str);
                                    if (!flgrejtimdate)
                                    {
                                        strnonman.Append(strErrrejtimdate);
                                        if (strErrrejtimdate != "")
                                        {
                                            flgrejtimdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Release Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrreldate = "";
                                    strErrreldate = IsDateTime(dr, str);
                                    if (!flgreldate)
                                    {
                                        strnonman.Append(strErrreldate);
                                        if (strErrreldate != "")
                                        {
                                            flgreldate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Actual Start date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrActstrdate = "";
                                    strErrActstrdate = IsDateTime(dr, str);
                                    if (!flgActstrdate)
                                    {
                                        strnonman.Append(strErrActstrdate);
                                        if (strErrActstrdate != "")
                                        {
                                            flgActstrdate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Actual End date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrActenddate = "";
                                    strErrActenddate = IsDateTime(dr, str);
                                    if (!flgActenddate)
                                    {
                                        strnonman.Append(strErrActenddate);
                                        if (strErrActenddate != "")
                                        {
                                            flgActenddate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Assigned Time Stamp":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrAssigndate = "";
                                    strErrAssigndate = IsDateTime(dr, str);
                                    if (!flgAssigndate)
                                    {
                                        strnonman.Append(strErrAssigndate);
                                        if (strErrAssigndate != "")
                                        {
                                            flgAssigndate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Sla Miss":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "");
                                    bool X1 = (dr[str].ToString() == "N") ||
                                        (dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString() == "NO");
                                    bool X2 = X1 ||
                                        (dr[str].ToString().ToUpper() == "MET") || (dr[str].ToString() == "NOTMET");

                                    if ((dr[str].ToString() == "0") || (dr[str].ToString() == "1") ||
                                        (dr[str].ToString() == "Y") || X2)
                                    {
                                        if ((dr[str].ToString() == "1") || (dr[str].ToString() == "Y") ||
                                            (dr[str].ToString() == "YES") || (dr[str].ToString() == "NOTMET"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "0") || (dr[str].ToString() == "N") ||
                                            (dr[str].ToString() == "NO") || (dr[str].ToString() == "MET"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strErrSLA = new StringBuilder();
                                        strErrSLA.Append("Ticket Upload unsuccessful: Please use 'Yes' or 'No' in")
                                        .Append(" [ SLA Miss ] Column. <BR>");
                                        if (!flgsSLAErr)
                                        {
                                            strnonman.Append(strErrSLA);
                                            if (strErrSLA.ToString() != "")
                                            {
                                                flgsSLAErr = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Met Response SLA":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "");
                                    bool w1 = (dr[str].ToString() == "0") || (dr[str].ToString() == "1") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N");
                                    bool w2 = (dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString().ToUpper() == "MET") || (dr[str].ToString() == "NOTMET");
                                    bool w3 = (dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL");
                                    if (w1 || w2 || w3 || (dr[str].ToString() == "NA"))
                                    {
                                        if ((dr[str].ToString() == "0") || (dr[str].ToString() == "Y") ||
                                            (dr[str].ToString() == "YES") || (dr[str].ToString() == "MET"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "1") || (dr[str].ToString() == "N") ||
                                            (dr[str].ToString() == "NO") || (dr[str].ToString() == "NOTMET"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else if ((dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL")
                                            || (dr[str].ToString() == "NA"))
                                        {
                                            dr[str] = "";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder sMetSLAErr = new StringBuilder();
                                        sMetSLAErr.Append("Ticket Upload unsuccessful: Please use 'Yes' or 'No' in")
                                        .Append(" [ Met response SLA ] Column. <BR>");
                                        if (!flgssMetresSLAErr)
                                        {
                                            strnonman.Append(sMetSLAErr);
                                            if (sMetSLAErr.ToString() != "")
                                            {
                                                flgssMetresSLAErr = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Met Acknowledgement SLA":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "");
                                    bool q1 = (dr[str].ToString() == "0") || (dr[str].ToString() == "1") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N");
                                    bool q2 = (dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString().ToUpper() == "MET") || (dr[str].ToString() == "NOTMET");
                                    bool q3 = (dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL") ||
                                        (dr[str].ToString() == "NA");

                                    if (q1 || q2 || q3)
                                    {
                                        if ((dr[str].ToString() == "0") || (dr[str].ToString() == "Y") ||
                                            (dr[str].ToString() == "YES") || (dr[str].ToString() == "MET"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "1") || (dr[str].ToString() == "N") ||
                                            (dr[str].ToString() == "NO") || (dr[str].ToString() == "NOTMET"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else if ((dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL") ||
                                            (dr[str].ToString() == "NA"))
                                        {
                                            dr[str] = "";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder sMetAckSLAErr = new StringBuilder();
                                        sMetAckSLAErr.Append("Ticket Upload unsuccessful: Please use 'Yes' or 'No'")
                                        .Append(" in [ Met acknowledgment SLA ] Column. <BR>");
                                        if (!flgssMetAckSLAErr)
                                        {
                                            strnonman.Append(sMetAckSLAErr);
                                            if (sMetAckSLAErr.ToString() != "")
                                            {
                                                flgssMetAckSLAErr = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Met Resolution":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "");
                                    bool a1 = (dr[str].ToString() == "0") || (dr[str].ToString() == "1") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N");
                                    bool a2 = (dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString().ToUpper() == "MET") || (dr[str].ToString() == "NOTMET");
                                    bool a3 = (dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL") ||
                                        (dr[str].ToString() == "NA");
                                    if (a1 || a2 || a3)
                                    {
                                        if ((dr[str].ToString() == "0") || (dr[str].ToString() == "Y") ||
                                            (dr[str].ToString() == "YES") || (dr[str].ToString() == "MET"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "1") || (dr[str].ToString() == "N") ||
                                            (dr[str].ToString() == "NO") || (dr[str].ToString() == "NOTMET"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else if ((dr[str].ToString() == "DEFAULT") || (dr[str].ToString() == "NULL") ||
                                            (dr[str].ToString() == "NA"))
                                        {
                                            dr[str] = "";
                                        }
                                        else
                                        {
                                            //madatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder sMetresSLAErr = new StringBuilder();
                                        sMetresSLAErr.Append("Ticket Upload unsuccessful: Please use 'Yes' or 'No' ")
                                        .Append("in [ Met resolution Column ]. <BR>");
                                        if (!flgssMetreslSLAErr)
                                        {
                                            strnonman.Append(sMetresSLAErr);
                                            if (sMetresSLAErr.ToString() != "")
                                            {
                                                flgssMetreslSLAErr = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Elevate Flag Internal":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "");
                                    if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "NO"))
                                    {
                                        if ((dr[str].ToString() == "YES"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "NO"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder selvErr = new StringBuilder();
                                        selvErr.Append("Ticket Upload unsuccessful: Please use 'Yes' or 'No' in")
                                        .Append(" [ ElevateFlagInternal ] Column. <BR>");
                                        if (!flgelvErr)
                                        {
                                            strnonman.Append(selvErr);
                                            if (selvErr.ToString() != "")
                                            {
                                                flgelvErr = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "KEDB updated":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "ADDED") || (dr[str].ToString() == "UPDATED"))
                                    {
                                        if ((dr[str].ToString() == "ADDED"))
                                        {
                                            dr[str] = "Added";
                                        }
                                        else if ((dr[str].ToString() == "UPDATED"))
                                        {
                                            dr[str] = "Updated";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strkedbup = new StringBuilder();
                                        strkedbup.
                                            Append("Ticket Upload unsuccessful: Please use 'Added' or 'Updated' ")
                                        .Append("in [ KEDBUpdated ] Column. <BR>");
                                        if (!flgkedbup)
                                        {
                                            strnonman.Append(strkedbup);
                                            if (strkedbup.ToString() != "")
                                            {
                                                flgkedbup = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Nature Of The Ticket":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "BATCH") || (dr[str].ToString() == "ONLINE"))
                                    {
                                        if ((dr[str].ToString() == "BATCH"))
                                        {
                                            dr[str] = "Batch";
                                        }
                                        else if ((dr[str].ToString() == "ONLINE"))
                                        {
                                            dr[str] = "Online";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strNatoftic = new StringBuilder();
                                        strNatoftic.Append("Ticket Upload unsuccessful: Please use 'Batch' or")
                                        .Append(" 'Online' in [ NatureOfTheTicket ] Column. <BR>");
                                        if (!flgNatoftic)
                                        {
                                            strnonman.Append(strNatoftic);
                                            if (strNatoftic.ToString() != "")
                                            {
                                                flgNatoftic = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Escalated Flag Customer":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N"))
                                    {
                                        if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "Y"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "NO") || (dr[str].ToString() == "N"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strEsc = new StringBuilder();
                                        strEsc.Append("Ticket Upload unsuccessful: Please use 'YES' or 'NO' in ")
                                        .Append("[ EscalatedFlagCustomer ] Column. <BR>");
                                        if (!flgEsc)
                                        {
                                            strnonman.Append(strEsc);
                                            if (strEsc.ToString() != "")
                                            {
                                                flgEsc = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Outage Flag":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N"))
                                    {
                                        if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "Y"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "NO") || (dr[str].ToString() == "N"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder stroutflg = new StringBuilder();
                                        stroutflg.Append("Ticket Upload unsuccessful: Please use 'YES' or 'NO' in")
                                        .Append(" [ OutageFlag ] Column. <BR>");
                                        if (!flgout)
                                        {
                                            strnonman.Append(stroutflg);
                                            if (stroutflg.ToString() != "")
                                            {
                                                flgout = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Warranty Issue":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "NO") ||
                                        (dr[str].ToString() == "Y") || (dr[str].ToString() == "N"))
                                    {
                                        if ((dr[str].ToString() == "YES") || (dr[str].ToString() == "Y"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "NO") || (dr[str].ToString() == "N"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strwarIss = new StringBuilder();
                                        strwarIss.Append("Ticket Upload unsuccessful: Please use 'YES' or 'NO' in")
                                        .Append(" [ WarrantyIssue ] Column. <BR>");
                                        if (!flgwarIss)
                                        {
                                            strnonman.Append(strwarIss);
                                            if (strwarIss.ToString() != "")
                                            {
                                                flgwarIss = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Release Type":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString() == "NORMAL") || (dr[str].ToString() == "EMERGENCY"))
                                    {
                                        if ((dr[str].ToString() == "NORMAL"))
                                        {
                                            dr[str] = "Normal";
                                        }
                                        else if ((dr[str].ToString() == "EMERGENCY"))
                                        {
                                            dr[str] = "Emergency";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strreltype = new StringBuilder();
                                        strreltype.Append("Ticket Upload unsuccessful: Please use 'NORMAL' or ")
                                        .Append("'EMERGENCY' in [ ReleaseType ] Column. <BR>");
                                        if (!flgreltype)
                                        {
                                            strnonman.Append(strreltype);
                                            if (strreltype.ToString() != "")
                                            {
                                                flgreltype = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case "Requested Resolution Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrRequestedResolutionDateTime = "";
                                    strErrRequestedResolutionDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrRequestedResolutionDateTime)
                                    {
                                        strnonman.Append(strErrRequestedResolutionDateTime);
                                        if (strErrRequestedResolutionDateTime != "")
                                        {
                                            flgstrErrRequestedResolutionDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "CSAT Score":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strCSATScore = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgstrCSATScore)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgstrCSATScore = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 10)
                                        {
                                            if (!flgstrCSATScore)
                                            {
                                                strErr.Append("Ticket Upload unsuccessful: Please use positive")
                                                .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strErr);
                                                flgstrCSATScore = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgstrCSATScore)
                                        {
                                            strCSATScore.Append("Ticket Upload unsuccessful: Please use positive")
                                            .Append(" decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strCSATScore);
                                            flgstrCSATScore = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Approved Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrApprovedDateTime = "";
                                    strErrApprovedDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrstrErrApprovedDateTime)
                                    {
                                        strnonman.Append(strErrApprovedDateTime);
                                        if (strErrApprovedDateTime != "")
                                        {
                                            flgstrErrstrErrApprovedDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;
                            case "Open Date":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrOpenDate = "";
                                    strErrOpenDate = IsDateTime(dr, str);
                                    if (!flgstrErrOpenDate)
                                    {
                                        strnonman.Append(strErrOpenDate);
                                        if (strErrOpenDate != "")
                                        {
                                            flgstrErrOpenDate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Reviewed Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrReviewedDateTime = "";
                                    strErrReviewedDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrReviewedDateTime)
                                    {
                                        strnonman.Append(strErrReviewedDateTime);
                                        if (strErrReviewedDateTime != "")
                                        {
                                            flgstrErrReviewedDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Started Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrStartedDateTime = "";
                                    strErrStartedDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrStartedDateTime)
                                    {
                                        strnonman.Append(strErrStartedDateTime);
                                        if (strErrStartedDateTime != "")
                                        {
                                            flgstrErrStartedDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "WIP Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrWIPDateTime = "";
                                    strErrWIPDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrWIPDateTime)
                                    {
                                        strnonman.Append(strErrWIPDateTime);
                                        if (strErrWIPDateTime != "")
                                        {
                                            flgstrErrWIPDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "On Hold Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrOnHoldDateTime = "";
                                    strErrOnHoldDateTime = IsDateTime(dr, str);
                                    if (!flgOnHoldDateTime)
                                    {
                                        strnonman.Append(strErrOnHoldDateTime);
                                        if (strErrOnHoldDateTime != "")
                                        {
                                            flgOnHoldDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Completed Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrCompletedDateTime = "";
                                    strErrCompletedDateTime = IsDateTime(dr, str);
                                    if (!flgCompletedDateTime)
                                    {
                                        strnonman.Append(strErrCompletedDateTime);
                                        if (strErrCompletedDateTime != "")
                                        {
                                            flgCompletedDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;

                            case "Cancelled Date Time":
                                if (dr[str].ToString() != "")
                                {
                                    string strErrCancelledDateTime = "";
                                    strErrCancelledDateTime = IsDateTime(dr, str);
                                    if (!flgstrErrCancelledDateTime)
                                    {
                                        strnonman.Append(strErrCancelledDateTime);
                                        if (strErrCancelledDateTime != "")
                                        {
                                            flgstrErrCancelledDateTime = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDateTime("01-01-1900").ToString();
                                }
                                break;
                            case "Residual Debt":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString().
                                        ToUpper() == "NO") || (dr[str].ToString().ToUpper() == "Y") ||
                                        (dr[str].ToString().ToUpper() == "N"))
                                    {
                                        if ((dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString().
                                            ToUpper() == "Y"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString() == "NO") || (dr[str].ToString() == "N"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strEsc = new StringBuilder();
                                        strEsc.Append("Ticket Upload unsuccessful: Please use 'YES' or 'NO' in ")
                                        .Append("[ ResidualDebt ] Column. <BR>");
                                        if (!flgEsc)
                                        {
                                            strnonman.Append(strEsc);
                                            if (strEsc.ToString() != "")
                                            {
                                                flgEsc = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "ITSM Effort":
                                if (dr[str].ToString() != "")
                                {
                                    StringBuilder strITSMEff = new StringBuilder();
                                    try
                                    {
                                        decimal PEF = Convert.ToDecimal(ConvertDecimal(dr[str]));
                                        dr[str] = PEF;
                                        if (PEF < 0)
                                        {
                                            if (!flgITSMffort)
                                            {
                                                strITSMEff.Append("Ticket Upload unsuccessful: Please use positive ")
                                               .Append("decimal  (+ve) in the [").Append(str).Append("] Column. <BR>");
                                                strnonman.Append(strITSMEff);
                                                flgITSMffort = true;
                                                break;
                                            }
                                        }
                                        else if (PEF > 8000)
                                        {
                                            if (!flgITSMffort)
                                            {
                                                strITSMEff.Append("Ticket Upload unsuccessful: Please use positive ")
                                                .Append("decimal (+ve) less than 8000  in the [").Append(str)
                                                .Append("] Column. <BR>");
                                                strnonman.Append(strITSMEff);
                                                flgITSMffort = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (!flgplaneff)
                                        {
                                            strITSMEff.Append("Ticket Upload unsuccessful: Please use positive ")
                                            .Append("decimal (+ve) in the [").Append(str).Append("] Column. <BR>");
                                            strnonman.Append(strITSMEff);
                                            flgITSMffort = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dr[str] = Convert.ToDecimal("0.00").ToString();
                                }
                                break;

                            case "Avoidable Flag":

                                if (dr[str].ToString() != "")
                                {
                                    dr[str] = Convert.ToString(dr[str]).ToUpper().Replace(" ", "").Trim();
                                    if ((dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString().ToUpper()
                                        == "NO") || (dr[str].ToString().ToUpper() == "Y") || (dr[str].ToString()
                                        .ToUpper() == "N"))
                                    {
                                        if ((dr[str].ToString().ToUpper() == "YES") || (dr[str].ToString().ToUpper()
                                            == "Y"))
                                        {
                                            dr[str] = "Yes";
                                        }
                                        else if ((dr[str].ToString().ToUpper() == "NO") || (dr[str].ToString().ToUpper()
                                            == "N"))
                                        {
                                            dr[str] = "No";
                                        }
                                        else
                                        {
                                            //mandatory else
                                        }
                                    }
                                    else
                                    {
                                        StringBuilder strEsc = new StringBuilder();
                                        strEsc.Append("Ticket Upload unsuccessful: Please use 'YES' or 'NO' ")
                                        .Append("in [ AvoidableFlag ] Column. <BR>");
                                        if (!flgEsc)
                                        {
                                            strnonman.Append(strEsc);
                                            if (strEsc.ToString() != "")
                                            {
                                                flgEsc = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                //CCAP FIX
                                break;
                        }
                    }
                }

                strValid = strnonman;

                return strValid.ToString();
            }
            catch (SqlException ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                StringBuilder exceptionMessage = new StringBuilder();
                exceptionMessage.Append("Bulk Upload Failed with Error: ").Append(ex.Message);
                MailstrResult = exceptionMessage.ToString();
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To ConvertDecimal
        /// </summary>
        /// <param name="Inboundvalue"></param>
        /// <returns></returns>
        private static string ConvertDecimal(object Inboundvalue)
        {
            string result = "";
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
        /// <summary>
        /// This Method Is Used To IsDateTime
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="strField"></param>
        /// <returns></returns>
        private string IsDateTime(DataRow dr, string strField)
        {
            try
            {
                StrResult = string.Empty;
                DateTime convertedDate = DateTime.Parse(dr[strField].ToString());
                return string.Empty;
            }
            catch
            {
                return StrResult = "Ticket Upload unsuccessful: Please use MM/DD/YYYY hh:mm:ss format " +
                    "in the [" + strField + "] Column. <BR>";


            }

        }
        /// <summary>
        /// This Method Is Used To ValidateBulkData
        /// </summary>
        /// <param name="dtBulk"></param>
        /// <param name="strDest"></param>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private string ValidateBulkData(ValidateBulkData param)
        {
            StringBuilder strValid = new StringBuilder();
            StringBuilder strErr = new StringBuilder();
            StringBuilder stremp = new StringBuilder();
            StringBuilder ticketid = new StringBuilder();
            StringBuilder StrResult = new StringBuilder();
            DataTable dtBulkCopy = new DataTable();
            dtBulkCopy.Locale = CultureInfo.InvariantCulture;
            dtBulkCopy = param.dtBulk.Clone();
            if (dtBulkCopy.Columns.Contains("Priority"))
            {
                dtBulkCopy.Columns["Priority"].DataType = typeof(string);
            }
            if (dtBulkCopy.Columns.Contains("Status"))
            {
                dtBulkCopy.Columns["Status"].DataType = typeof(string);
            }
            if (dtBulkCopy.Columns.Contains("Ticket Type"))
            {
                dtBulkCopy.Columns["Ticket Type"].DataType = typeof(string);
            }
            if (dtBulkCopy.Columns.Contains("Ticket ID"))
            {
                dtBulkCopy.Columns["Ticket ID"].DataType = typeof(string);
            }
            if (dtBulkCopy.Columns.Contains("Application"))
            {
                dtBulkCopy.Columns["Application"].DataType = typeof(string);
            }
            foreach (DataRow dr in param.dtBulk.Rows)
            {
                dtBulkCopy.ImportRow(dr);
            }

            try
            {
                foreach (string str in param.strSource)
                {
                    strValid = new StringBuilder();
                    stremp = new StringBuilder();
                    if (str == "Ticket ID")
                    {
                        stremp.Append("[").Append(str).Append("] ='' OR ");
                    }
                    else
                    {
                        stremp = new StringBuilder();
                    }
                    strValid.Append(stremp).Append("[").Append(str).Append("] is null  ");
                    foreach (DataRow dr in dtBulkCopy.Select(strValid.ToString()))
                    {
                        if (dr.Table.Rows.Count > 0)
                        {
                            ticketid = new StringBuilder();
                            strErr.Append("[").Append(str).Append("] ,").ToString();
                            ticketid.Append("Ticket ID :").Append(dr["Ticket ID"].ToString());
                            break;
                        }
                    }
                }
                if (strErr.ToString() != "")
                {
                    var errorstrn = strErr.ToString();
                    return StrResult.Append(" Ticket Upload unsuccessful : ").Append(errorstrn.
                        Remove(strErr.Length - 1)).Append(" column contains null / invalid values for ").
                        Append(ticketid).Append(".<BR>").ToString();
                }
            }
            catch (SqlException ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                StrResult.Append("Bulk Upload Failed with Error: ").Append(ex.Message);
                return StrResult.ToString();
            }
            return StrResult.ToString();
        }
        /// <summary>
        /// This Method Is Used To ColumnValidation
        /// </summary>
        /// <param name="dtBulk"></param>
        /// <param name="dtColumnNames"></param>
        /// <param name="strSource"></param>
        /// <param name="strDes"></param>
        /// <returns></returns>
        private DataTable ColumnValidation(ref DataTable dtBulk, string[] dtColumnNames, List<string> strSource,
            List<string> strDes, string ProjectID, bool isTicketDescriptionOpted, string accountname, List<HcmSupervisorList> supervisorLists, string esaprojectid, string access)
        {
            try
            {
                string[] sArrSource;
                string[] sArrDestination;
                List<string> diff;
                IEnumerable<string> set1 = dtColumnNames;
                IEnumerable<string> set2 = strSource;
                if (set2.Count() > set1.Count())
                {
                    diff = set2.Except(set1).ToList();
                }
                else
                {
                    diff = set1.Except(set2).ToList();
                }
                if (!isTicketDescriptionOpted)
                {
                    WorkPatternColumns workpaterncol = GetWorkPatternColumns(ProjectID);
                    var isDefaultworkpatternColumnsExcel = false;
                    var isProjectsWorkPatternColumnsExcel = false;
                    var isDefaultAndProjectSame = false;

                    if (dtColumnNames.Contains("Desc_Base_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && dtColumnNames.Contains("Desc_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && dtColumnNames.Contains("Res_Base_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && dtColumnNames.Contains("Res_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                    {
                        isDefaultworkpatternColumnsExcel = true;
                    }

                    if (dtColumnNames.Contains(workpaterncol.TicketDescriptionBasePattern, StringComparer.OrdinalIgnoreCase)
                         && dtColumnNames.Contains(workpaterncol.TicketDescriptionSubPattern, StringComparer.OrdinalIgnoreCase)
                         && dtColumnNames.Contains(workpaterncol.ResolutionRemarksBasePattern, StringComparer.OrdinalIgnoreCase)
                         && dtColumnNames.Contains(workpaterncol.ResolutionRemarksSubPattern, StringComparer.OrdinalIgnoreCase))
                    {
                        isProjectsWorkPatternColumnsExcel = true;
                    }

                    if (workpaterncol.TicketDescriptionBasePattern.ToLower() == "desc_base_workpattern" && workpaterncol.TicketDescriptionSubPattern.ToLower() == "desc_sub_workpattern"
                        && workpaterncol.ResolutionRemarksBasePattern.ToLower() == "res_base_workpattern" && workpaterncol.ResolutionRemarksSubPattern.ToLower() == "res_sub_workpattern")
                    {
                        isDefaultAndProjectSame = true;
                    }

                    if (isDefaultworkpatternColumnsExcel && isProjectsWorkPatternColumnsExcel && !isDefaultAndProjectSame)
                    {
                        if (diff.Contains("Desc_Base_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Desc_Base_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Desc_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Desc_Sub_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Res_Base_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Res_Base_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Res_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Res_Sub_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("ServiceName", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("ServiceName", StringComparison.OrdinalIgnoreCase));
                        }

                        dtBulk.Columns.Remove("Desc_Base_WorkPattern");
                        dtBulk.Columns.Remove("Desc_Sub_WorkPattern");
                        dtBulk.Columns.Remove("Res_Base_WorkPattern");
                        dtBulk.Columns.Remove("Res_Sub_WorkPattern");
                        dtBulk.Columns.Remove(workpaterncol.TicketDescriptionBasePattern);
                        dtBulk.Columns.Remove(workpaterncol.TicketDescriptionSubPattern);
                        dtBulk.Columns.Remove(workpaterncol.ResolutionRemarksBasePattern);
                        dtBulk.Columns.Remove(workpaterncol.ResolutionRemarksSubPattern);
                        if (dtBulk.Columns.Contains("ServiceName"))
                        {
                            dtBulk.Columns.Remove("ServiceName");
                        }
                    }

                    if (isDefaultworkpatternColumnsExcel)
                    {
                        if (diff.Contains(workpaterncol.TicketDescriptionBasePattern, StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals(workpaterncol.TicketDescriptionBasePattern, StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains(workpaterncol.TicketDescriptionSubPattern, StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals(workpaterncol.TicketDescriptionSubPattern, StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains(workpaterncol.ResolutionRemarksBasePattern, StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals(workpaterncol.ResolutionRemarksBasePattern, StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains(workpaterncol.ResolutionRemarksSubPattern, StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals(workpaterncol.ResolutionRemarksSubPattern, StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Desc_Base_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Desc_Base_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Desc_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Desc_Sub_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Res_Base_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Res_Base_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("Res_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("Res_Sub_WorkPattern", StringComparison.OrdinalIgnoreCase));
                        }
                        if (diff.Contains("ServiceName", StringComparer.OrdinalIgnoreCase))
                        {
                            diff.RemoveAll(n => n.Equals("ServiceName", StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    if ((diff.Contains("Desc_Base_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && diff.Contains("Desc_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && diff.Contains("Res_Base_WorkPattern", StringComparer.OrdinalIgnoreCase)
                        && diff.Contains("Res_Sub_WorkPattern", StringComparer.OrdinalIgnoreCase))
                        || (!isDefaultworkpatternColumnsExcel && (diff.Contains(workpaterncol.TicketDescriptionBasePattern, StringComparer.OrdinalIgnoreCase)
                         && diff.Contains(workpaterncol.TicketDescriptionSubPattern, StringComparer.OrdinalIgnoreCase)
                         && diff.Contains(workpaterncol.ResolutionRemarksBasePattern, StringComparer.OrdinalIgnoreCase)
                         && diff.Contains(workpaterncol.ResolutionRemarksSubPattern, StringComparer.OrdinalIgnoreCase)))
                         && diff.Contains("ServiceName", StringComparer.OrdinalIgnoreCase)
                        )
                    {

                        sArrSource = strSource.ToArray();
                        sArrDestination = strDes.ToArray();
                        List<string> lstDuplicate = new List<string>();
                        for (int x = 0; x < sArrSource.Length; x++)
                        {
                            string sSourceCol = sArrSource[x];
                            string sDestinationCol = sArrDestination[x];
                            StringBuilder sb = new StringBuilder();
                            bool r1 = sSourceCol.ToLower() != workpaterncol.TicketDescriptionBasePattern.ToLower()
                                && sSourceCol.ToLower() != workpaterncol.TicketDescriptionSubPattern.ToLower();
                            if (r1 && sSourceCol.ToLower() != workpaterncol.ResolutionRemarksBasePattern.ToLower()
                                && sSourceCol.ToLower() != workpaterncol.ResolutionRemarksSubPattern.ToLower()
                                && sSourceCol.ToLower() != "servicename")
                            {
                                bool bFlag = IsFieldExists(dtBulk, sDestinationCol);
                                if (!bFlag)
                                {
                                    int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                                    dtBulk.Columns[iIndex].ColumnName = sDestinationCol;
                                }
                                else
                                {
                                    int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                                    sb.Append(sDestinationCol).Append(",").Append(iIndex);
                                    lstDuplicate.Add(sb.ToString());
                                }
                            }
                        }
                        if (lstDuplicate.Count > 0)
                        {
                            for (int j = 0; j < lstDuplicate.Count; j++)
                            {
                                string[] sDup = lstDuplicate[j].Split(',');
                                int iCol = Convert.ToInt32(sDup[1]);
                                dtBulk.Columns[iCol].ColumnName = sDup[0];
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < diff.Count; j++)
                        {
                            dtBulk.CaseSensitive = false;
                            dtBulk.Columns.Remove(diff[j]);
                        }
                        bool res = (!isDefaultworkpatternColumnsExcel && !isProjectsWorkPatternColumnsExcel && diff.Count != 0);
                        if ((!isDefaultworkpatternColumnsExcel && isProjectsWorkPatternColumnsExcel && diff.Count != 0) || res)
                        {

                            dtBulk.CaseSensitive = false;
                            if (dtBulk.Columns.Contains("Desc_Base_WorkPattern"))
                            {
                                dtBulk.Columns.Remove("Desc_Base_WorkPattern");
                            }
                            if (dtBulk.Columns.Contains("Desc_Sub_WorkPattern"))
                            {
                                dtBulk.Columns.Remove("Desc_Sub_WorkPattern");
                            }
                            if (dtBulk.Columns.Contains("Res_Base_WorkPattern"))
                            {
                                dtBulk.Columns.Remove("Res_Base_WorkPattern");
                            }
                            if (dtBulk.Columns.Contains("Res_Sub_WorkPattern"))
                            {
                                dtBulk.Columns.Remove("Res_Sub_WorkPattern");
                            }
                            if (dtBulk.Columns.Contains(workpaterncol.TicketDescriptionBasePattern))
                            {
                                dtBulk.Columns.Remove(workpaterncol.TicketDescriptionBasePattern);
                            }
                            if (dtBulk.Columns.Contains(workpaterncol.TicketDescriptionSubPattern))
                            {
                                dtBulk.Columns.Remove(workpaterncol.TicketDescriptionSubPattern);
                            }
                            if (dtBulk.Columns.Contains(workpaterncol.ResolutionRemarksBasePattern))
                            {
                                dtBulk.Columns.Remove(workpaterncol.ResolutionRemarksBasePattern);
                            }
                            if (dtBulk.Columns.Contains(workpaterncol.ResolutionRemarksSubPattern))
                            {
                                dtBulk.Columns.Remove(workpaterncol.ResolutionRemarksSubPattern);
                            }
                            if (dtBulk.Columns.Contains("ServiceName"))
                            {
                                dtBulk.Columns.Remove("ServiceName");
                            }
                        }

                        sArrSource = strSource.ToArray();
                        sArrDestination = strDes.ToArray();
                        List<string> lstDuplicate = new List<string>();
                        for (int x = 0; x < sArrSource.Length; x++)
                        {
                            string sSourceCol = sArrSource[x];
                            string sDestinationCol = sArrDestination[x];
                            StringBuilder sb = new StringBuilder();
                            bool x1 = sSourceCol.ToLower() != workpaterncol.ResolutionRemarksSubPattern.ToLower()
                                && sSourceCol.ToLower() != "desc_base_workpattern"
                                && sSourceCol.ToLower() != "desc_sub_workpattern";
                            bool x2 = sSourceCol.ToLower() != "res_base_workpattern"
                                && sSourceCol.ToLower() != "res_sub_workattern"
                                && sSourceCol.ToLower() != "servicename";
                            bool x3 = x1 && x2;
                            if (sSourceCol.ToLower() != workpaterncol.TicketDescriptionBasePattern.ToLower()
                                && sSourceCol.ToLower() != workpaterncol.TicketDescriptionSubPattern.ToLower()
                                && sSourceCol.ToLower() != workpaterncol.ResolutionRemarksBasePattern.ToLower()
                                && x3)
                            {
                                bool bFlag = IsFieldExists(dtBulk, sDestinationCol);
                                if (!bFlag)
                                {
                                    int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                                    dtBulk.Columns[iIndex].ColumnName = sDestinationCol;
                                }
                                else
                                {
                                    int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                                    sb.Append(sDestinationCol).Append(",").Append(iIndex);
                                    lstDuplicate.Add(sb.ToString());
                                }
                            }
                        }
                        if (lstDuplicate.Count > 0)
                        {
                            for (int j = 0; j < lstDuplicate.Count; j++)
                            {
                                string[] sDup = lstDuplicate[j].Split(',');
                                int iCol = Convert.ToInt32(sDup[1]);
                                dtBulk.Columns[iCol].ColumnName = sDup[0];
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < diff.Count; j++)
                    {
                        dtBulk.CaseSensitive = false;
                        dtBulk.Columns.Remove(diff[j]);
                    }
                    sArrSource = strSource.ToArray();
                    sArrDestination = strDes.ToArray();
                    List<string> lstDuplicate = new List<string>();
                    for (int x = 0; x < sArrSource.Length; x++)
                    {
                        string sSourceCol = sArrSource[x];
                        string sDestinationCol = sArrDestination[x];
                        StringBuilder sb = new StringBuilder();
                        bool bFlag = IsFieldExists(dtBulk, sDestinationCol);
                        if (!bFlag)
                        {
                            int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                            dtBulk.Columns[iIndex].ColumnName = sDestinationCol;
                        }
                        else
                        {
                            int iIndex = dtBulk.Columns[sSourceCol].Ordinal;
                            sb.Append(sDestinationCol).Append(",").Append(iIndex);
                            lstDuplicate.Add(sb.ToString());
                        }
                    }
                    if (lstDuplicate.Count > 0)
                    {
                        for (int j = 0; j < lstDuplicate.Count; j++)
                        {
                            string[] sDup = lstDuplicate[j].Split(',');
                            int iCol = Convert.ToInt32(sDup[1]);
                            dtBulk.Columns[iCol].ColumnName = sDup[0];
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                TrackBusinessLayerExceptionMessages(ex);
                sbException.Append("Template is not matching with ITSM configuration Column mapping. ")
                    .Append("Please upload valid template.");

                string MyActivityNeededKey = new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"];
                if (MyActivityNeededKey == "true")
                {
                    string Workitemcode = new AppSettings().AppsSttingsKeyValues["TicketuploadFailedCode"];
                    bool CheckSourceidstatus = new MyActivity().CheckexistingActivity(Convert.ToInt64(ProjectID), Workitemcode, access);

                    if (!CheckSourceidstatus)
                    {
                        foreach (var item in supervisorLists)
                        {
                            MyActivitySourceDto myActivitySource = new MyActivitySourceDto();
                            myActivitySource.ActivityDescription = "Ticket upload failed for the project " + esaprojectid + " - " + accountname + " due to incorrect template." +
                                " Uploaded template should match with the column mapping defined for your project.";
                            myActivitySource.WorkItemCode = Workitemcode;
                            myActivitySource.SourceRecordID = Convert.ToInt64(ProjectID);
                            myActivitySource.RaisedOnDate = DateTime.Today;
                            myActivitySource.RequestorJson = "";
                            myActivitySource.CreatedBy = "System";
                            myActivitySource.ActivityTo = item.HcmSupervisorID;
                            string st = new MyActivity().MySaveActivity(myActivitySource, access);
                        }
                    }
                }

                StrResult = sbException.ToString();
            }
            return dtBulk;
        }
        /// <summary>
        /// This Method Is Used To IsFieldExists
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public bool IsFieldExists(DataTable dt, string FieldName)
        {
            bool isFieldExists = false;
            for (int I = 0; I < dt.Columns.Count; I++)
            {
                if (dt.Columns[I].ColumnName == FieldName)
                {
                    isFieldExists = true;
                }
            }
            return isFieldExists;
        }
        /// <summary>
        /// This Method Is Used To ExcelToDataSet
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="EmployeeName"></param>
        /// <returns></returns>
        public DataTable ExcelToDataSet(string filename, string EmployeeName)
        {
            DataTable dtNull = new DataTable();
            dtNull.Locale = CultureInfo.InvariantCulture;
            try
            {
                bool flgxlsm = Path.GetExtension(filename).Contains("xlsm");
                bool flgcsv = Path.GetExtension(filename).Contains("csv");
                bool flgxls = Path.GetExtension(filename).Contains("xls");
                DataSet ds;
                string sWSName = "";
                int rowcnt = 0;
                sWSName = "BulkUpload";
                ds = new DataSet();
                ds.Locale = CultureInfo.InvariantCulture;
                var columnNames = GetMappedDateColumns();
                ds.Tables.Add(new OpenXMLOperations().ToDataTableBySheetName(filename, sWSName,
                    0, 1, columnNames).Copy());

                var filteredRows = ds.Tables[0].Rows.Cast<DataRow>().Where(
                    row => row.ItemArray.Any(field => !(field is System.DBNull)));

                rowcnt = filteredRows.Count();

                if (rowcnt == 0)
                {
                    MailstrResult = "Ticket Dump is empty";
                    return null;
                }

                if ((rowcnt < 2001 && rowcnt > 0))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && rowcnt > 0)
                    {
                        foreach (DataColumn dc in ds.Tables[0].Columns)
                        {
                            string sColumnName = dc.ColumnName;
                            ds.Tables[0].Columns[sColumnName].ColumnName = ds.Tables[0].Columns[sColumnName].
                                ColumnName.TrimStart().TrimEnd();
                        }
                        foreach (DataRow wdr in ds.Tables[0].Rows)
                        {
                            int arrayCount = wdr.ItemArray.Count();
                            for (int iLoop = 0; iLoop < arrayCount; iLoop++)
                            {
                                foreach (DataColumn col in ds.Tables[0].Columns)
                                {
                                    if (col.DataType != typeof(System.DateTime))
                                    {
                                        if (!string.IsNullOrEmpty(wdr[iLoop].ToString()))
                                        {
                                            wdr[iLoop] = Convert.ToString(wdr[iLoop]).Trim();
                                        }
                                    }
                                }
                            }
                        }
                        ds.Tables[0].Columns.Add(new DataColumn("EmployeeID", typeof(String)));
                        ds.Tables[0].Columns.Add(new DataColumn("EmployeeName", typeof(String)));
                        ds.Tables[0].Columns.Add(new DataColumn("ProjectID", typeof(Int32)));

                        bool isFirstRow = true;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dr["EmployeeID"] = StrEmployeeID;
                            dr["EmployeeName"] = EmployeeName;
                            dr["ProjectID"] = IntProjectID;

                            isFirstRow = false;
                        }
                        int tableRowsCount = ds.Tables[0].Rows.Count - 1;
                        for (int i = tableRowsCount; i >= 0; i--)
                        {
                            DataRow dr = ds.Tables[0].Rows[i];
                            if ((dr[0].ToString() == "") && (dr[1].ToString() == ""))
                            {
                                dr.Delete();
                            }
                        }
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                        else
                        {
                            StrResult = "The file has no data to upload.";
                        }
                    }
                    else
                    {
                        MailstrResult = "Ticket Dump is empty";

                    }
                }
                else
                {
                    MailstrResult = "Ticket dump with more than 2000 tickets cannot be uploaded here.";
                    return dtNull;
                }

            }

            catch (Exception ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                return dtNull;
                throw ex;
            }

            return dtNull;
        }
        /// <summary>
        /// This Method Is Used To GetMappedDateColumns
        /// </summary>
        /// <returns></returns>
        private string[] GetMappedDateColumns()
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", IntProjectID);
                prms[1] = new SqlParameter("@UserID", stremployeeID);
                DataTable dt = (new DBHelper()).GetTableFromSP("GetMappedDateColumns", prms, ConnectionString);
                return dt.AsEnumerable().Select(x => Convert.ToString((x["ProjectColumn"]))).ToArray();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
                return null;
            }

        }
        /// <summary>
        /// This Method Is Used To HasWriteAccessToFolder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// This Method Is Used To DataTableConvertion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable DataTableConvertion<TSource>(IList<TSource> data)
        {
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            dataTable.Locale = CultureInfo.InvariantCulture;
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        /// <summary>
        /// This Method Is Used To InsertTicketDumpUpload
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="FileName"></param>
        /// <param name="Ticket"></param>
        /// <param name="TicketUploadTrackID"></param>
        /// <returns></returns>
        public string InsertTicketDumpUpload(string EmployeeID, string ProjectID, string FileName,
            List<TicketDetail> Ticket, Int64 TicketUploadTrackID, string accountname, List<HcmSupervisorList> supervisorLists, string esaprojectid, string access, bool allowEncrypt)
        {
            string result = "";
            DataSet Ticketupload = new DataSet();
            Ticketupload.Locale = CultureInfo.InvariantCulture;
            excelfilename = FileName;
            string MyActivityNeededKey = new AppSettings().AppsSttingsKeyValues["IsMyActivityNeeded"];

            try
            {
                int totalCount = !allowEncrypt ? Ticket.Count : 0;
                Ticket.ForEach(x => x.TicketUploadTrackID = TicketUploadTrackID.ToString());
                Ticket.ForEach(x => x.ProjectID = ProjectID);
                List<TicketDetail> WithDescTickets = new List<TicketDetail>();
                List<TicketDetail> WithSummayTickets = new List<TicketDetail>();
                if (!allowEncrypt)
                {
                    WithDescTickets = Ticket.Where(x => x.TicketDescription != "").ToList();
                    WithSummayTickets = Ticket.Where(x => x.TicketSummary != null &&
                        WithDescTickets.Where(desc => desc.TicketID == x.TicketID).Count() == 0).ToList();
                    Ticket = Ticket.Where(x => x.TicketDescription == "").ToList().Where(x => x.TicketSummary == null).ToList();
                }
                MailstrResult = new DBHelper().InsertDatatable(DataTableConvertion<TicketDetail>(Ticket), "[dbo].[TicketUpload]", ConnectionString);

                bool isAuditAvailable = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["IsAuditAvailable"]);
                bool isThreadEnable = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["EnableThreadforTicketUpload"]);
                Int32 SleepTime = Convert.ToInt32(new AppSettings().AppsSttingsKeyValues["SleepTimings"]);
                string flag = "Manual";
                SqlParameter[] prms = new SqlParameter[6];
                prms[0] = new SqlParameter("@projectid", ProjectID);
                prms[1] = new SqlParameter("@CogID", EmployeeID);
                prms[2] = new SqlParameter("@Flag", flag);
                prms[3] = new SqlParameter("@IsAuditAvailable", isAuditAvailable);
                prms[4] = new SqlParameter("@mode", "TicketUpload");
                prms[5] = new SqlParameter("@TicketUploadTrackID", TicketUploadTrackID);
                if (isThreadEnable)
                {
                    Thread.Sleep(SleepTime);
                }
                try
                {
                    Ticketupload = (new DBHelper()).GetNoiseTableFromSP("dbo.Tk_Ticketupload", prms, ConnectionString);
                    if (WithDescTickets.Count > 0)
                    {
                        foreach (TicketDetail tk in WithDescTickets)
                        {
                            DataRow dtr1 = Ticketupload.Tables[0].NewRow();
                            dtr1[0] = tk.TicketID;
                            dtr1[1] = Constants.EncryptionFailure;
                            Ticketupload.Tables[0].Rows.Add(dtr1);
                        }
                        result = "1";
                        Ticketupload.Tables[1].Rows[0]["Result"] = result;
                    }
                    if (WithSummayTickets.Count > 0)
                    {
                        foreach (TicketDetail tk in WithSummayTickets)
                        {
                            DataRow dtr1 = Ticketupload.Tables[0].NewRow();
                            dtr1[0] = tk.TicketID;
                            dtr1[1] = Constants.EncryptionFailure;
                            Ticketupload.Tables[0].Rows.Add(dtr1);
                        }
                        result = "1";
                        Ticketupload.Tables[1].Rows[0]["Result"] = result;
                    }

                }
                catch (Exception ex)
                {
                    TrackBusinessLayerExceptionMessages(ex);
                    result = "Problem in Upload";
                    throw ex;
                }
                result = Ticketupload.Tables[1].Rows[0][0].ToString();

                ErrorLogExcel(Convert.ToInt64(ProjectID), EmployeeID, "", Ticketupload.Tables[0], TicketUploadTrackID, result, totalCount);

                if (MyActivityNeededKey == "true" && (result.Trim() == "1" || result.Trim() == "2"))
                {
                    string Workitemcode = new AppSettings().AppsSttingsKeyValues["TicketuploadFailedCode"];
                    bool CheckSourceidstatus = new MyActivity().CheckexistingActivity(Convert.ToInt64(ProjectID), Workitemcode, access);

                    if (!CheckSourceidstatus)
                    {
                        foreach (var item in supervisorLists)
                        {
                            MyActivitySourceDto myActivitySource = new MyActivitySourceDto();
                            myActivitySource.ActivityDescription = "Ticket upload failed for the project " + esaprojectid + " - " + accountname + ". Click here to view the error log";
                            myActivitySource.WorkItemCode = Workitemcode;
                            myActivitySource.SourceRecordID = Convert.ToInt64(ProjectID);
                            myActivitySource.RaisedOnDate = DateTime.Today;
                            myActivitySource.RequestorJson = "";
                            myActivitySource.CreatedBy = "System";
                            myActivitySource.ActivityTo = item.HcmSupervisorID;
                            string st = new MyActivity().MySaveActivity(myActivitySource, access);
                        }
                    }
                }

                if (result.Trim() == "1" || result.Trim() == "0" || result.Trim() == "2")
                {
                    string isAutoClassified;
                    string autoClassificationDate;
                    string isDDAutoClassified;
                    string dDAutoClassificationDate;
                    string isAutoClassifiedInfra;
                    string autoClassificationDateInfra;
                    string isDDAutoClassifiedInfra;
                    string dDAutoClassificationDateInfra;

                    DataSet aVDS = new DataSet();
                    aVDS.Locale = CultureInfo.InvariantCulture;
                    aVDS.Tables.Add(GetAutoClassifiedDetailsForDebt(ProjectID).Copy());
                    isAutoClassified = aVDS.Tables[0].Rows[0]["IsAutoClassified"].ToString();
                    autoClassificationDate = aVDS.Tables[0].Rows[0]["AutoClassificationDate"].ToString();
                    isDDAutoClassified = aVDS.Tables[0].Rows[0]["IsDDAutoClassified"].ToString();
                    dDAutoClassificationDate = aVDS.Tables[0].Rows[0]["DDClassifiedDate"].ToString();
                    isAutoClassifiedInfra = aVDS.Tables[0].Rows[0]["IsAutoClassifiedInfra"].ToString();
                    autoClassificationDateInfra = aVDS.Tables[0].Rows[0]["AutoClassificationDateInfra"].ToString();
                    isDDAutoClassifiedInfra = aVDS.Tables[0].Rows[0]["IsDDAutoClassifiedInfra"].ToString();
                    dDAutoClassificationDateInfra = aVDS.Tables[0].Rows[0]["DDClassifiedDateInfra"].ToString();

                    SqlParameter[] prm = new SqlParameter[10];
                    prm[0] = new SqlParameter("@ProjectID", ProjectID);
                    prm[1] = new SqlParameter("@UserID", EmployeeID);
                    prm[2] = new SqlParameter("@IsAutoClassified", isAutoClassified);
                    prm[3] = new SqlParameter("@IsDDAutoClassified", isDDAutoClassified);
                    if (!string.IsNullOrEmpty(autoClassificationDate))
                    {
                        prm[4] = new SqlParameter("@AutoClassificationDate", Convert.
                            ToDateTime(autoClassificationDate));
                    }
                    else
                    {
                        prm[4] = new SqlParameter("@AutoClassificationDate", null);
                    }
                    prm[5] = new SqlParameter("@DDAutoClassificationDate", dDAutoClassificationDate);
                    prm[6] = new SqlParameter("@IsAutoClassifiedInfra", isAutoClassifiedInfra);
                    prm[7] = new SqlParameter("@IsDDAutoClassifiedInfra", isDDAutoClassifiedInfra);
                    if (!string.IsNullOrEmpty(autoClassificationDateInfra))
                    {
                        prm[8] = new SqlParameter("@AutoClassificationDateInfra", Convert.
                            ToDateTime(autoClassificationDateInfra));
                    }
                    else
                    {
                        prm[8] = new SqlParameter("@AutoClassificationDateInfra", null);
                    }
                    prm[9] = new SqlParameter("@DDAutoClassificationDateInfra", dDAutoClassificationDateInfra);


                    (new DBHelper()).ExecuteNonQuery("[dbo].[InsertProjectForMLClassification]", prm, ConnectionString);
                    result = result.Trim() == "0" ? "Uploaded successfully" : "Uploaded successfully." +
                        " Please check error log for failed tickets.";
                }
                else
                {
                    result = "Problem in Upload";
                }
            }
            catch (Exception ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                result = "Problem in Upload";
                throw ex;
            }
            if (MyActivityNeededKey == "true" && result == "Uploaded successfully")
            {
                string Workitemcode = new AppSettings().AppsSttingsKeyValues["TicketuploadFailedCode"];
                bool CheckSourceidstatus = new MyActivity().CheckexistingActivity(Convert.ToInt64(ProjectID), Workitemcode, access);

                if (CheckSourceidstatus)
                {
                    UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel();
                    expiryModel.WorkItemCode = Workitemcode;
                    expiryModel.SourceRecordId = Convert.ToInt64(ProjectID);
                    expiryModel.ModifiedBy = "System";
                    string st = new MyActivity().UpdateActivityToExpiry(expiryModel, access);
                }
            }
            return result;
        }

        /// <summary>
        /// Checks If Multilingual is enabled
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <returns>bool</returns>
        public bool CheckIfMultilingualEnabled(string projectID, string employeeID)
        {

            try
            {


                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                prms[1] = new SqlParameter("@CogID", employeeID);
                DataTable dtColumns = (new DBHelper()).GetTableFromSP("AVL.CheckIfMultilingualEnabled", prms, ConnectionString);
                if (dtColumns != null)
                {
                    if (dtColumns.Rows.Count > 0)
                    {
                        return Convert.ToBoolean(dtColumns.Rows[0][0]);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets Ticket Values
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <returns>List<TicketDescriptionSummary></returns>
        public List<TicketDescriptionSummary> GetTicketValues(List<TicketSupportTypeMapping> lstColumnMapping,
            string projectID, string employeeID)
        {
            List<TicketDescriptionSummary> lstColumns = new List<TicketDescriptionSummary>();
            try
            {
                string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];

                DataTable dtTicket = new DataTable();
                dtTicket.Locale = CultureInfo.InvariantCulture;
                dtTicket.Columns.Add("TicketID");
                dtTicket.Columns.Add("SupportType");
                foreach (var val in lstColumnMapping)
                {
                    DataRow _row = dtTicket.NewRow();
                    _row["TicketID"] = val.TicketId;
                    _row["SupportType"] = val.SupportType;
                    dtTicket.Rows.Add(_row);
                    dtTicket.AcceptChanges();
                }

                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                prms[1] = new SqlParameter("@CogID", employeeID);
                prms[2] = new SqlParameter("@TicketID", dtTicket);
                prms[2].SqlDbType = SqlDbType.Structured;
                prms[2].TypeName = "AVL.TVP_TicketSupportTypeMapping";
                DataTable dtSumDesc = (new DBHelper()).
                    GetTableFromSP("AVL.GetTicketSummaryDescriptionDetails_Multilingual", prms, ConnectionString);
                if (dtSumDesc != null)
                {
                    if (dtSumDesc.Rows.Count > 0)
                    {
                        AESEncryption aesMod = new AESEncryption();
                        foreach (DataRow dr in dtSumDesc.Rows)
                        {
                            TicketDescriptionSummary tDesSum = new TicketDescriptionSummary();
                            tDesSum.TicketId = dr["TicketID"].ToString();
                            if (encryptionEnabled.ToLower() == "enabled")
                            {
                                tDesSum.TicketSummary = Convert.ToString(string.IsNullOrEmpty(dr["TicketSummary"].
                                    ToString()) ? string.Empty :
                                 aesMod.DecryptStringFromBytes(Convert.FromBase64String((string)dr["TicketSummary"]),
                                 AseKeyDetail.AesKeyConstVal));
                                tDesSum.TicketDescription = Convert.ToString(string.IsNullOrEmpty(dr["TicketDescription"].
                                    ToString()) ? string.Empty :
                                 aesMod.DecryptStringFromBytes(Convert.FromBase64String((string)dr["TicketDescription"]),
                                 AseKeyDetail.AesKeyConstVal));
                            }
                            else
                            {
                                tDesSum.TicketSummary = Convert.ToString(string.IsNullOrEmpty(dr["TicketSummary"].
                                    ToString()) ? string.Empty : (string)dr["TicketSummary"]);
                                tDesSum.TicketDescription = Convert.ToString(string.IsNullOrEmpty(dr["TicketDescription"].
                                    ToString()) ? string.Empty : (string)dr["TicketDescription"]);
                            }
                            tDesSum.SupportType = Convert.ToInt16(dr["SupportType"]);
                            tDesSum.FlexField1 = string.IsNullOrEmpty(Convert.ToString(dr["FlexField1"])) ?
                                string.Empty : Convert.ToString(dr["FlexField1"]);
                            tDesSum.FlexField2 = string.IsNullOrEmpty(Convert.ToString(dr["FlexField2"])) ?
                                string.Empty : Convert.ToString(dr["FlexField2"]);
                            tDesSum.FlexField3 = string.IsNullOrEmpty(Convert.ToString(dr["FlexField3"])) ?
                                string.Empty : Convert.ToString(dr["FlexField3"]);
                            tDesSum.FlexField4 = string.IsNullOrEmpty(Convert.ToString(dr["FlexField4"])) ?
                                string.Empty : Convert.ToString(dr["FlexField4"]);
                            lstColumns.Add(tDesSum);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstColumns;
        }

        /// <summary>
        /// Gets Encrypted Columns
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <returns>List<string></returns>
        private List<string> GetColumnsEncrypted(string projectID, string employeeID)
        {
            List<string> lstColumns = new List<string>();
            try
            {
                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                prms[1] = new SqlParameter("@CogID", employeeID);
                DataTable dtColumns = (new DBHelper()).
                    GetTableFromSP("AVL.CheckForEncryptedTicketColumns_Multilingual", prms, ConnectionString);
                if (dtColumns != null && dtColumns.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtColumns.Rows)
                    {
                        lstColumns.Add(dr[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstColumns;
        }
        /// <summary>
        /// This Method Is Used To TrackBusinessLayerExceptionMessages
        /// </summary>
        /// <param name="ex"></param>
        private void TrackBusinessLayerExceptionMessages(Exception ex)
        {
            StringBuilder ExceptionFormatter = new StringBuilder();
            ExceptionFormatter.Append("Method Name    :");
            ExceptionFormatter.Append("    ");
            ExceptionFormatter.Append(new StackTrace().GetFrame(1).GetMethod().Name);

            ExceptionFormatter.Append("Exception Message    :");
            ExceptionFormatter.Append("    ");
            ExceptionFormatter.Append(ex.Message);

            TicketUploadTrack ticketUploadtrack = new TicketUploadTrack();
            ticketUploadtrack.BLErrorMessage = ExceptionFormatter.ToString();
            UpdateTicketUploadtrackCommonFields("", ticketUploadtrack);
            SaveTicketUploadTrack(ticketUploadtrack, 0, ticketUploadTrackID);

            InsertTicketUploadTrackDetails(
               new StackTrace().GetFrame(1).GetMethod().Name,
               new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
              new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
              "ErrorMessage    :" + ex.Message,
              ticketUploadTrackID);
        }
        /// <summary>
        /// This Method Is Used To ErrorLogExcel
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="CogID"></param>
        /// <param name="source"></param>
        /// <param name="dt"></param>
        private void ErrorLogExcel(Int64 projectID, string CogID, string source, DataTable dt,
            Int64 ticketUploadTrackID, string result, Int64 totalCount = 0)
        {
            string eRRfile;
            StringBuilder sPath = new StringBuilder();
            try
            {
                Int64 pid = projectID;
                eRRfile = "";
                if (dt.Rows.Count > 0)
                {
                    StringBuilder ErrorExcelFileName = new StringBuilder();
                    StringBuilder ErrorExcelPath = new StringBuilder();
                    ErrorExcelPath.Append(new AppSettings().AppsSttingsKeyValues["ErrorLogExcel"]);
                    if (source == "External")
                    {
                        ErrorExcelFileName.Append("ERRORLOG_External_").Append(projectID).Append("_").
                            Append(DateTimeOffset.Now.DateTime.Month.ToString()).Append(DateTimeOffset.Now.DateTime.Day.ToString()).
                            Append(DateTimeOffset.Now.DateTime.Year.ToString()).Append(DateTimeOffset.Now.DateTime.Hour.ToString()).
                            Append(DateTimeOffset.Now.DateTime.Minute.ToString()).Append(DateTimeOffset.Now.DateTime.Second.ToString()).
                            Append(".xlsx");
                    }
                    else
                    {
                        ErrorExcelFileName.Append("ERRORLOG").Append(projectID).Append("_").
                            Append(DateTimeOffset.Now.DateTime.Month.ToString()).Append(DateTimeOffset.Now.DateTime.Day.ToString()).
                            Append(DateTimeOffset.Now.DateTime.Year.ToString()).Append(DateTimeOffset.Now.DateTime.Hour.ToString()).
                            Append(DateTimeOffset.Now.DateTime.Minute.ToString()).Append(DateTimeOffset.Now.DateTime.Second.ToString()).
                            Append(".xlsx");

                    }
                    sPath.Append(ErrorExcelPath).Append(ErrorExcelFileName);
                    Delete30Daysfile(ErrorExcelPath.ToString());


                    if (Directory.Exists(ErrorExcelPath.ToString()) && !File.Exists(sPath.ToString()))
                    {
                        //CCAP FIX

                    }
                    else
                    {
                        Directory.CreateDirectory(ErrorExcelPath.ToString());
                    }
                    InsertTicketUploadTrackDetails(
                               new StackTrace().GetFrame(1).GetMethod().Name,
                               new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                               new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                               "ErrorLogExcel Going To create",
                               ticketUploadTrackID);
                    new OpenXMLOperations().ToExcelSheetByDataSet(dt, null, sPath.ToString());
                    InsertTicketUploadTrackDetails(
                              new StackTrace().GetFrame(1).GetMethod().Name,
                              new StackTrace().GetFrame(1).GetFileLineNumber().ToString(),
                              new StackTrace().GetFrame(1).GetFileColumnNumber().ToString(),
                              "ErrorLogExcel created " + sPath,
                              ticketUploadTrackID);
                    eRRfile = ErrorExcelFileName.ToString();
                    SqlParameter[] prms1 = new SqlParameter[5];
                    prms1[0] = new SqlParameter("@filename", ErrorExcelFileName.ToString());
                    prms1[1] = new SqlParameter("@ProjectID", projectID);
                    prms1[2] = new SqlParameter("@userid", CogID);
                    prms1[3] = new SqlParameter("@ticketCount", totalCount);
                    prms1[4] = new SqlParameter("@failedTicketCount", dt.Rows.Count);

                    (new DBHelper()).ExecuteNonQuery("sp_UpdateExcelErrorFile", prms1, ConnectionString);

                    if (result == "1")
                    {
                        Mail1(projectID.ToString(), CogID, "", "Uploaded successfully. Please check " +
                            "error log for failed tickets.");
                    }
                }
            }
            catch (Exception ex)
            {
                TrackBusinessLayerExceptionMessages(ex);
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetUserProjectDetail
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<UserDetails> GetUserProjectDetail(string EmployeeID, int CustomerID, string MenuRole)
        {
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@CustomerID", CustomerID);
            prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
            prms[2] = new SqlParameter("@MenuRole", MenuRole);
            List<UserDetails> projectDetails = new List<UserDetails>();

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[sp_GetAVMProjectDetails]", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    projectDetails = dt.AsEnumerable().Select(x => new UserDetails
                    {
                        ProjectId = Convert.ToString((x["ProjectID"])),
                        ProjectName = Convert.ToString((x["ProjectName"])),
                        SupportTypeId = Convert.ToString((x["SupportTypeId"])),
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return projectDetails;

        }
        /// <summary>
        /// Gets Onboarding Percentage Details
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public int GetOnboardingPercentageDetails(Int64 ProjectID, string EmployeeID)
        {
            int onboardingPercentage = 0;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectID", ProjectID);
            prms[1] = new SqlParameter("@EmployeeID", EmployeeID);
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[PP].[GetAdaptersScopeDetails]", prms, ConnectionString);
                if (dt != null)
                {
                    onboardingPercentage = Convert.ToInt32(dt.Rows[0]["tot"]);
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message, ex);
            }
            return onboardingPercentage;

        }
        /// <summary>
        /// This Method Is Used To Delete30Daysfile
        /// </summary>
        /// <param name="path"></param>
        private void Delete30Daysfile(string path)
        {
            try
            {
                string directoryPath = path;
                string[] oldFiles = System.IO.Directory.GetFiles(directoryPath, "*.*");
                foreach (string currFile in oldFiles)
                {
                    System.IO.FileInfo currFileInfo = new System.IO.FileInfo(currFile);
                    if (currFileInfo.LastWriteTime < (DateTimeOffset.Now.DateTime.AddDays(-30)))
                    {
                        currFileInfo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetAutoClassifiedDetailsForDebt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable GetAutoClassifiedDetailsForDebt(string ProjectID)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectId", ProjectID);
            DataTable dt = (new DBHelper()).GetTableFromSP("debt_getautoclassifiedfieldforsharepathchange", prms, ConnectionString);
            return dt;
        }
        /// <summary>
        /// TicketDescriptionCollection
        /// </summary>
        public class TicketDescriptionCollection : List<Models.TicketDescriptionDetails>, IEnumerable<SqlDataRecord>
        {
            /// <summary>
            /// GetEnumerator
            /// </summary>
            /// <returns></returns>
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow1 = new SqlDataRecord(
                      new SqlMetaData("TicketID", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("TicketDescription", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ApplicationID", SqlDbType.VarChar, SqlMetaData.Max),
                      new SqlMetaData("ApplicationName", SqlDbType.VarChar, SqlMetaData.Max));

                foreach (Models.TicketDescriptionDetails obj in this)
                {
                    sqlRow1.SetString(0, obj.TicketId != null ? obj.TicketId : "");
                    sqlRow1.SetString(1, obj.TicketDescription != null ? obj.TicketDescription : "");
                    sqlRow1.SetString(2, obj.ApplicationId != null ? obj.ApplicationId : "");
                    sqlRow1.SetString(3, obj.ApplicationName != null ? obj.ApplicationName : "");
                    yield return sqlRow1;
                }
            }
        }

        /// <summary>
        /// This Method Is Used To get EsaProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public Int32 EffortUploadEsaProjectID(int ProjectID)
        {
            Int32 EsaProjectID = 0;
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@Projectid", ProjectID);
            List<UserDetails> ProjectDetails = new List<UserDetails>();
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[dbo].[EsaProjectIDforEffortUpload]", prms, ConnectionString);
                if (dt != null)
                {
                    EsaProjectID = Convert.ToInt32(dt.Rows[0]["EsaProjectID"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EsaProjectID;
        }
        /// <summary>
        /// This Method Is Used To ChekcITSM
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public Int32 ChekcITSM(string CustomerID, string ProjectID)
        {
            Int32 percentage = 0;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@CustomerID", CustomerID);
            prms[1] = new SqlParameter("@ProjectID", ProjectID);
            List<UserDetails> ProjectDetails = new List<UserDetails>();

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("GetCompletionPercentage", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    percentage = ((from value in dt.AsEnumerable()
                                   select value.Field<Int32>("ITSMPerc")).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return percentage;
        }
        /// <summary>
        /// This Method Is Used To CheckIsManualOrAuto
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public string CheckIsManualOrAuto(string projectID)
        {
            string manualOrAuto = "";
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", projectID);
            List<UserDetails> ProjectDetails = new List<UserDetails>();

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("CheckIsManualOrAuto", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    manualOrAuto = ((from value in dt.AsEnumerable()
                                     select value.Field<string>("IsManualOrAuto")).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return manualOrAuto;
        }
        /// <summary>
        /// This Method Is Used To CheckMandatecolumns
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public string CheckMandatecolumns(string projectID)
        {
            string validate = "";
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", projectID);
            List<UserDetails> ProjectDetails = new List<UserDetails>();

            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[TicketUploadSupportTypeMandateColumns]", prms, ConnectionString);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        validate = dt.Rows[0]["Valid"].ToString();

                    }
                    else
                    {
                        validate = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validate;
        }


        /// <summary>
        /// Function used to update the ticketUploadTrack table with the help of Running ID
        /// </summary>
        /// <param name="ticketUploadTrack"></param>
        /// <returns></returns>
        private Int64 SaveTicketUploadTrack(TicketUploadTrack ticketUploadTrack, Int16 scenario,
            Int64 TicketUploadTrackID)
        {
            Int64 Result = 0;
            List<TicketUploadTrack> ticketUploadTracks;
            try
            {
                ticketUploadTracks = new List<TicketUploadTrack>();
                ticketUploadTracks.Add(ticketUploadTrack);

                SqlParameter[] prms = new SqlParameter[3];
                prms[0] = new SqlParameter("@TicketUploadTrackType",
                    ListExtensions.ToDataTable<TicketUploadTrack>(ticketUploadTracks));
                prms[0].SqlDbType = SqlDbType.Structured;
                prms[0].TypeName = "AVL.TicketUploadTrackType";
                prms[1] = new SqlParameter("@Scenario", scenario);
                prms[2] = new SqlParameter("@TicketUploadTrackID", TicketUploadTrackID);
                DataSet ds = new DBHelper().GetDatasetFromSP("[AVL].[SaveTicketUploadTrack]", prms, ConnectionString);
                Result = Convert.ToInt64(ds.Tables[0].Rows[0][0]);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Result;
        }

        /// <summary>
        /// This Method Is Used to InsertTicketUploadTrackDetails
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="LineNumber"></param>
        /// <param name="position"></param>
        /// <param name="Message"></param>
        /// <param name="TicketUploadTrackID"></param>
        public void InsertTicketUploadTrackDetails(string MethodName, string LineNumber,
            string position, string Message, Int64 TicketUploadTrackID)
        {
            try
            {

                StringBuilder FormattedStringBuilder = new StringBuilder();
                FormattedStringBuilder.Append("Method Name  :");
                FormattedStringBuilder.Append(MethodName);
                FormattedStringBuilder.Append("    ");

                FormattedStringBuilder.Append("Line Number  :");
                FormattedStringBuilder.Append(LineNumber);
                FormattedStringBuilder.Append("    ");

                FormattedStringBuilder.Append("Position  :");
                FormattedStringBuilder.Append(position);
                FormattedStringBuilder.Append("    ");

                FormattedStringBuilder.Append("Message  :");
                FormattedStringBuilder.Append(Message);
                FormattedStringBuilder.Append("    ");

                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@Message", FormattedStringBuilder.ToString());
                prms[1] = new SqlParameter("@TicketUploadTrackID", TicketUploadTrackID);
                DataSet ds = (new DBHelper()).GetDatasetFromSP("[AVL].[InsertTicketUploadTrackDetails]", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used to Get SupportType
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        private List<AdminSupportType> GetSupportType(string ProjectID)
        {
            List<AdminSupportType> LstSupportTypeDetails = new List<AdminSupportType>();
            try
            {
                DataTable dt = (new DBHelper()).GetTableFromSP("[AVL].[GetSupportType]",
                    new DBHelper().CreateSinglePara("ProjectID", ProjectID), ConnectionString);

                if (dt != null && dt.Rows.Count > 0)
                {
                    LstSupportTypeDetails = DataTableEntensions.ToList<AdminSupportType>(dt).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstSupportTypeDetails;
        }

        /// <summary>
        /// Save Ticket Upload Error details
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="employeeID"></param>
        /// <param name="errorDetails"></param>
        /// <param name="customerID"></param>
        /// <param name="uploadedFilename"></param>
        /// <returns></returns>
        private void SaveTicketUploadErrors(string projectID, string employeeID, string errorDetails, string customerID, string uploadedFilename)
        {
            try
            {

                SqlParameter[] prms = new SqlParameter[5];

                prms[0] = new SqlParameter("@EmployeeID", employeeID);
                prms[1] = new SqlParameter("@ProjectID", projectID);
                prms[2] = new SqlParameter("@CustomerID", customerID);
                prms[3] = new SqlParameter("@Error_Details", errorDetails);
                prms[4] = new SqlParameter("@UploadedFileName", uploadedFilename);

                (new DBHelper()).ExecuteNonQuery("AVL.SaveTicketuploadErrors", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckIsTicketDescriptionOpted(string projectID)
        {
            bool result = true;
            try
            {
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                DataTable dt = (new DBHelper()).GetTableFromSP("AVL.CheckIsTicketDescriptionOpted", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var dbdetails = Convert.ToBoolean(dt.Rows[0]["IsTicketDescriptionOpted"] == DBNull.Value ? "True" : dt.Rows[0]["IsTicketDescriptionOpted"].ToString());
                    result = dbdetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<TicketDetail> UpdateWorkPatternFields(string projectID, DataTable dtBulkUpload, List<TicketDetail> ticket)
        {
            try
            {
                WorkPatternColumns workpaterncol = GetWorkPatternColumns(projectID);
                if (workpaterncol != null)
                {
                    foreach (var item in ticket)
                    {
                        if (dtBulkUpload != null && dtBulkUpload.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtBulkUpload.Rows)
                            {

                                if (!string.IsNullOrEmpty(workpaterncol.TicketDescriptionBasePattern) &&
                                    dr[workpaterncol.TicketDescriptionBasePattern] != null && dr["Ticket ID"].ToString() == item.TicketID)
                                {
                                    item.TicketDescriptionBasePattern = dr[workpaterncol.TicketDescriptionBasePattern].ToString();
                                }
                                if (!string.IsNullOrEmpty(workpaterncol.TicketDescriptionSubPattern) &&
                                    dr[workpaterncol.TicketDescriptionSubPattern] != null && dr["Ticket ID"].ToString() == item.TicketID)
                                {
                                    item.TicketDescriptionSubPattern = dr[workpaterncol.TicketDescriptionSubPattern].ToString();
                                }
                                if (!string.IsNullOrEmpty(workpaterncol.ResolutionRemarksBasePattern) &&
                                    dr[workpaterncol.ResolutionRemarksBasePattern] != null && dr["Ticket ID"].ToString() == item.TicketID)
                                {
                                    item.ResolutionRemarksBasePattern = dr[workpaterncol.ResolutionRemarksBasePattern].ToString();
                                }
                                if (!string.IsNullOrEmpty(workpaterncol.ResolutionRemarksSubPattern) &&
                                    dr[workpaterncol.ResolutionRemarksSubPattern] != null && dr["Ticket ID"].ToString() == item.TicketID)
                                {
                                    item.ResolutionRemarksSubPattern = dr[workpaterncol.ResolutionRemarksSubPattern].ToString();
                                }
                            }
                        }
                    }
                }

                return ticket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public WorkPatternColumns GetWorkPatternColumns(string projectID)
        {
            try
            {
                WorkPatternColumns workpaterncol = new WorkPatternColumns();
                SqlParameter[] prms = new SqlParameter[1];
                prms[0] = new SqlParameter("@ProjectID", projectID);
                DataTable dt = (new DBHelper()).GetTableFromSP("AVL.GetWorkPatternColumns", prms, ConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    workpaterncol.TicketDescriptionBasePattern = dt.Rows[0]["TicketDescriptionBasePattern"] == DBNull.Value ? "" : dt.Rows[0]["TicketDescriptionBasePattern"].ToString();
                    workpaterncol.TicketDescriptionSubPattern = dt.Rows[0]["TicketDescriptionSubPattern"] == DBNull.Value ? "" : dt.Rows[0]["TicketDescriptionSubPattern"].ToString();
                    workpaterncol.ResolutionRemarksBasePattern = dt.Rows[0]["ResolutionRemarksBasePattern"] == DBNull.Value ? "" : dt.Rows[0]["ResolutionRemarksBasePattern"].ToString();
                    workpaterncol.ResolutionRemarksSubPattern = dt.Rows[0]["ResolutionRemarksSubPattern"] == DBNull.Value ? "" : dt.Rows[0]["ResolutionRemarksSubPattern"].ToString();
                }
                return workpaterncol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This Method Is Used To GetUserProjectDetail
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ConfigList GetUploadConfigDetails(long ProjectId, string EmployeeId)
        {
            DataSet dsResult = new DataSet();
            dsResult.Locale = CultureInfo.InvariantCulture;
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@ProjectId", ProjectId);
            prms[1] = new SqlParameter("@EmployeeId", EmployeeId);
            ConfigList Clist = new ConfigList();
            List<TicketUploadConfig> TUploadConfig = new List<TicketUploadConfig>();
            List<EffortUploadConfig> EUploadConfig = new List<EffortUploadConfig>();

            try
            {
                dsResult = (new DBHelper()).GetDatasetFromSP("[AVL].[GetUploadConfigDetails]", prms, ConnectionString);
                if (dsResult != null)
                {

                    if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count > 0)
                    {
                        TUploadConfig = dsResult.Tables[0].AsEnumerable().Select(x => new TicketUploadConfig
                        {
                            SharePath = x["SharePath"] != DBNull.Value ? Convert.ToString(x["SharePath"])
                                 : string.Empty,
                            TicketSharePathUsers = x["TicketSharePathUsers"] != DBNull.Value ? Convert.ToString(x["TicketSharePathUsers"])
                                 : string.Empty,
                        }).ToList();
                    }
                    if (dsResult.Tables[1] != null && dsResult.Tables[1].Rows.Count > 0)
                    {
                        EUploadConfig = dsResult.Tables[1].AsEnumerable().Select(x => new EffortUploadConfig
                        {
                            SharePathName = x["SharePathName"] != DBNull.Value ? Convert.ToString(x["SharePathName"])
                                 : string.Empty,
                        }).ToList();

                    }
                }
                Clist.TicketUpload = TUploadConfig;
                Clist.EffortUpload = EUploadConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Clist;

        }

        #region Associate Lens
        /// <summary>
        /// This Method is Used to GetUserScreenAccess
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public List<AssociateLensModel> GetHealticketdetails(string fromDate, string toDate)
        {
            List<AssociateLensModel> associateLensList = new List<AssociateLensModel>();
            List<AssociateLensTrackModel> associateTrackList = new List<AssociateLensTrackModel>();
            List<AssociateLensModel> lstAssociateCertificateDetails = new List<AssociateLensModel>();
            SqlParameter[] prmsObj = new SqlParameter[2];
            prmsObj[0] = new SqlParameter("@FromDate", fromDate);
            prmsObj[1] = new SqlParameter("@ToDate", toDate);
            DataSet dsHeal = (new DBHelper()).GetDatasetFromSP("[AVL].[GetHealticketdetailsList]", prmsObj, conn);
            if (dsHeal.Tables[0] != null && dsHeal.Tables[0].Rows.Count > 0)
            {
                associateLensList = DataTableToList.ToDTList<AssociateLensModel>(dsHeal.Tables[0]).ToList();
                associateTrackList = DataTableToList.ToDTList<AssociateLensTrackModel>(dsHeal.Tables[1]).ToList();
                lstAssociateCertificateDetails = associateLensList.Select(x => new AssociateLensModel
                {
                    AccountId = x.AccountId,
                    Award = x.Award,
                    AccountName = x.AccountName,
                    Category = x.Category,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    EsaProjectId = x.EsaProjectId,
                    CertificationMonth = x.CertificationMonth,
                    CertificationYear = x.CertificationYear,
                    Designation = x.Designation,
                    NoOfATicketsClosed = x.NoOfATicketsClosed,
                    NoOfHTicketsClosed = x.NoOfHTicketsClosed,
                    ProjectID = x.ProjectID,
                    ProjectName = x.ProjectName,
                    IncReductionMonth = x.IncReductionMonth,
                    EffortReductionMonth = x.EffortReductionMonth,
                    SolutionIdentified = x.SolutionIdentified,
                    NoOfCodeAssetContributed = x.NoOfCodeAssetContributed,
                    NoOfKEDBCreatedApproved = x.NoOfKEDBCreatedApproved,
                    ReferenceId = associateTrackList.Where(y => y.ProjectID == x.ProjectID && y.EmployeeId == x.EmployeeId).Select(r => r.ReferenceId).ToList()
                }).ToList();
            }
            return lstAssociateCertificateDetails;
        }


        /// <summary>
        /// This Method is Used to GetUserScreenAccess
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<AssociateLensModel> GetAutomationticketdetails(string fromDate, string toDate)
        {
            List<AssociateLensModel> associateLensList = new List<AssociateLensModel>();
            List<AssociateLensTrackModel> associateTrackList = new List<AssociateLensTrackModel>();
            List<AssociateLensModel> lstAssociateCertificateDetails = new List<AssociateLensModel>();
            SqlParameter[] prmsObj = new SqlParameter[2];
            prmsObj[0] = new SqlParameter("@FromDate", fromDate);
            prmsObj[1] = new SqlParameter("@ToDate", toDate);
            DataSet dsAuto = (new DBHelper()).GetDatasetFromSP("[AVL].[GetAutomationAssociateList]", prmsObj, conn);

            if (dsAuto.Tables[0] != null && dsAuto.Tables[0].Rows.Count > 0)
            {
                associateLensList = DataTableToList.ToDTList<AssociateLensModel>(dsAuto.Tables[0]).ToList();
                associateTrackList = DataTableToList.ToDTList<AssociateLensTrackModel>(dsAuto.Tables[1]).ToList();
                lstAssociateCertificateDetails = associateLensList.Select(x => new AssociateLensModel
                {
                    AccountId = x.AccountId,
                    Award = x.Award,
                    AccountName = x.AccountName,
                    Category = x.Category,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.EmployeeName,
                    EsaProjectId = x.EsaProjectId,
                    CertificationMonth = x.CertificationMonth,
                    CertificationYear = x.CertificationYear,
                    Designation = x.Designation,
                    NoOfATicketsClosed = x.NoOfATicketsClosed,
                    NoOfHTicketsClosed = x.NoOfHTicketsClosed,
                    ProjectID = x.ProjectID,
                    ProjectName = x.ProjectName,
                    IncReductionMonth = x.IncReductionMonth,
                    EffortReductionMonth = x.EffortReductionMonth,
                    SolutionIdentified = x.SolutionIdentified,
                    NoOfCodeAssetContributed = x.NoOfCodeAssetContributed,
                    NoOfKEDBCreatedApproved = x.NoOfKEDBCreatedApproved,
                    ReferenceId = associateTrackList.Where(y => y.ProjectID == x.ProjectID && y.EmployeeId == x.EmployeeId).Select(r => r.ReferenceId).ToList()
                }).ToList();
            }
            return lstAssociateCertificateDetails;
        }
        #endregion

        /// <summary>
        /// Download the Debt unclassified ticket
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="ProjectName"></param>
        /// <param name="ClosedDateFrom"></param>
        /// <param name="ClosedDateTo"></param>
        /// <returns></returns>
        public string DownloadDebtTemplate(string EsaProjectID, string projectId, string ProjectName, string ClosedDateFrom, string ClosedDateTo,
            string AppTowerID, string ispureApp, string userID)
        {
            try
            {
                string orgpath = string.Empty;
                string encryptionEnabled = new AppSettings().AppsSttingsKeyValues["EncryptionEnabled"];
                ispureApp = ispureApp == "true" ? "1" : "2";
                SqlParameter[] prms = new SqlParameter[5];
                prms[0] = new SqlParameter("@ProjectID", Convert.ToInt32(projectId, CultureInfo.CurrentCulture));
                prms[1] = new SqlParameter("@ClosedDateFrom", Convert.ToDateTime(ClosedDateFrom));
                prms[2] = new SqlParameter("@ClosedDateTo", Convert.ToDateTime(ClosedDateTo));
                prms[3] = new SqlParameter("@SupportType", Convert.ToInt32(ispureApp, CultureInfo.CurrentCulture));
                prms[4] = new SqlParameter("@ApplicationTowerID", AppTowerID);
                DataSet dsExcel = new DBHelper().GetDatasetFromSP
               (StoredProcedure.GetDebtUnClassifiedTickets, prms, ConnectionString);

                if (dsExcel.Tables[0].Rows.Count > 0)
                {
                    AESEncryption aesMod = new AESEncryption();
                    OpenXMLOperations xmlOp = new OpenXMLOperations();
                    string newpth = string.Empty;


                    string sourcePath = new AppSettings().AppsSttingsKeyValues["ExcelDebtTemplatePath"];
                    string strExtension = Path.GetExtension(sourcePath);
                    string folderName = new AppSettings().AppsSttingsKeyValues["DebtExcelsaveTemplatePath"];

                    string orginalFile = Path.GetDirectoryName(sourcePath) + "\\";
                    string fileName = Path.GetFileName(sourcePath);
                    DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
                    FileInfo fleInfo = new FileInfo(sourcePath);
                    var ext = strExtension;
                    string strTimeStamp = DateTimeOffset.Now.DateTime.ToString("yyyy_MM_dd_HH_mm_ss");
                    orgpath = folderName + string.Concat(userID + "-" + EsaProjectID + "-" + ProjectName + "-" + strTimeStamp + "-" + fleInfo.Name.Split('.')[0], ext);
                    DirectoryInfo directoryInfoorg = new DirectoryInfo(orginalFile);
                    if (directoryInfo.Exists)
                    {
                        newpth = orgpath;
                        //Veracode Fix
                        string dirctoryName = System.IO.Path.GetDirectoryName(newpth);
                        string fName = System.IO.Path.GetFileNameWithoutExtension(newpth);
                        string validatePath = System.IO.Path.Combine(dirctoryName, fName, ".xlsm");
                        validatePath = RemoveLastIndexCharacter(validatePath);
                        validatePath = Logger.RegexPath(validatePath);
                        fleInfo.CopyTo(validatePath, true);
                    }

                    if (encryptionEnabled == "Enabled")
                    {
                        foreach (DataRow dr in dsExcel.Tables[0].Rows)
                        {
                            if (dr["Ticket Description"] != null && dr["Ticket Description"] != "")
                            {
                                dr.SetField("Ticket Description", aesMod.DecryptStringFromBytes(Convert.FromBase64String((string)dr["Ticket Description"]),
                                             AseKeyDetail.AesKeyConstVal));
                            }
                        }
                    }

                    DataTable dtExcelTowerDownload = dsExcel.Tables[0];
                    DataTable HdnCauseCode = dsExcel.Tables[1];
                    DataTable HdnResolutionCode = dsExcel.Tables[2];
                    DataTable HdnDebtCategory = dsExcel.Tables[3];
                    DataTable HdnAvoidable = dsExcel.Tables[4];
                    DataTable HdnResidual = dsExcel.Tables[5];

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                        (dtExcelTowerDownload, null, newpth, "Debt Unclassified Tickets", 1, true);

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                      (HdnDebtCategory, null, newpth,
                    "HdnDebtCategory", 0);

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                       (HdnCauseCode, null, newpth,
                     "HdnCauseCode", 0);

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                      (HdnResolutionCode, null, newpth,
                    "HdnResolutionCode", 0);

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                      (HdnAvoidable, null, newpth,
                    "HdnAvoidable", 0);

                    xmlOp.ToExcelSheetDebtUnclassifiedByDataTable
                      (HdnResidual, null, newpth,
                    "HdnResidual", 0);
                }

                return orgpath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Upload for the debt unclassified ticket
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="cognizantId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>

        public string ProcessFileUploadforDebt(string fileName, string filePath,
           string projectId, string ispureApp, string UserId)
        {
            string Result = string.Empty;
            string strPath = filePath;
            DataTable dtDebt = new DataTable();
            dtDebt.Locale = CultureInfo.InvariantCulture;
            dtDebt.Locale = CultureInfo.CurrentCulture;
            ispureApp = ispureApp == "true" ? "1" : "2";

            DataTable dtDebtTicket = new OpenXMLOperations().ToDataTableBySheetName(strPath,
                "Debt Unclassified Tickets", 1, 1);

            if (dtDebtTicket != null && dtDebtTicket.Rows.Count > 0)
            {
                dtDebtTicket.Rows[0].Delete();
                dtDebtTicket.AcceptChanges();
                dtDebtTicket.Columns.RemoveAt(2);
                dtDebtTicket.Columns.RemoveAt(1);

                List<string> excelColumnNames = dtDebtTicket.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToList();
                SqlParameter[] prmsValidate = new SqlParameter[1];
                prmsValidate[0] = new SqlParameter("@ProjectID", projectId); //projectId
                DataTable dtGetFlexField = (new DBHelper()).
                    GetTableFromSP(StoredProcedure.GetDebtUnClassifiedTicketTemplate, prmsValidate, ConnectionString);
                var filteredRows = dtGetFlexField.AsEnumerable().Select(x => x.ItemArray).ToList()[0].Where(x => x.ToString() != "").ToList();
                //Check if new flex been configured
                var IsMismatch = filteredRows.Except(excelColumnNames).ToList();
                var IsMismatchExcel = excelColumnNames.Except(filteredRows).ToList();
                //Check if Empty occurs in excel
                dtDebt = dtDebtTicket.AsEnumerable().Where(x => x.Field<string>("Ticket ID") != null).ToList().CopyToDataTable();
                var IsEmpty = 0;
                bool IsValidate = false;
                if (IsMismatch.Count == 0 && IsMismatchExcel.Count == 0)//Get the latest Flex Field
                {
                    for (int i = 0; i < excelColumnNames.Count; i++)
                    {
                        IsEmpty = dtDebt.AsEnumerable().Where(x => x.Field<string>(excelColumnNames[i]) == null).ToList().Count();
                        if (IsEmpty > 0)//Check if empty exists
                        {
                            IsValidate = false;
                            break;
                        }
                        else
                        {
                            IsValidate = true;
                        }
                    }
                    dtDebtTicket = dtDebtTicket.AsEnumerable().Where(x => x.Field<string>("Ticket ID") != null).CopyToDataTable();
                    if (dtDebtTicket.Rows.Count > 0 && IsValidate == true)
                    {
                        dtDebtTicket = FormatDatatableDebt(dtDebtTicket, dtGetFlexField);

                        SqlParameter[] prms = new SqlParameter[4];
                        prms[0] = new SqlParameter("@ProjectId", Convert.ToInt32(projectId, CultureInfo.CurrentCulture));
                        prms[1] = new SqlParameter("@SupportType", Convert.ToInt32(ispureApp, CultureInfo.CurrentCulture));
                        prms[2] = new SqlParameter("@UserId", UserId);
                        prms[3] = new SqlParameter("@TVP_DebtUnclassifiedTicketDetails", dtDebtTicket);
                        prms[3].SqlDbType = SqlDbType.Structured;
                        prms[3].TypeName = "AVL.DebtUnclassifiedTicketDetails";
                        DataTable dtSaveUserName = (new DBHelper()).
                            GetTableFromSP(StoredProcedure.UploadDebtUnClassifiedTickets, prms, ConnectionString);
                        Result = "Uploaded Successfully.";
                    }
                    else
                    {
                        Result = "Invalid template.Please validate the template and upload it again";
                    }
                }
                else
                {
                    Result = "Please Download the latest template and try again.";
                }

            }
            else
            {
                Result = "Invalid template.Please validate the template and upload it again";
            }

            return Result;

        }


        /// <summary>
        /// Format the debe unclassified Tickets
        /// </summary>
        /// <param name="dtDebtTicket"></param>
        /// <returns></returns>
        public DataTable FormatDatatableDebt(DataTable dtDebtTicket, DataTable dtGetFlexField)
        {

            if (!dtDebtTicket.Columns.Contains(dtGetFlexField.Rows[0]["FlexField1"].ToString()))
            {
                dtDebtTicket.Columns.Add(dtGetFlexField.Rows[0]["FlexField1"].ToString()).SetOrdinal(6);
            }
            if (!dtDebtTicket.Columns.Contains(dtGetFlexField.Rows[0]["FlexField2"].ToString()))
            {
                dtDebtTicket.Columns.Add(dtGetFlexField.Rows[0]["FlexField2"].ToString()).SetOrdinal(7);
            }
            if (!dtDebtTicket.Columns.Contains(dtGetFlexField.Rows[0]["FlexField3"].ToString()))
            {
                dtDebtTicket.Columns.Add(dtGetFlexField.Rows[0]["FlexField3"].ToString()).SetOrdinal(8);
            }
            if (!dtDebtTicket.Columns.Contains(dtGetFlexField.Rows[0]["FlexField4"].ToString()))
            {
                dtDebtTicket.Columns.Add(dtGetFlexField.Rows[0]["FlexField4"].ToString()).SetOrdinal(9);
            }

            dtDebtTicket.Columns["Ticket ID"].SetOrdinal(0);
            dtDebtTicket.Columns["Cause Code"].SetOrdinal(1);
            dtDebtTicket.Columns["Resolution Code"].SetOrdinal(2);
            dtDebtTicket.Columns["Debt Classification"].SetOrdinal(3);
            dtDebtTicket.Columns["Avoidable Flag"].SetOrdinal(4);
            dtDebtTicket.Columns["Residual Debt"].SetOrdinal(5);



            dtDebtTicket.Columns[0].ColumnName = "TicketId";
            dtDebtTicket.Columns[1].ColumnName = "CauseCode";
            dtDebtTicket.Columns[2].ColumnName = "ResolutionCode";
            dtDebtTicket.Columns[3].ColumnName = "DebtClassificationName";
            dtDebtTicket.Columns[4].ColumnName = "AvoidableFlag";
            dtDebtTicket.Columns[5].ColumnName = "ResiDualDebt";
            dtDebtTicket.Columns[6].ColumnName = "FlexField1";
            dtDebtTicket.Columns[7].ColumnName = "FlexField2";
            dtDebtTicket.Columns[8].ColumnName = "FlexField3";
            dtDebtTicket.Columns[9].ColumnName = "FlexField4";

            return dtDebtTicket;
        }
        public List<HcmSupervisorList> GetSupervisorAndEmployeeList(string projectId)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@projectid", projectId);
            var Ticketupload = (new DBHelper()).GetDatasetFromSP("[AVL].[GetSupervisorAndEmployeeList]", prms, ConnectionString);
            List<HcmSupervisorList> supervisorLists = new List<HcmSupervisorList>();
            if (Ticketupload != null && Ticketupload.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Ticketupload.Tables[0].Rows.Count; i++)
                {
                    HcmSupervisorList hcmList = new HcmSupervisorList();
                    hcmList.ProjectID = Ticketupload.Tables[0].Rows[i]["ProjectID"] != null ? Convert.ToInt32(Ticketupload.Tables[0].Rows[i]["ProjectID"]) : 0;
                    hcmList.CustomerID = Ticketupload.Tables[0].Rows[i]["CustomerID"] != null ? Convert.ToInt32(Ticketupload.Tables[0].Rows[i]["CustomerID"]) : 0;
                    hcmList.HcmSupervisorID = Ticketupload.Tables[0].Rows[i]["HcmSupervisorID"].ToString();
                    supervisorLists.Add(hcmList);
                }
            }
            return supervisorLists;
        }
    }
}


