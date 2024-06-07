using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.DAL.Contracts
{
    /// <summary>
    /// Interface for Continuous Learning
    /// </summary>
    interface IContinuousLearningRepository
    {
        /// <summary>
        /// This Method Is Used To GetDropDownValuesBU
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        List<BusinessUnit> GetDropDownValuesBU(string employeeID);
        /// <summary>
        /// This Method Is Used To GetDropDownValuesAccount
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="BUID"></param>
        /// <returns></returns>
        List<Account> GetDropDownValuesAccount(string employeeID, int BUID);
        /// <summary>
        ///  This Method Is Used To GetDropDownValuesProjectPortfolio
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsPortfolio"></param>
        /// <returns></returns>
        ContinuousLearningList GetDropDownValuesProjectPortfolio(string employeeID, long CustomerID,
            long ProjectID, int IsPortfolio);
        /// <summary>
        ///  This Method Is Used To GetDropDownValuesApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="portfolioID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        List<Application> GetDropDownValuesApplication(long projectID, long portfolioID, long CustomerID);
        /// <summary>
        ///  This Method Is Used To ToShowCLConfig
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        CLConfig ToShowCLConfig(int projectID, string UserID);
        /// <summary>
        ///  This Method Is Used To SaveConfigValues
        /// </summary>
        /// <param name="clconfig"></param>
        /// <returns></returns>
        string SaveConfigValues(CLConfig clconfig);
        /// <summary>
        /// This Method Is Used To SaveCLPatterns
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        int SaveCLPatterns(CLPatternsSignOff pattern);
        
        /// <summary>
        ///  This Method Is Used To LearningEnrichmentDate
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        LearningEnrichmentDate GetLearningEnrichmentDates(int ProjectId);
    }
}
