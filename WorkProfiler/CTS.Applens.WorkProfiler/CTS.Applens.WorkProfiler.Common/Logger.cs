using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Common
{ /// <summary>
  /// logger deails
  /// </summary>
    public class Logger
    {
        private ILog Log { get; set; }
        /// <summary>
        /// construcor
        /// </summary>
         Logger()
        {
            try
            {
                Log = LogManager.GetLogger(typeof(Logger));
            }
            catch
            {
                //CCAP FIX
            }
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(object msg)
        {
            try
            {
                new Logger().Log.Error(msg);
            }
            catch
            {
                //CCAP FIX
            }
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Error(object msg, Exception ex)
        {
            try
            {
                new Logger().Log.Error(msg, ex);
            }
            catch
            {
                //CCAP FIX
            }
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            try
            {
                new Logger().Log.Error(ex.Message, ex);
            }
            catch
            {
                //CCAP FIX
            }
        }
        /// <summary>
        /// Infor
        /// </summary>
        /// <param name="msg"></param>
        public  void Info(object msg)
        {
            try
            {
                new Logger().Log.Info(msg);
            }
            catch
            {
                //CCAP FIX
            }
        }
        public static string RegexPath(string fullPath)
        {
            if (fullPath != null)
            {
                fullPath = fullPath.Replace(">", "");
                fullPath = fullPath.Replace("<", "");
                fullPath = fullPath.Replace("..", "");
                return fullPath;
            }
            else
            {
                return "";
            }
        }
    }
}
