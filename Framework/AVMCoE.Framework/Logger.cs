using System;
using log4net;

namespace AVMCoE.Framework
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        private static ILog Log { get; set; }
        /// <summary>
        /// construcor
        /// </summary>
        static Logger()
        {
            Log = LogManager.GetLogger(typeof(Logger));
            //Log = LogManager.GetLogger("LogFileAppender");
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(object msg)
        {
            Log.Error(msg);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            Log.Error(ex.Message,ex.InnerException);
        }
        /// <summary>
        /// Infor
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(object msg)
        {
            Log.Info(msg);
        }
    }
}
