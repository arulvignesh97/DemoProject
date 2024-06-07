using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TranslationDAL = CTS.Applens.WorkProfiler.DAL.Translation;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// This class holds Translation details
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// This method is used to Get project multiligual translate fields
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="projectId">projectId</param>
        /// <param name="employeeId">employeeId</param>
        /// <returns>List of project multiligual translate fields</returns>
        public MultilingualConfigModel GetProjectMultilinugalTranslateFields(string customerId,
            string projectId)
        {
            try
            {
                return new TranslationDAL().GetProjectMultilinugalTranslateFields(customerId, projectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        /// Get MultiLingual Tickets by Ticket ID
        /// </summary>
        /// <param name="TicketID">TicketID</param>
        /// <param name="SupportTypeID">SupportTypeID</param>        
        public void GetTickets(string TicketID, int SupportTypeID)
        {
            try
            {
                new TranslationDAL().GetTickets(TicketID, SupportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Checks if a subscription key is active
        /// </summary>
        /// <returns></returns>
        public List<ConcatenateStrings> CheckIfProjectSubscriptionIsActive()
        {
            try
            {
                return new TranslationDAL().CheckIfProjectSubscriptionIsActive();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To DataTableConvertion
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        public DataTable DataTableConvertion<TSource>(IList<TSource> data)
        {
            try
            {
                return new TranslationDAL().DataTableConvertion<TSource>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Processes tickets for translation
        /// </summary>
        /// <returns></returns> 
        public List<ConcatenateStrings> ProcessForTranslation()
        {
            try
            {
                return new TranslationDAL().ProcessForTranslation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Updates the translated data in Database
        /// </summary>
        /// <param name="objTranslate">ObjTranslate</param>
        /// <returns></returns>
        public Task<Boolean> CallProjectSubscriptionIsActive(TranslateValidation objTranslate)
        {
            try
            {
                return new TranslationDAL().CallProjectSubscriptionIsActive(objTranslate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        /// <summary>
        /// Calls The trabslantor API based on language
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="languageTo">languageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public List<ConcatenateStrings> CallTranslator(List<ConcatenateStrings> conStr,
            string languageTo, string[] from, string key)
        {
            try
            {
                return new TranslationDAL().CallTranslator(conStr, languageTo, from, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calls The trabslantor API
        /// </summary>
        /// <param name="conStr">ConcatenateStrings</param>
        /// <param name="languageTo">LanguageTo</param>
        /// <param name="from">From</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public Task<List<ConcatenateStrings>> PostToMSTranslatorList(List<ConcatenateStrings> conStr, 
            string languageTo, string[] from, string key)
        {
            try
            {
                return new TranslationDAL().PostToMSTranslatorList(conStr, languageTo, from, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Logs Translate API Error
        /// </summary>
        /// <param name="timeTickerID">TimeTickerID</param>
        /// <param name="translateText">TranslateText</param>
        /// <param name="errorScope">ErrorScope</param>
        /// <param name="errorMessage">ErrorMessage</param>
        /// <returns></returns>
        public void LogTranslateAPIError(ErrorLogTranslationTicket errorLogTicket,
            string translateText, string errorScope, string errorMessage)
        {
            try
            {
                new TranslationDAL().LogTranslateAPIError(errorLogTicket, translateText, errorScope, errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// BreakSentence if 5000 above character is sent to translate
        /// </summary>
        /// <param name="lengthText">TimeTickerID</param>
        /// <param name="key">Key</param>
        /// <param name="conStr">ConcatenateStrings</param>      
        /// <returns></returns>
        public Task<dynamic> BreakSentence(string lengthText, string key, ConcatenateStrings conStr)
        {
            try
            {
                return new TranslationDAL().BreakSentence(lengthText, key, conStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            try
            {
                new TranslationDAL().EnableTrustedHosts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}