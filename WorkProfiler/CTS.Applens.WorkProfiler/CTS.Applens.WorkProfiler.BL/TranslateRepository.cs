using CTS.Applens.WorkProfiler.DAL;
using System;
using Translate = CTS.Applens.WorkProfiler.DAL.TranslateRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    public class TranslateRepository
    {
        /// <summary>
        ///Logs API Key Error
        /// </summary>
        /// <param name="ProjectID">This parameter holds ProjectID value </param>
        /// <param name="Key">This parameter holds Key value</param>
        /// <param name="Error">This parameter holds Error value</param>
        /// <returns></returns>
        public void LogTranslateAPIKeyError(long projectID, string key, string error)
        {
            try
            {
                Translate.LogTranslateAPIKeyError(projectID, key, error);
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
        public void LogTranslateAPIError(CommonTicketProperty ticketProp, string translateText,
            string errorScope, string errorMessage)
        {
            try
            {
                Translate.LogTranslateAPIError(ticketProp, translateText, errorScope, errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
