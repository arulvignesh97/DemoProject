using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using InitialLearning = CTS.Applens.WorkProfiler.DAL.InitialLearningRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    public class InitialLearningRepository
    {
        /// <summary>
        /// Function to get the Configured Values for the project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        public MLDetails GetTopFiltersOnLoad(Int32 ProjectID,int SupportTypeID)
        {
            try
            {
                return new InitialLearning().GetTopFiltersOnLoad(ProjectID, SupportTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Sampling Details
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<DebtSamplingModel> GetInfraDebtSamplingData(int projectID)
        {
            try
            {
                return new InitialLearning().GetInfraDebtSamplingData(projectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// to update sampled tickets
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateInfraSampledTicketsFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            try
            {
                return new InitialLearning().UpdateInfraSampledTicketsFromCSV(ProjectId, MLJobId, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SaveDebtSamplingDetails
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="lstDebtSampling">lstDebtSampling</param>
        /// <returns></returns>
        public string SaveDebtSamplingDetails(string UserId, string ProjectID,List<GetDebtSamplingDetails> lstDebtSampling)
        {
            try
            {
                return new InitialLearning().SaveDebtSamplingDetails(UserId, ProjectID, lstDebtSampling);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Submit Sampling details
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtSampling">List of sampling details</param>
        /// <returns></returns>
        public string SubmitDebtSamplingDetails(string UserId, int ProjectID,
            List<GetDebtSamplingDetails> lstDebtSampling)
        {
            try
            {
                return new InitialLearning().SubmitDebtSamplingDetails(UserId, ProjectID, lstDebtSampling);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
