using CTS.Applens.WorkProfiler.Entities;
using System;
using System.Security.Principal;

namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// Class for Logging Exceptions into DB
    /// </summary>
    public class ExceptionLogging : IExcpetionLogging
    {
        /// <summary>
        /// Method to log excetions
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="identity"></param>
        public void LogException(Exception exception, string userId)
        {
            new DAL.ExceptionLogging().LogException(exception, userId);
        }
    }
}
