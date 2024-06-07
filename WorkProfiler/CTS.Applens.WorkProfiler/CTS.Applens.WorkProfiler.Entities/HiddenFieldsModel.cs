using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds HiddenUserProjectID details
    /// </summary>
    public class HiddenUserProjectID
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets CustomerTimeZone
        /// </summary>
        public string CustomerTimeZone { get; set;}
        /// <summary>
        /// Gets or sets UserTimeZone
        /// </summary>
        public string UserTimeZone { get; set; }
        /// <summary>
        /// Gets or Sets IsApplensAsALM
        /// </summary>
        public bool IsApplensAsALM { get; set; }
        /// <summary>
        /// Gets or Sets EsaProjectId
        /// </summary>
        public string EsaProjectId { get; set; }
        /// <summary>
        /// Gets or Sets IsExempted
        /// </summary>
        public bool IsExempted { get; set; }
    }

    /// <summary>
    /// This class holds Hidden Scope
    /// </summary>
    public class HiddenScope
    {
        /// <summary>
        /// Gets or Sets Project Id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Gets or sets Scope
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// Gets or sets ScopeName
        /// </summary>
        public string ScopeName { get; set; }
    }
    /// <summary>
    /// This class holds Hidden Project Percentage
    /// </summary>
    public class HiddenProjectPercentage
    {
        /// <summary>
        /// Gets or Sets Project Id
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets Tile Id
        /// </summary>
        public  Int16 TileId { get; set; }
        /// <summary>
        /// Gets or sets Tile Progress Percentage
        /// </summary>
        public int TileProgressPercentage { get; set; }
    }
    public class HcmSupervisorList
    {
        public int ProjectID { get; set; }
        public int CustomerID { get; set; }
        public string HcmSupervisorID { get; set; }

    }
    /// <summary>
    /// This class holds HiddenFieldsModel details
    /// </summary>
    public class HiddenFieldsModel
    {
        private List<HiddenUserProjectID> lstProjectUserID = new List<HiddenUserProjectID>();
        private List<HiddenScope> lstScope = new List<HiddenScope>();
        private List<HiddenProjectPercentage> lstProjectPercentage = new List<HiddenProjectPercentage>();
        private List<HcmSupervisorList> lstHCMSupervisorlist = new List<HcmSupervisorList>();
        /// <summary>
        /// Gets or sets LstProjectUserID 
        /// </summary>

        public List<HiddenUserProjectID> LstProjectUserID
        {
            get
            {
                return lstProjectUserID;
            }
            set
            {
                lstProjectUserID = value;
            }
        }
        /// <summary>
        /// Gets or Sets LstScope
        /// </summary>
        public List<HiddenScope> LstScope
        {
            get
            {
                return lstScope;
            }
            set
            {
                lstScope = value;
            }
        }
        /// <summary>
        /// Gets or Sets LstScope
        /// </summary>
        public List<HiddenProjectPercentage> LstProjectPercentage
        {
            get
            {
                return lstProjectPercentage;
            }
            set
            {
                lstProjectPercentage = value;
            }
        }
        public List<HcmSupervisorList> LstHCMSupervisorlist
        {
            get
            {
                return lstHCMSupervisorlist;
            }
            set
            {
                lstHCMSupervisorlist = value;
            }
        }
   
        /// <summary>
        /// Gets or sets CustomerID
        /// </summary>
        [MaxLength(50)]
        public string CustomerId { get; set; }
        /// <summary>
        /// Gets or sets CustomerName
        /// </summary>
        [MaxLength(200)]
        public string CustomerName { get; set; }
        /// <summary>
        /// Gets or sets CustomerTimeZoneID
        /// </summary>
        [MaxLength(8)]
        public string CustomerTimeZoneId { get; set; }
        /// <summary>
        /// Gets or sets CustomerTimeZoneName
        /// </summary>
        [MaxLength(400)]
        public string CustomerTimeZoneName { get; set; }
        /// <summary>
        /// Gets or sets EmployeeID
        /// </summary>
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets IsCognizant
        /// </summary>
        public int IsCognizant { get; set; }
        /// <summary>
        /// Gets or sets IsEffortConfigured
        /// </summary>
        public int IsEffortConfigured { get; set; }
        /// <summary>
        /// Gets or sets EmployeeName
        /// </summary>
        [MaxLength(400)]
        public string EmployeeName { get; set; }
        /// <summary>
        /// Gets or sets IsDebtEngineEnabled
        /// </summary>
        public int IsDebtEngineEnabled { get; set; }
        /// <summary>
        /// Gets or sets IsDaily
        /// </summary>
        public int IsDaily { get; set; }
        /// <summary>
        /// Gets or sets RoleName
        /// </summary>
        [MaxLength(400)]
        public string RoleName { get; set; }
        /// <summary>
        /// Gets or sets ApiURL
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ApiURL { get; set; }
        /// <summary>
        /// Gets or sets ApiKeyHandler
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string ApiKeyHandler { get; set; }
        /// <summary>
        /// Gets or sets APIValueHandler
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string APIValueHandler { get; set; }
        /// <summary>
        /// Gets or sets APIAuthKeyHandler
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string APIAuthKeyHandler { get; set; }
        /// <summary>
        /// Gets or sets APIAuthValueHandler
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string APIAuthValueHandler { get; set; }

        [MaxLength(int.MaxValue)] 
        public string IsADMApplicableforCustomer { get; set; }

        [MaxLength(int.MaxValue)] 
        public string IsExtended { get; set; }

        [MaxLength(int.MaxValue)] 
        public string ChooseDaysCount { get; set; }
    }
}
