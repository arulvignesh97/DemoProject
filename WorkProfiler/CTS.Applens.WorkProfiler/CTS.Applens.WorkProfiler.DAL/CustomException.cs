using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// Error Log Class is used to capture all the Exceptions in the code!
    /// </summary>
    [Serializable]
    public class CustomException : Exception
    {
        public static readonly string ConnectionString = new AppSettings().AppsSttingsKeyValues["ConnectionStrings.ApplensConnection"];
        bool ErrorLogEnabled = Convert.ToBoolean(ApplicationConstants.ErrorLogEnabled, CultureInfo.CurrentCulture);
        /// <summary>
        /// This Constuctor is used to call CustomException Function!
        /// </summary>
        /// 
        public CustomException()
        : base()
        { }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="message"></param>
        public CustomException(string message)
        : base(message)
        { }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public CustomException(string format, params object[] args)
        : base(string.Format(format, args,CultureInfo.CurrentCulture))
        { }

        /// <summary>
        /// Constroctor catches the throw exception in the code
        /// </summary>
        public CustomException(string message, Exception innerException)
        : base(message, innerException)
        {
            try
            {
                CreateErrorLog(innerException, string.Empty, string.Empty, string.Empty);
                if (ErrorLogEnabled == true)
                {
                    Logger.Error(message, innerException);
                }
            }
            catch
            {
                //CCAP FIX
            }

        }
        /// <summary>
        ///  Constroctor catches the throw exception in the code
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="pagename"></param>
        public CustomException(string message, Exception innerException, string pagename)
       : base(message, innerException)
        {

            try
            {
                CreateErrorLog(innerException, string.Empty, string.Empty, pagename);
            }
            catch
            {
                //CCAP FIX
            }
        }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="format"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public CustomException(string format, Exception innerException, params object[] args)
        : base(string.Format(format, args,CultureInfo.CurrentCulture), innerException)
        { }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CustomException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        { }

        /// <summary>
        /// This Method is used to CreateErrorLog!
        /// </summary>
        /// <param name="Ex"></param>
        /// <param name="CustomerID"></param>
        /// <param name="UserID"></param>
        /// <param name="pagename"></param>
        public void CreateErrorLog(Exception Ex, string CustomerID, string UserID, string pagename)
        {

            StackTrace trace = new StackTrace(Ex, true);
            StackFrame stackFrame = trace.GetFrame(trace.FrameCount - 1);
            string errorDescription;
            if (pagename != "DBHelperCustom")
            {
                errorDescription = Ex.Message;
            }
            else
            {
                errorDescription = Ex.Message;
            }

            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@ErrSource", pagename);
                prms[1] = new SqlParameter("@Message", errorDescription);
                prms[2] = new SqlParameter("@UserID", UserID);
                prms[3] = new SqlParameter("@CustomerID", CustomerID);

                DataSet dsErrorLogs = (new DBHelper().GetDatasetFromSP)("[dbo].[AVL_InsertError]", prms, ConnectionString);

                if (ErrorLogEnabled == true) 
                {
                    Logger.Error(errorDescription); 
                }
            }
            catch
            {
                //CCAP FIX
            }
        }


        /// <summary>
        /// To Write ErrorLog details
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        public static void ErrorLog(string errorMessage)
        {
            string path = ApplicationConstants.ErrorLogPath;
            string fileName = DateTimeOffset.Now.DateTime.Day.ToString(CultureInfo.CurrentCulture) + DateTimeOffset.Now.DateTime.Month.ToString(CultureInfo.CurrentCulture) + DateTimeOffset.Now.DateTime.Year.ToString(CultureInfo.CurrentCulture) + "_Logs.txt";
            System.IO.StreamWriter file = null;

            try
            {
                file = new System.IO.StreamWriter(path + fileName, true);
                file.WriteLine(DateTimeOffset.Now.DateTime.ToString(CultureInfo.CurrentCulture) + ": " + errorMessage);

            }
            catch (Exception)
            {
                //CCAP FIX
            }
            finally
            {
                if(file != null)
                {
                    file.Close();
                }
                
            }
        }

    }
}