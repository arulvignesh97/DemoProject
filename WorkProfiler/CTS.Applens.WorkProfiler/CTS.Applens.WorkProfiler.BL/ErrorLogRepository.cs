using CTS.Applens.WorkProfiler.DAL;
using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using ErrorLog = CTS.Applens.WorkProfiler.DAL.ErrorLogRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// Repository for Error Log
    /// </summary>

    public class ErrorLogRepository
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
            try
            {
                return new ErrorLog().GetErrorLog(EmployeeID, CustomerID, ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetEffortUploadErrorLog
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<Models.ErrorLog> GetEffortUploadErrorLog( int ProjectId)
        {
            try
            {
                return new ErrorLog().GetEffortUploadErrorLog(ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                   // "Getting File Name", 0);
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
            try
            {
                return new ErrorLog().GetCofigDetails(ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
