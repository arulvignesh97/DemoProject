using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using DebtFieldsApproval = CTS.Applens.WorkProfiler.DAL.DebtFieldsApprovalRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// DebtFieldsApprovalRepository
    /// </summary>
    public class DebtFieldsApprovalRepository
    {
        /// <summary>
        /// Initial stage
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="Datefrom">Date from</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="UserID">User ID</param>
        /// <param name="IsSMTicket">IsSMTicket</param>
        /// <param name="IsDARTTicket">IsDARTTicket</param>
        /// <param name="OptFieldProjID">OptField ProjID</param>
        /// <returns></returns>
        public List<GetDebtTicketsForValidation> GetDebtValidateTicketsForML(int ProjectID, DateTime Datefrom,
            DateTime DateTo, string UserID, int OptFieldProjID, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtValidateTicketsForML(ProjectID, Datefrom, DateTo, UserID, OptFieldProjID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updating Icon Details
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <param name="TicketConsidered"></param>
        /// <param name="TicketAnalysed"></param>
        /// <param name="SamplingCount"></param>
        /// <param name="PatternCount"></param>
        /// <param name="ApprovedCount"></param>
        /// <param name="MuteCount"></param>
        /// <param name="userid"></param>
        public void UpdateILCountDetails(string Choose, int ProjectId, int TicketConsidered, int TicketAnalysed,
            int SamplingCount, int PatternCount, int ApprovedCount, int MuteCount, string userid,int SupportID)
        {
            try
            {
                new DebtFieldsApproval().UpdateILCountDetails(Choose, ProjectId, TicketConsidered, TicketAnalysed,
                                                                               SamplingCount, PatternCount, ApprovedCount, MuteCount, userid, SupportID);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// To get noise data and save to db
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="NoiseEliminationJobId">NoiseElimination JobId</param>
        /// <returns></returns>
        public NoiseElimination GetNoiseEliminationData(int projectID, string NoiseEliminationJobId, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetNoiseEliminationData(projectID, NoiseEliminationJobId, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To skip optional field upload
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string UpdateOptUpload(Int64 ProjectID, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateOptUpload(ProjectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to UpdateNoiseSkipAndContinue
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string UpdateNoiseSkipAndContinue(Int64 ProjectID, string EmployeeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateNoiseSkipAndContinue(ProjectID, EmployeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get criteria for project
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtTickets">List of valid tickets</param>
        /// <param name="UserId">User Id</param>
        /// <param name="DateFrom">Date From</param>
        /// <param name="DateTo">Date To</param>
        /// <returns></returns>
        public string SaveDebtTicketDetailsAfterProcessing(Int32 ProjectID,
            List<GetDebtTicketsForValidation> lstDebtTickets, string UserId, string DateFrom, string DateTo,
            int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().SaveDebtTicketDetailsAfterProcessing(ProjectID, lstDebtTickets, UserId, DateFrom, DateTo, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Tickets details for ML
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="UserID">Employee ID</param>
        /// <returns></returns>

        public string MLDatSetBindingForCSVCreation(int ProjectID, string UserID, Int32 SupportType)
        {
            try
            {
                return new DebtFieldsApproval().MLDatSetBindingForCSVCreation(ProjectID, UserID, SupportType);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Error Log insert
        /// </summary>
        /// <param name="ErrorMessage">Error Message</param>
        /// <param name="step">step</param>
        /// <param name="ProjectID">Project ID</param>
        public void ErrorLOG(string ErrorMessage, string step, Int64 ProjectID, string UserID = null)
        {
            try
            {
                new DebtFieldsApproval().ErrorLOG(ErrorMessage, step, ProjectID, UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// To get Master values for sampling screen
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>
        public List<DebtSamplingValues> GetDebtSamplingDataValues(int projectID,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtSamplingDataValues(projectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// save sampling details
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtSampling">Sampling details</param>
        /// <returns></returns>

        public string SaveDebtSamplingDetails(string UserId, string ProjectID,
            List<GetDebtSamplingDetails> lstDebtSampling)
        {
            try
            {
                return new DebtFieldsApproval().SaveDebtSamplingDetails(UserId, ProjectID, lstDebtSampling);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get details for invoking ML Job
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="UserID">Employee ID</param>
        /// <returns></returns>
        public string MLDatSetBindingAfterSamplingForCSVCreation(int projectID, string UserID,Int32 SupportId)
        {
            try
            {
                return new DebtFieldsApproval().MLDatSetBindingAfterSamplingForCSVCreation(projectID, UserID, SupportId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to UpdateSamplingSubmitFlag
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        public string UpdateSamplingSubmitFlag(int ProjectId, string AssociateID,Int32 SupportId)
        {
            try
            {
                return new DebtFieldsApproval().UpdateSamplingSubmitFlag(ProjectId, AssociateID, SupportId);
            }
            catch (Exception)
            {
                throw;
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
                return new DebtFieldsApproval().SubmitDebtSamplingDetails(UserId, ProjectID, lstDebtSampling);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get Project details
        /// </summary>
        /// <param name="EmployeeID">Employee ID</param>
        /// <param name="CustomerID">Customer ID</param>
        /// <returns></returns>
        public List<GetProjectDetailsById> GetProjectDetailsByEmployeeID(string EmployeeID, int CustomerID)
        {
            try
            {
                return new DebtFieldsApproval().GetProjectDetailsByEmployeeID(EmployeeID, CustomerID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Get Sampling Details
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <returns></returns>

        public List<DebtSamplingModel> GetDebtSamplingData(int projectID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtSamplingData(projectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Noise Elimination Flag
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <returns></returns>

        public List<GetMLDetails> UpdateNoiseEliminationFlag(Int64 ProjectId, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateNoiseEliminationFlag(ProjectId, AssociateID, StartDate, EndDate, OptionalFieldProj, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Update Sampling Flag
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <returns></returns>
        public List<GetMLDetails> UpdateSamplingFlag(int ProjectId, string AssociateID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj,Int32 SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateSamplingFlag(ProjectId, AssociateID, StartDate, EndDate, OptionalFieldProj, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert ML Jobid 
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="initialLearningId">initialLearning Id</param>
        /// <param name="MLJobId">ML JobId </param>
        /// <param name="FileName">File Name</param>
        /// <param name="DataPath">Data Path</param>
        /// <param name="JobType">Job Type</param>
        /// <param name="JobMessage">Job Message</param>
        /// <param name="UserID">Employee id</param>
        /// <returns></returns>
        public string InsertMLJobId(int ProjectID, string initialLearningId, string MLJobId, string FileName,
            string DataPath, string JobType, string JobMessage, string UserID,int SupportId)
        {
            try
            {
                return new DebtFieldsApproval().InsertMLJobId(ProjectID, initialLearningId, MLJobId, FileName,
                                                                        DataPath, JobType, JobMessage, UserID, SupportId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// To get hivepath  based on mljobid
        /// </summary>
        /// <param name="MLJobId"></param>
        /// <returns></returns>
        public FilePath GetHivePathnameByJobId(string MLJobId)
        {
            try
            {
                return new DebtFieldsApproval().GetHivePathnameByJobId(MLJobId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get infra hivepath based on mljobid
        /// </summary>
        /// <param name="MLJobId"></param>
        /// <returns></returns>
        public FilePath GetInfraHivePathnameByJobId(string MLJobId)
        {
            try
            {
                return new DebtFieldsApproval().GetInfraHivePathnameByJobId(MLJobId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// get hive path details for  noise elimination
        /// </summary>
        /// <param name="NoiseEliminationJobId">Noise Elimination JobId</param>
        /// <returns></returns>
        public FilePathNoiseEl GetHivePathnameByJobIdForNoiseEl(string NoiseEliminationJobId)
        {
            try
            {
                return new DebtFieldsApproval().GetHivePathnameByJobIdForNoiseEl(NoiseEliminationJobId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// update error status
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="errorData">error Data</param>
        /// <param name="JobStatus">Job Status</param>
        /// <returns></returns>
        public string UpdateErrorStatus(int ProjectID, string MLJobId, string errorData, int JobStatus,
            int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateErrorStatus(ProjectID, MLJobId, errorData, JobStatus, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To save recieved ML Pattern
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateMLPatternFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateMLPatternFromCSV(ProjectId, MLJobId, UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// To save recieved ML Pattern
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateMLPatternFromCSVInfra(int ProjectId, string MLJobId, string UserID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateMLPatternFromCSVInfra(ProjectId, MLJobId, UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get ML Details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public List<GetMLDetails> GetMLDetailsOnLoad(Int32 ProjectID, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetMLDetailsOnLoad(ProjectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// To check the project is Auto Classified
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>

        public string CheckProjectAutoClassified(int ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().CheckProjectAutoClassified(ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// To check if noise file is generated
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="NoiseEliminationJobId">Noise Elimination Job Id </param>
        /// <returns></returns>
        public NoiseElimination CheckIfNoiseOutputFileGenerated(int ProjectID, string NoiseEliminationJobId,
            int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().CheckIfNoiseOutputFileGenerated(ProjectID, NoiseEliminationJobId, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// For downloading ML Base details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string UpdatePatternId(Int32 ProjectID,int SupportTypeId)
        {
            try
            {
                return new DebtFieldsApproval().UpdatePatternId(ProjectID, SupportTypeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        /// <summary>
        /// To check whether ML File is generated
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="JobType">Job Type</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string CheckIfMLFileGenerated(int ProjectID, string MLJobId, string JobType, string UserID,
            int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().CheckIfMLFileGenerated(ProjectID, MLJobId, JobType, UserID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable SaveMLBase(string HiveFilepath, string mlBaseFile, string mlOutputFile,
            string FileErrorOutputPresent,string DownloadPath,int SupportTypeID,Int64 ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().SaveMLBase(HiveFilepath, mlBaseFile, mlOutputFile, FileErrorOutputPresent, DownloadPath, SupportTypeID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// to update sampled tickets
        /// </summary>
        /// <param name="ProjectId">Project Id</param>
        /// <param name="MLJobId">ML Job Id</param>
        /// <param name="UserID">User ID</param>
        /// <returns></returns>
        public string UpdateSampledTicketsFromCSV(int ProjectId, string MLJobId, string UserID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateSampledTicketsFromCSV(ProjectId, MLJobId, UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// AddTask
        /// </summary>
        /// <param name="projectID"></param>
        /// /// <param name="employeeID"></param>
        /// /// <param name="Option"></param>
        /// <returns></returns>
        public void AddTask(long projectID, string employeeID, int Option)
        {
            try
            {
                new DebtFieldsApproval().AddTask(projectID, employeeID, Option);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get processin details of project on load
        /// </summary>
        /// <param name="ProjectID">Project id</param>
        /// <returns></returns>
        public MLSamplingProcess GetProcessingRequiredOnLoad(int ProjectID,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetProcessingRequiredOnLoad(ProjectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //update ml Flag

        /// <summary>
        ///  update ml Flag
        /// </summary>
        /// <param name="StartDate">Min value of the date</param>
        /// <param name="EndDate">End date</param>
        /// <param name="OptionalFieldProj">Optional Field</param>
        /// <param name="ProjectID">ID OF THE PROJECT</param>
        /// <param name="UserID">ID of the Employee</param>
        /// <returns></returns>
        public List<GetMLDetails> MLUpdateInitialLearningStateDetails(int ProjectID, string UserID,
            DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().MLUpdateInitialLearningStateDetails(ProjectID, UserID, StartDate, EndDate, OptionalFieldProj, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ML Base details download
        /// </summary>
        /// <param name="dtBaseData">base data for ml pattern</param>
        /// <returns></returns>
        public string ExportToExcelForMLBaseDetails(DataTable dtBaseData,int SupportTypeId)
        {
            try
            {
                return new DebtFieldsApproval().ExportToExcelForMLBaseDetails(dtBaseData, SupportTypeId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// download ticket details 
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns></returns>
        public string ExportToExcelForML(int ProjectID, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().ExportToExcelForML(ProjectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to InsertCheck
        /// </summary>
        /// <param name="ErrorMessage"></param>
        public void InsertCheck(string ErrorMessage)
        {
            try
            {
                new DebtFieldsApproval().InsertCheck(ErrorMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to ExcelTodataset
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DataSet ExcelToDataSet(string filename)
        {
            try
            {
                return new DebtFieldsApproval().ExcelToDataSet(filename);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method is used to GetListTicketMasterDebtFieldsFrom
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public List<TicketMasterModel> GetListTicketMasterDebtFieldsFrom(string UserID, Int64 ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().GetListTicketMasterDebtFieldsFrom(UserID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To save for upload
        /// </summary>
        ///<param name="filename" name of the file></param>
        /// <param name="filePath" path of the file></param>
        /// <param name="AssociateID" ID of Employee></param>
        /// <param name="ProjectID" ID of the project></param>
        ///<param name="OptfieldProj" Optional field for the project></param>
        /// <returns></returns>

        public string ProcessFileUpload(string filename, string filePath, string AssociateID,
            Int32 ProjectID, Int16 OptfieldProj, int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().ProcessFileUpload(filename, filePath, AssociateID, ProjectID, OptfieldProj, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  To get  the master fields for debt
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>

        public List<TicketMasterModel> GetListTicketMasterDebtFieldsTo(string UserID, Int64 ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().GetListTicketMasterDebtFieldsTo(UserID, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To Signoff the ML patterns
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SignOffDebtPatternValidation(int ProjectID, DateTime Datefrom, string UserID,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().SignOffDebtPatternValidation(ProjectID, Datefrom, UserID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        string jobSuccess;
        /// <summary>
        ///  To get the list of optional field based on projectid
        /// </summary>      
        /// <param name="ProjectID"></param>
        /// <returns></returns>


        public List<ProjOptionalFields> GetOptionalFieldsOnLoad(int projectID)
        {
            try
            {
                return new DebtFieldsApproval().GetOptionalFieldsOnLoad(projectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To get the details for sampling
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string SamplingDatSetBindingForCSVCreation(MLDetailsParam MLUserdetails)
        {
            try
            {
                return new DebtFieldsApproval().SamplingDatSetBindingForCSVCreation(MLUserdetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        ///  To submit for noise elimination
        /// </summary>
        /// <param name="dsResult">dataset of valid ticket details</param>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="UserID">Cog ID</param>
        /// <returns></returns>
        public string SubmitNoiseEliminationJob(DataSet dsResult, int ProjectID, string UserID,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().SubmitNoiseEliminationJob(dsResult, ProjectID, UserID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To get the ticket details for noise elimination
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="AssociateID">Employee ID</param>
        /// <returns></returns>
        public string GetTicketsForNoiseElimination(string ProjectID, string AssociateID,Int32 SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetTicketsForNoiseElimination(ProjectID, AssociateID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This Method is used to UpdateNoiseSkipAndContinue
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public string UpdateNoiseInfraSkipAndContinue(Int32 ProjectID, string EmployeeID)
        {
            try
            {
                return new DebtFieldsApproval().UpdateNoiseInfraSkipAndContinue(ProjectID, EmployeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Used to get the Pattern details of a project
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="Datefrom">Date from</param>
        /// <param name="DateTo">Date To</param>
        /// <param name="ID">Employee ID</param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternValidationReport(int projectID, string ID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtMLPatternValidationReport(projectID, ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Used to get details for view all screen
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public SpDebtMLPatternValidationModelForViewAll GetDebtMLPatternValidationReportForViewAll(int projectID,
            int supportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtMLPatternValidationReportForViewAll(projectID, supportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Used to get pattern occurence Report
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="AppIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="subPattern"></param>
        /// <param name="AdditionalTextPattern"></param>
        /// <param name="AdditionalTextsubPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<SpDebtMLPatternValidationModel> GetDebtMLPatternOccurenceReport(int projectID, string AppIds,
            string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextsubPattern,
            int causeCodeId, int ResolutionCodeID)
        {

            try
            {
                return new DebtFieldsApproval().GetDebtMLPatternOccurenceReport(projectID, AppIds,
            TicketPattern, subPattern, AdditionalTextPattern, AdditionalTextsubPattern, causeCodeId, ResolutionCodeID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  To save ml pattern details
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="lstDebtMLPatternModel">ML Details</param>
        /// <param name="UserId">User Id</param>
        /// <returns></returns>
        public string SaveDebtPatternValidationDetails(int ProjectID,
            List<DebtMLPatternValidationModel> lstDebtMLPatternModel, string UserId,int SupportType)
        {

            try
            {
                return new DebtFieldsApproval().SaveDebtPatternValidationDetails(ProjectID, lstDebtMLPatternModel, UserId, SupportType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Used to generate patterns functionality
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <param name="lstGeneratePatternApps">Application details</param>
        /// <param name="UserId">User Id</param>
        /// <param name="CustomerID">Customer ID</param>
        /// <returns></returns>
        public string Generatepatterns(int ProjectID, List<RegenerateApplicationDetails> lstGeneratePatternApps,
            string UserId, int CustomerID)
        {
            try
            {
                return new DebtFieldsApproval().Generatepatterns(ProjectID, lstGeneratePatternApps, UserId, CustomerID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Used to get copy patterns
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstCopyPatternsModel"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string CopyPatterns(int ProjectID, List<DebtMLPatternValidationModel> lstCopyPatternsModel,
            string UserId)
        {
            try
            {
                return new DebtFieldsApproval().CopyPatterns(ProjectID, lstCopyPatternsModel, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        ///  Used to get the Master value of Debt fields
        /// </summary>
        /// <param name="projectID">project ID</param>

        /// <returns></returns>
        public List<SpDebtMasterValues> GetDebtMasterValues(int projectID,int SupportTypeID)

        {
            try
            {
                return new DebtFieldsApproval().GetDebtMasterValues(projectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
               
        /// <summary>
        /// This Method is used to SaveNoiseEliminationDetails
        /// </summary>
        /// <param name="NoiseData"></param>
        /// <param name="Projectid"></param>
        /// <param name="Choose"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public string SaveNoiseEliminationDetails(NoiseElimination NoiseData, int Projectid, int Choose,
            string EmployeeId)
        {
            try
            {
                return new DebtFieldsApproval().SaveNoiseEliminationDetails(NoiseData, Projectid, Choose, EmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method is used to GetFilteredNoiseEliminationData
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public NoiseElimination GetFilteredNoiseEliminationData(string Selection, int Filter, int ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().GetFilteredNoiseEliminationData(Selection, Filter, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method is used to GetIconDetails
        /// </summary>
        /// <param name="Choose"></param>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public IconDetails GetIconDetails(string Choose, int ProjectId, int SupportID)
        {
            try
            {
                return new DebtFieldsApproval().GetIconDetails(Choose, ProjectId, SupportID);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This Method is used to CreateInitialLearningID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public string CreateInitialLearningID(int ProjectID, string employeeID)
        {
            try
            {
                return new DebtFieldsApproval().CreateInitialLearningID(ProjectID, employeeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Method to Validate ML
        /// </summary>
        /// <param name="CriteriaMet"></param>
        /// <param name="ProjectID"></param>
        /// <param name="UserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OptionalFieldProj"></param>
        /// <returns></returns>
        public ILValidationResult ValidateML(string CriteriaMet, Int64 ProjectID, string UserID, DateTime StartDate,
            DateTime EndDate, int OptionalFieldProj,int SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().ValidateML(CriteriaMet, ProjectID, UserID, StartDate, EndDate, OptionalFieldProj, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        /// <summary>
        /// ReWrite Module for Jquery Method GetMLDetailsOnLoad
        /// </summary>
        /// <param name="MLDetails"></param>
        public ILValidationResult MLDetailsLoad(List<GetMLDetails> MLDetails)
        {
            try
            {
                return new DebtFieldsApproval().MLDetailsLoad(MLDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// This Method is used to Get FilteredNoiseEliminationData for Infra
        /// </summary>
        /// <param name="Selection"></param>
        /// <param name="Filter"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public NoiseEliminationInfra GetFilteredNoiseEliminationDataInfra(string Selection, int Filter, int ProjectID)
        {
            try
            {
                return new DebtFieldsApproval().GetFilteredNoiseEliminationDataInfra(Selection, Filter, ProjectID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To get noise data and save to Infra table in DB
        /// </summary>
        /// <param name="projectID">project ID</param>
        /// <param name="NoiseEliminationJobId">NoiseElimination JobId</param>
        /// <returns></returns>
        public NoiseEliminationInfra GetNoiseEliminationInfraData(int projectID, string NoiseEliminationJobId)
        {
            try
            {
                return new DebtFieldsApproval().GetNoiseEliminationInfraData(projectID, NoiseEliminationJobId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// To check if noise file is generated for Infra
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <param name="NoiseEliminationJobId">Noise Elimination Job Id </param>
        /// <returns></returns>
        public NoiseEliminationInfra CheckIfNoiseOutputFileGeneratedInfra(int ProjectID, string NoiseEliminationJobId,
            int SupportId)
        {
            try
            {
                return new DebtFieldsApproval().CheckIfNoiseOutputFileGeneratedInfra(ProjectID, NoiseEliminationJobId, SupportId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get hive path details for  noise elimination
        /// </summary>
        /// <param name="NoiseEliminationJobId">Noise Elimination JobId</param>
        /// <returns></returns>
        public FilePathNoiseEl GetHivePathnameByJobIdForInfraNoiseEl(string NoiseEliminationJobId)
        {
            try
            {
                return new DebtFieldsApproval().GetHivePathnameByJobIdForInfraNoiseEl(NoiseEliminationJobId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to Save NoiseElimination Infra Details in Infra Table
        /// </summary>
        /// <param name="NoiseData"></param>
        /// <param name="Projectid"></param>
        /// <param name="Choose"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public string SaveNoiseEliminationInfraDetails(NoiseEliminationInfra NoiseData, int Projectid, int Choose,
            string EmployeeId)
        {
            try
            {
                return new DebtFieldsApproval().SaveNoiseEliminationInfraDetails(NoiseData, Projectid, Choose, EmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get pattern Details for infra configured project
        /// </summary>
        /// <param name="projectID">project id</param>
        /// <param name="ID">User id</param>

        public List<MLPatternValidationInfra> GetMLpatternValidationReport(int projectID, string ID)
        {
            try
            {
                return new DebtFieldsApproval().GetMLpatternValidationReport(projectID, ID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Method to get DebtMLPatternOccurenceReporInfra
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="TowerIds"></param>
        /// <param name="TicketPattern"></param>
        /// <param name="subPattern"></param>
        /// <param name="AdditionalTextPattern"></param>
        /// <param name="AdditionalTextsubPattern"></param>
        /// <param name="causeCodeId"></param>
        /// <param name="ResolutionCodeID"></param>
        /// <returns></returns>
        public List<MLPatternValidationInfra> GetDebtMLPatternOccurenceReporInfra(int projectID, string TowerIds,
    string TicketPattern, string subPattern, string AdditionalTextPattern, string AdditionalTextsubPattern,
    int causeCodeId, int ResolutionCodeID)
        {
            try
            {
                return new DebtFieldsApproval().GetDebtMLPatternOccurenceReporInfra(projectID, TowerIds, TicketPattern, subPattern, AdditionalTextPattern, 
                    AdditionalTextsubPattern, causeCodeId, ResolutionCodeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// GetRegenerateILDetails - used to get the Application , Tower Details based on the Support type
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="SupportTypeID"></param>
        /// <returns></returns>
        public RegenerateILDetails GetRegenerateILDetails(Int32 projectID, Int16 SupportTypeID)
        {
            try
            {
                return new DebtFieldsApproval().GetRegenerateILDetails(projectID, SupportTypeID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Generate Infra Pattern
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="lstGeneratePatternApps"></param>
        /// <param name="UserId"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string GenerateInfrapatterns(int ProjectID, List<RegenerateApplicationDetails> lstGeneratePatternApps,
            string UserId, int CustomerID)
        {
            try
            {
                return new DebtFieldsApproval().GenerateInfrapatterns(ProjectID, lstGeneratePatternApps, UserId, CustomerID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

