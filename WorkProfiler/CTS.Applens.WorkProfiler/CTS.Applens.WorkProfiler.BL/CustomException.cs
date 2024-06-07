using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Serialization;
using CustomExceptions = CTS.Applens.WorkProfiler.DAL.CustomException;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// Error Log Class is used to capture all the Exceptions in the code!
    /// </summary>
    [Serializable]
    public class CustomException : Exception
    {
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
        : base(string.Format(format, args))
        { }

        /// <summary>
        /// Constroctor catches the throw exception in the code
        /// </summary>
        public CustomException(string message, Exception innerException)
        : base(message, innerException)
        {
            try
            {
                new CustomExceptions(message, innerException);
            }
            catch { }

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
                new CustomExceptions(message, innerException, pagename);
            }
            catch { }
        }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="format"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        public CustomException(string format, Exception innerException, params object[] args)
        : base(string.Format(format, args), innerException)
        { }

        /// <summary>
        /// CustomException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CustomException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        { }
    }
}