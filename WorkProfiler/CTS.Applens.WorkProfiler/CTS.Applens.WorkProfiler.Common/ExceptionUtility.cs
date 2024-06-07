using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Common
{
    public class ExceptionUtility
    {
        /// <summary>
        /// LogExceptionMessage
        /// </summary>
        /// <param name="ex"></param>
        public void LogExceptionMessage(Exception ex)
        {
            string ExceptionType = ex.GetType().ToString();
            switch(ExceptionType)
            {
                case "Exception":
                case "SystemException":
                case "IndexOutOfRangeException":
                case "NullReferenceException":
                case "AccessViolationException":
                case "InvalidOperationException":
                case "ArgumentException":
                case "ArgumentNullException":
                case "ArgumentOutOfRangeException":
                case "ExternalException":
                case "COMException":
                case "SEHException":
                    break;
                default:
                    //CCAP FIX
                    break;
            }
        }
       
    }
}