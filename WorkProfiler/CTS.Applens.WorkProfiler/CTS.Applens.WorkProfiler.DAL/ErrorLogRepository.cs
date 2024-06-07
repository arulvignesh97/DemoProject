using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Models;
using System.Configuration;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using CTS.Applens.Framework;
using System.Net.Http;
using System.Runtime.InteropServices;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Entities;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// Repository for Error Log
    /// </summary>

    public class ErrorLogRepository : DBContext
    {

        /// <summary>
        /// This Method Is Used To GetErrorLog
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<Models.ErrorLog> GetErrorLog(string EmployeeID, int CustomerID, int ProjectId)
        {
            SqlParameter[] prms = new SqlParameter[3];
            prms[0] = new SqlParameter("@ProjectID", ProjectId);
            prms[1] = new SqlParameter("@CustomerID", CustomerID);
            prms[2] = new SqlParameter("@EmployeeID", EmployeeID);
            List<Models.ErrorLog> ErrorLog = new List<Models.ErrorLog>();

            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("GetErrorLog", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        ErrorLog = dt.Tables[0].AsEnumerable().Select(x => new Models.ErrorLog
                        {
                            ProjectId = Convert.ToString((x["ProjectID"])),
                            FileName = Convert.ToString((x["FileName"])),
                            UploadedDate = Convert.ToDateTime((x["UploadDate"])),
                            UploadSource = Convert.ToString((x["UploadMode"])),
                            Status = Convert.ToString((x["Status"])),
                            LoadedTickets = Convert.ToInt64((x["UpdatedticketCount"])),
                            RejectedTickets = Convert.ToInt32((x["FailedticketCount"])),
                            ReUploadedTickets = Convert.ToInt32((x["ReUploadedTicketCount"])),
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
               // Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                 //   "Getting the data in sp GetErrorLog ", ProjectId);
                throw ex;
            }
            return ErrorLog;
        }
        /// <summary>
        /// This Method Is Used To GetEffortUploadErrorLog
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<Models.ErrorLog> GetEffortUploadErrorLog(int ProjectId)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectId);


            List<Models.ErrorLog> ErrorLog = new List<Models.ErrorLog>();

            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("GetEffortUploadErrorLog", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        ErrorLog = dt.Tables[0].AsEnumerable().Select(x => new Models.ErrorLog
                        {
                            ProjectId = Convert.ToString((x["ProjectID"])),
                            FileName = Convert.ToString((x["ErrorFileName"])),
                            UploadedDate = Convert.ToDateTime((x["UploadedEndDate"])),

                            Status = Convert.ToString((x["Status"])),
                            LoadedTickets = Convert.ToInt64((x["TotalRecords"])),
                            RejectedTickets = Convert.ToInt32((x["FailedCount"])),

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                //Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                   // "Getting the data in sp GetErrorLog ", ProjectId);
                throw ex;
            }
            return ErrorLog;
        }
        /// <summary>
        /// This Method Is Used To GetFileName
        /// </summary>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public string GetFileName(string PathName)
        {
            string fileName = string.Empty;
            try
            {
                fileName = Path.GetFileName(PathName);
            }
            catch (Exception ex)
            {
               // Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                //    "Getting File Name", 0);
                throw ex;
            }
            return fileName;
        }
        /// <summary>
        /// This Method Is Used To CheckforFile
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public string CheckforFile(string Path)
        {
            if (File.Exists(Path.Replace("..", "")))
            {
                return Path.Replace("..", "");

            }
            else
            {
                return "File does not exists";
            }
        }
        /// <summary>
        /// This Method Is Used To GetCofigDetails
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<Config> GetCofigDetails(int ProjectId)
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@ProjectID", ProjectId);
            List<Config> Config = new List<Config>();

            try
            {
                DataSet dt = new DataSet();
                dt.Locale = CultureInfo.InvariantCulture;
                dt.Tables.Add((new DBHelper()).GetTableFromSP("GetErrorLogConfigDetails", prms, ConnectionString).Copy());
                if (dt.Tables[0] != null)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        Config = dt.Tables[0].AsEnumerable().Select(x => new Config
                        {
                            ModuleName = Convert.ToString((x["Module"])),
                            IsActive = Convert.ToString((x["IsActive"]))
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
               // Utility.ErrorLOG("Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace,
                  //  "Getting the data in sp GetErrorLog ", ProjectId);
                throw ex;
            }

            return Config;
        }

    }
}
