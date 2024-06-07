using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.Framework;
using System;
using Microsoft.Data.SqlClient;
using System.Security.Principal;

namespace CTS.Applens.WorkProfiler.DAL
{
    public class ExceptionLogging : DBContext
    {
        public void LogException(Exception exception, string userId)
        {
                new DBHelper().ExecuteNonQuery(
                                              StoredProcedure.ErrorLog,
                                              new SqlParameter[] { new SqlParameter("@ErrSource", Environment.MachineName),
                                              new SqlParameter("@Message",string.Concat(exception.Message," ", exception.Message," ", exception?.InnerException)),
                                              new SqlParameter("@UserID",  userId) },
                                              ConnectionString);
        }
    }
}
