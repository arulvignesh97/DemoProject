using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    public enum TicketUploadTrackScenarios
    {
        MandatoryColumnValidation = 1,
        NonMandatoryColumnValidation,
        StoredProcedureStartTime = 5,
        StoredProcedureEndTime
    }
    /// <summary>
    /// TicketUploadTrack
    /// </summary>
    public class TicketUploadTrack
    {

        public long? ProjectId { get; set; }

        public string EmployeeId { get; set; }

        public int? Mode { get; set; }

        public string FileName { get; set; }

        public bool? IsColumnMappingValidated { get; set; }

        public DateTime? MndColValBeginTime { get; set; }

        public DateTime? MndColValEndTime { get; set; }

        public DateTime? NonMndColValBeginTime { get; set; }

        public DateTime? NonMndColValEndTime { get; set; }

        public DateTime? NullValUpdateBeginTime { get; set; }

        public DateTime? NullValUpdateEndTime { get; set; }

        public DateTime? MasterValuesUpdateBeginTime { get; set; }

        public DateTime? MasterValuesUpdateEndTime { get; set; }

        public string BLErrorMessage { get; set; }

        public string DBErrorMessage { get; set; }

        public int? TotalRecordsInExcel { get; set; }

        public int? TotalValidRecords { get; set; }

        public int? TotalDuplicateRecords { get; set; }

        public int? TotalRejectedRecords { get; set; }

        public bool? IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? StoredProcedureStartTime { get; set; }

        public DateTime? StoredProcedureEndTime { get; set; }
    }

    public class WorkPatternColumns
    {
        public string TicketDescriptionBasePattern { get; set; }
        public string TicketDescriptionSubPattern { get; set; }
        public string ResolutionRemarksBasePattern { get; set; }
        public string ResolutionRemarksSubPattern { get; set; }
    }

    /// <summary>
    /// Veracode Fix
    /// </summary>
    public class TicketExcelUploadTrack
    {
        public string result { get; set; }

        public bool isMultilingual { get; set; }
    }

    /// <summary>
    /// Veracode Fix
    /// </summary>
    public class TicketUploadCheckITSM
    {
        public Int32 Percentage { get; set; }

        public string ManualOrAuto { get; set; }

        public string Responce { get; set; }

        public string MandateColumn { get; set; }
        public bool IsTicketDescriptionOpted { get; set; }

    }
    /// <summary>
    /// Gets or sets TicketUploadConfig
    /// </summary>
    public class TicketUploadConfig
    {
        /// <summary>
        /// Gets or sets SharePath
        /// </summary>
        public string SharePath { get; set; }
        /// <summary>
        /// Gets or sets TicketSharePathUsers
        /// </summary>

        public string TicketSharePathUsers { get; set; }
    }
    /// <summary>
    /// Gets or sets EffortUploadConfig
    /// </summary>
    public class EffortUploadConfig
    {
        /// <summary>
        /// Gets or sets SharePathName
        /// </summary>
        public string SharePathName { get; set; }
    }
    /// <summary>
    /// This class holds ConfigList
    /// </summary>
    public class ConfigList
    {
        /// <summary>
        /// Gets or sets TicketUploadconfig details
        /// </summary>
        public List<TicketUploadConfig> TicketUpload { get; set; }
        /// <summary>
        /// Gets or sets EffortUploadConfig details
        /// </summary>
        public List<EffortUploadConfig> EffortUpload { get; set; }
    }
    /// <summary>
    /// This class holds ProjectDetails
    /// </summary>
    public class EffortuploadProjectDetails
    {
        /// <summary>
        /// Gets or sets Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int32 ProjectID { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public bool IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsEffortTrackActivityWise
        /// </summary>
        public bool IsEffortTrackActivityWise { get; set; }
        /// <summary>
        /// Gets or sets IsDaily
        /// </summary>
        public bool IsDaily { get; set; }
        /// <summary>
        /// Gets or sets TrackID
        /// </summary>
        public Int64 TrackID { get; set; }
    }
    #region Associate Lens
    /// <summary>
    /// Get Associate Lens Certification detilas
    /// </summary>
    public class AssociateLensModel
    {
        /// <summary>
        /// Gets or sets the CategoryId.
        /// </summary>
        [MaxLength(500)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the AwardId.
        /// </summaryProjectID
        [MaxLength(500)]
        public string Award { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        [MaxLength(500)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets the EmployeeName.
        /// </summary>
        [MaxLength(500)]
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets the ProjectName.
        /// </summary>
        [MaxLength(500)]
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the AccountId.
        /// </summary>
        [MaxLength(500)]
        public Int64 AccountId { get; set; }

        /// <summary>
        /// Gets or sets the AccountName.
        /// </summary>
        [MaxLength(500)]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the EsaProjectId.
        /// </summary>
        [MaxLength(500)]
        public string EsaProjectId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>
        [MaxLength(500)]
        public Int64 ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the Designation.
        /// </summary>
        [MaxLength(500)]
        public string Designation { get; set; }

        /// <summary>
        /// Gets or sets the CertificationMonth.
        /// </summary>
        public string CertificationMonth { get; set; }

        /// <summary>
        /// Gets or sets the CertificationYear.
        /// </summary>
        public string CertificationYear { get; set; }

        /// <summary>
        /// Gets or sets the NoOfATicketsClosed.
        /// </summary>
        [MaxLength(500)]
        public int NoOfATicketsClosed { get; set; }

        /// <summary>
        /// Gets or sets the NoOfHTicketsClosed.
        /// </summary>
        [MaxLength(500)]
        public int NoOfHTicketsClosed { get; set; }

        /// <summary>
        /// Gets or sets the IncReductionMonth.
        /// </summary>
        public int IncReductionMonth { get; set; }

        /// <summary>
        /// Gets or sets the EffortReductionMonth.
        /// </summary>
        public int EffortReductionMonth { get; set; }

        /// <summary>
        /// Gets or sets the SolutionIdentified.
        /// </summary>
        public int SolutionIdentified { get; set; }

        /// <summary>
        /// Gets or sets the NoOfKEDBCreatedApproved.
        /// </summary>
        [MaxLength(500)]
        public int NoOfKEDBCreatedApproved { get; set; }

        /// <summary>
        /// Gets or sets the NoOfCodeAssetContributed.
        /// </summary>
        [MaxLength(500)]
        public int NoOfCodeAssetContributed { get; set; }

        /// <summary>
        /// Gets or sets the Associate ID.
        /// </summary>
        [MaxLength(200)]
        public List<string> ReferenceId { get; set; }

    }


    /// <summary>
    /// The ScreenAccessParam class holds screens and its access details.
    /// </summary>
    public class AssociateLensTrackModel
    {
        /// <summary>
        /// Gets or sets the Customer Id.
        /// </summary>
        [MaxLength(200)]
        public string ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId.
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>
        [MaxLength(500)]
        public Int64 ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the Associate ID.
        /// </summary>
        [MaxLength(200)]
        public string ReferenceId { get; set; }
    }

    /// <summary>
    /// The ScreenAccessParam class holds screens and its access details.
    /// </summary>
    public class AssociateLensParam
    {
        /// <summary>
        /// Gets or sets the Customer Id.
        /// </summary>
        [MaxLength(200)]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the Associate ID.
        /// </summary>
        [MaxLength(200)]
        public string EndDate { get; set; }
    }
    #endregion

}