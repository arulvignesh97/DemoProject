using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds ErrorLog details
    /// </summary>
    public class ErrorLog
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets FileName 
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets UploadedDate
        /// </summary>
        public DateTime UploadedDate { get; set; }
        /// <summary>
        /// Gets or sets UploadSource
        /// </summary>
        public string UploadSource { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets loadedtickets
        /// </summary>
        public Int64 LoadedTickets { get; set; }
        /// <summary>
        /// Gets or sets rejectedtickets
        /// </summary>
        public int RejectedTickets { get; set; }
        /// <summary>
        /// Gets or sets reuploadedtickets
        /// </summary>
        public int ReUploadedTickets { get; set; }
    }
    /// <summary>
    /// Config
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets ModuleName
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// Gets or sets Active
        /// </summary>
        public string IsActive { get; set; }
    }

    /// <summary>
    /// ErrorLogParam
    /// </summary>
    public class ErrorLogParam
    {
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string CustomerID { get; set; }
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ProjectID { get; set; }
        /// <summary>
        /// Gets or sets choice
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Choice { get; set; }
        /// <summary>
        /// Gets or sets Filename
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets Path
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets MenuRole
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string MenuRole { get; set; }
    }

    public class GetUserInfo
    {
        /// <summary>
        /// Get or set the Esa Project Id
        /// </summary>
        [MaxLength(50)]
        public string EsaProjectID { get; set; }
        /// <summary>
        /// Gets or sets the ProjectName.
        /// </summary>      
        [MaxLength(500)]
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>      
        [MaxLength(50)]
        public string ProjectID { get; set; }
        /// <summary>
        /// Get or set the Closed Date from 
        /// </summary>
        [MaxLength(50)]
        public string ClosedDateFrom { get; set; }

        /// <summary>
        /// Get or set the Closed Date to
        /// </summary>
        [MaxLength(50)]
        public string ClosedDateTo { get; set; }

        /// <summary>
        /// get the Application or the TowerId
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string AppTowerId { get; set; }
        /// <summary>
        /// get is the app or tower project id
        /// </summary>
        [MaxLength(50)]
        public string ispureApp { get; set; }
        /// <summary>
        /// To get associateid
        /// </summary>
        [MaxLength(50)]
        public string userID { get; set; }

    }
    /// <summary>
    /// Holds File Params
    /// </summary>
    public class Fileparam
    {
        /// <summary>
        /// gets or sets fileName
        /// </summary>
        [MaxLength(2000)]
        public string fileName { get; set; }
    }
    public class Debtparam
    {
        /// <summary>
        /// Get or set the Esa Project Id
        /// </summary>
        [MaxLength(50)]
        public string EsaProjectID { get; set; }
        /// <summary>
        /// Gets or sets the ProjectName.
        /// </summary>      
        [MaxLength(500)]
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>      
        [MaxLength(50)]
        public string ProjectID { get; set; }
        /// <summary>
        /// Get or set the Closed Date from 
        /// </summary>
        [MaxLength(50)]
        public string ClosedDateFrom { get; set; }

        /// <summary>
        /// Get or set the Closed Date to
        /// </summary>
        [MaxLength(50)]
        public string ClosedDateTo { get; set; }

        /// <summary>
        /// get the Application or the TowerId
        /// </summary>
        [MaxLength(Int32.MaxValue)]
        public string AppTowerId { get; set; }
        /// <summary>
        /// get is the app or tower project id
        /// </summary>
        [MaxLength(50)]
        public string ispureApp { get; set; }
        /// <summary>
        /// To get associateid
        /// </summary>
        [MaxLength(50)]
        public string userID { get; set; }

        /// <summary>
        /// gets or sets fileName
        /// </summary>
        [MaxLength(2000)]
        public string fileName { get; set; }
    }
    /// <summary>
    /// TicketUploadParam
    /// </summary>
    public class TicketUploadParam
    {
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets CustomerId
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string CustomerID { get; set; }

        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ProjectID { get; set; }

        /// <summary>
        /// Gets or sets Filename
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string IsCognizant { get; set; }

        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets IsApp
        /// </summary>
        public Boolean IsApp { get; set; }
        /// <summary>
        /// Gets or sets MenuRole
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string MenuRole { get; set; }   
    }

    /// <summary>
    /// Download
    /// </summary>
    public class Download
    {
        /// <summary>
        /// Gets or sets ESAProjectID
        /// </summary>
        public string ESAProjectID { get; set; }

        /// <summary>
        /// Gets or sets ExcelPath
        /// </summary>
        public string ExcelPath { get; set; }

        /// <summary>
        /// Gets or sets FName
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// Gets or sets fresult
        /// </summary>
        public FileResult fresult { get; set; }
    }

    /// <summary>
    /// ValidateBulkData - Ticket Upload
    /// </summary>
    public class ValidateBulkData
    {
        /// <summary>
        /// Gets or sets dtBulk
        /// </summary>
        public DataTable dtBulk { get; set; }
        /// <summary>
        /// Gets or sets strDest
        /// </summary>
        public List<string> strDest { get; set; }
        /// <summary>
        /// Gets or sets strSource
        /// </summary>
        public List<string> strSource { get; set; }
     }
}