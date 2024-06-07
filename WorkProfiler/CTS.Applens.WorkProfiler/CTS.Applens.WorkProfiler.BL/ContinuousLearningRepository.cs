using CTS.Applens.WorkProfiler.Models;
using CTS.Applens.WorkProfiler.Models.ContinuousLearning;
using System;
using System.Collections.Generic;
using ContinuousLearning = CTS.Applens.WorkProfiler.DAL.ContinuousLearningRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// ContinuousLearningRepository
    /// </summary>
    public class ContinuousLearningRepository
    {
        /// <summary>
        /// This Method is used to LearningEnrichmentDate
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>

        public LearningEnrichmentDate GetLearningEnrichmentDates(int ProjectId)
        {
            try
            {
                return new ContinuousLearning().GetLearningEnrichmentDates(ProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  This Method is used to ToShowCLConfig
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public CLConfig ToShowCLConfig(int projectID, string userID)
        {
            try
            {
                return new ContinuousLearning().ToShowCLConfig(projectID, userID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  This Method is used to SaveConfigValues
        /// </summary>
        /// <param name="clconfig"></param>
        /// <returns></returns>
        public string SaveConfigValues(CLConfig clconfig)
        {
            try
            {
                return new ContinuousLearning().SaveConfigValues(clconfig);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to CheckIfAllPatternsAreSignedOff
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public bool CheckIfAllPatternsAreSignedOff(long ProjectID)
        {
            try
            {
                return new ContinuousLearning().CheckIfAllPatternsAreSignedOff(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to GetDropDownValuesBU
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<BusinessUnit> GetDropDownValuesBU(string employeeID)
        {
            try
            {
                return new ContinuousLearning().GetDropDownValuesBU(employeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcel(List<CLExcel> excel)
        {
            try
            {
                return new ContinuousLearning().ExportToExcel(excel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcelTEST(List<TimeSheetDataDaily> excel)
        {
            try
            {
                return new ContinuousLearning().ExportToExcelTEST(excel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to ExportToExcel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public string ExportToExcelWeekly(List<TimeSheetData> excel)
        {
            try
            {
                return new ContinuousLearning().ExportToExcelWeekly(excel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to SaveCLPatterns
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public int SaveCLPatterns(CLPatternsSignOff pattern)
        {
            try
            {
                return new ContinuousLearning().SaveCLPatterns(pattern);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to GetDropDownValuesAccount
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="BUID"></param>
        /// <returns></returns>
        public List<Account> GetDropDownValuesAccount(string employeeID, int BUID)
        {
            try
            {
                return new ContinuousLearning().GetDropDownValuesAccount(employeeID, BUID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to GetDropDownValuesApplication
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="portfolioID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<Application> GetDropDownValuesApplication(long projectID, long portfolioID, long CustomerID)
        {
            try
            {
                return new ContinuousLearning().GetDropDownValuesApplication(projectID, portfolioID, CustomerID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to CheckCognizantCustomer
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public CustomerCognizant CheckCognizantCustomer(string employeeID)
        {
            try
            {
                return new ContinuousLearning().CheckCognizantCustomer(employeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to GetDropDownValuesProjectPortfolio
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="IsPortfolio"></param>
        /// <returns></returns>
        public ContinuousLearningList GetDropDownValuesProjectPortfolio(string employeeID, long CustomerID,
            long ProjectID, int IsPortfolio)
        {
            try
            {
                return new ContinuousLearning().GetDropDownValuesProjectPortfolio(employeeID, CustomerID, ProjectID, IsPortfolio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to GetDebtMLPatternValidationReportContinuous
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <returns></returns>
        public GetDebtPatternValidation GetDebtMLPatternValidationReportContinuous(int ProjectId)
        {
            try
            {
                return new ContinuousLearning().GetDebtMLPatternValidationReportContinuous(ProjectId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to VerifyJobStatus
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public JobDetails VerifyJobStatus(int ProjectID)
        {
            try
            {
                return new ContinuousLearning().VerifyJobStatus(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to GetCLDetails
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<CLDetails> GetCLDetails(int ProjectID)
        {
            try
            {
                return new ContinuousLearning().GetCLDetails(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetDebtMLPatternOccurenceReportContinuous
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternOccurenceReportContinuous(int projectID,
            int PatternApplicationID, int CauseCodeId, int ResolutionCodeId, string TicketPattern,
            string TicketSubPattern, string AddiPattern, string AddiSubPattern)
        {
            try
            {
                return new ContinuousLearning().GetDebtMLPatternOccurenceReportContinuous(projectID,
                           PatternApplicationID, CauseCodeId, ResolutionCodeId, TicketPattern,
                           TicketSubPattern, AddiPattern, AddiSubPattern);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
