using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using System;
using Microsoft.Data.SqlClient;

namespace CTS.Applens.WorkProfiler.DAL
{
    public static class TranslateRepository
    {
        private static readonly string connectionString = new AppSettings().AppsSttingsKeyValues["ConnectionStrings.ApplensConnection"];

        /// <summary>
        ///Logs API Key Error
        /// </summary>
        /// <param name="ProjectID">This parameter holds ProjectID value </param>
        /// <param name="Key">This parameter holds Key value</param>
        /// <param name="Error">This parameter holds Error value</param>
        /// <returns></returns>
        public static void LogTranslateAPIKeyError(long projectID, string key, string error)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = new SqlParameter("@ProjectID", projectID);
                sqlParams[1] = new SqlParameter("@Key", key);
                sqlParams[2] = new SqlParameter("@Error", error);
                sqlParams[3] = new SqlParameter("@User", "SYSTEM");
                new DBHelper()
                    .ExecuteNonQuery("AVL.LogTranslateAPIKeyError", sqlParams,connectionString);
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        /// <summary>
        ///Logs API  Error
        /// </summary>
        /// <param name="ticketProp"></param>
        /// <param name="TranslateText"></param>
        /// <param name="errorScope"></param>
        /// <param name="errorMessage"></param>
        /// <param name="translateText"></param>
        /// <returns></returns>
        public static void LogTranslateAPIError(CommonTicketProperty ticketProp, string translateText,
            string errorScope, string errorMessage)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[6];
                sqlParams[0] = new SqlParameter("@TimeTickerID", ticketProp.TimeTickerId);
                sqlParams[1] = new SqlParameter("@TranslateText", translateText == null ? string.Empty
                    : translateText);
                sqlParams[2] = new SqlParameter("@ErrorScope", errorScope);
                sqlParams[3] = new SqlParameter("@ErrorMessage", errorMessage);
                sqlParams[4] = new SqlParameter("@User", "SYSTEM");
                sqlParams[5] = new SqlParameter("@SupportType", ticketProp.SupportType);
                new DBHelper()
                    .ExecuteNonQuery("AVL.LogTranslateAPIError", sqlParams,connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
