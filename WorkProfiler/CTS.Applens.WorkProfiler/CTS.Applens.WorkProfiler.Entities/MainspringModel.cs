using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds MainspringModel details
    /// </summary>
    public class MainspringModel
    {
        /// <summary>
        /// Gets or sets lstMainspringProjectsModel
        /// </summary>
        public List<MainspringProjectModel> LstMainspringProjectsModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringFrequencyModel
        /// </summary>
        public List<MainspringFrequencyModel> LstMainspringFrequencyModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringReportingPeriodModel
        /// </summary>
        public List<MainspringReportingPeriodModel> LstMainspringReportingPeriodModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringServiceListModel
        /// </summary>
        public List<MainspringServiceListModel> LstMainspringServiceListModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringMetricsListModel
        /// </summary>
        public List<MainspringMetricsModel> LstMainspringMetricsListModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringavailabilityModel
        /// </summary>
        public List<MainspringUserDefinedBaseMeasureModel> LstMainspringAvailabilityModel { get; set; }
        /// <summary>
        /// Gets or sets lstMainspringBaseMeasureProgressModel
        /// </summary>
        public List<MainspringBaseMeasureProgressModel> LstMainspringBaseMeasureProgressModel { get; set; }
    }
    /// <summary>
    /// This class holds MainspringProjectModel details
    /// </summary>
    public class MainspringProjectModel
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Gets or sets ProjectName
        /// </summary>
        public string ProjectName { get; set; }     
        /// <summary>
        /// Gets or sets BaseMeasureValue
        /// </summary>
        public string BaseMeasureValue { get; set; }
        /// <summary>
        /// Gets or sets IsMainSpringConfigured
        /// </summary>
        public string IsMainSpringConfigured { get; set; }
        /// <summary>
        /// Gets or sets SupportCategoryCount
        /// </summary>
        public int SupportCategoryCount { get; set; }
        /// <summary>
        /// Gets or sets IsODCRestricted
        /// </summary>
        public string IsODCRestricted { get; set; }
        /// <summary>
        /// Gets or sets FrequencyId
        /// </summary>
        public string FrequencyId { get; set; }
        /// <summary>
        /// Gets or sets FrequencyName
        /// </summary>
        public string FrequencyName { get; set; }
        /// <summary>
        /// Gets or sets RowIndex
        /// </summary>
        public string RowIndex { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriodId
        /// </summary>
        public string ReportPeriodId { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriod
        /// </summary>
        public string ReportPeriod { get; set; }
        /// <summary>
        /// Gets or sets Service
        /// </summary>
        public List<ServiceList> Lstservice { get; set; }
        /// <summary>
        /// Gets or sets Metrics
        /// </summary>
        public List<MetricList> LstMetrics { get; set; }
    }
    public class ServiceList
    {
        /// <summary>
        /// Gets or sets ServiceId
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
    }
    public class MetricList
    {
        /// <summary>
        /// Gets or sets MetricId
        /// </summary>
        public string MetricId { get; set; }
        /// <summary>
        /// Gets or sets MetricName
        /// </summary>
        public string MetricName { get; set; }
        /// <summary>
        /// Gets or sets UOMDESC
        /// </summary>
        public string UOMDESC { get; set; }
        /// <summary>
        /// Gets or sets UOMDataType
        /// </summary>
        public string UOMDataType { get; set; }
        /// <summary>
        /// Gets or sets ServiceId
        /// </summary>
        public string ServiceId { get; set; }
    }
    /// <summary>
    /// This class holds MainspringFrequencyModel details
    /// </summary>
    public class MainspringFrequencyModel
    {
        /// <summary>
        /// Gets or sets FrequencyID
        /// </summary>
        public string FrequencyId { get; set; }
        /// <summary>
        /// Gets or sets FrequencyName
        /// </summary>
        public string FrequencyName { get; set; }
    }
    /// <summary>
    /// This class holds MainspringReportingPeriodModel details
    /// </summary>
    public class MainspringReportingPeriodModel
    {
        /// <summary>
        /// Gets or sets RowIndex
        /// </summary>
        public string RowIndex { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriodID
        /// </summary>
        public string ReportPeriodId { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriod
        /// </summary>
        public string ReportPeriod { get; set; }
        /// <summary>
        /// Gets or sets FrequencyID
        /// </summary>
        public string FrequencyId { get; set; }
    }
    /// <summary>
    /// This class holds MainspringServiceListModel details
    /// </summary>
    public class MainspringServiceListModel
    {
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
    }
    /// <summary>
    /// This class holds MainspringMetricsModel details
    /// </summary>
    public class MainspringMetricsModel
    {
        /// <summary>
        /// Gets or sets MetricID
        /// </summary>
        public string MetricId { get; set; }
        /// <summary>
        /// Gets or sets MetricName
        /// </summary>
        public string MetricName { get; set; }
        /// <summary>
        /// Gets or sets UOMDESC
        /// </summary>
        public string UOMDESC { get; set; }
        /// <summary>
        /// Gets or sets UOMDataType
        /// </summary>
        public string UOMDataType { get; set; }
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public string ServiceId { get; set; }
    }
    /// <summary>
    /// This class holds MainspringUserDefinedBaseMeasureModel details
    /// </summary>
    public class MainspringUserDefinedBaseMeasureModel
    {
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceName
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureID
        /// </summary>
        public int BaseMeasureId { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureName
        /// </summary>
        public string BaseMeasureName { get; set; }
        /// <summary>
        /// Gets or sets UOMDESC
        /// </summary>
        public string UOMDesc { get; set; }
        /// <summary>
        /// Gets or sets UOMDataType
        /// </summary>
        public string UOMDataType { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureValue
        /// </summary>
        public string BaseMeasureValue { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureTypeID
        /// </summary>
        public int BaseMeasureTypeId { get; set; }
        /// <summary>
        /// Gets or sets MetricID
        /// </summary>
        public int MetricId { get; set; }
        /// <summary>
        /// Gets or sets MetricName
        /// </summary>
        public string MetricName { get; set; }
        /// <summary>
        /// Gets or sets PRIORITYID
        /// </summary>
        public string PriorityId { get; set; }
        /// <summary>
        /// Gets or sets MainspringPriorityName
        /// </summary> 
        public string MainspringPriorityName { get; set; }
        /// <summary>
        /// Gets or sets SUPPORTCATEGORY
        /// </summary>
        public string SupportCategory { get; set; }
        /// <summary>
        /// Gets or sets MainspringSUPPORTCATEGORYName
        /// </summary>
        public string MainspringSupportCategoryName { get; set; }
        /// <summary>
        /// Gets or sets TECHNOLOGY
        /// </summary>
        public string Technology { get; set; }
        /// <summary>
        /// Gets or sets UOMID
        /// </summary>
        public int UOMId { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryValue
        /// </summary>
        public string TicketSummaryValue { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryBaseID
        /// </summary>
        public int TicketSummaryBaseId { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryBaseName
        /// </summary>
        public string TicketSummaryBaseName { get; set; }
    }
    public class SearchBaseMeasureParams : MainspringUserDefinedBaseMeasureModel
    {
        /// <summary>
        /// Gets or sets ServiceMetricBaseMeasureId
        /// </summary>
        public int ServiceMetricBaseMeasureId { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriodID
        /// </summary>
        public int ReportPeriodID { get; set; }
    }
    
    /// <summary>
    /// This class holds MainspringUserDefinedBaseMeasureModel details
    /// </summary>
    public class MainspringUDODCBaseMeasureModel
    {
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureID
        /// </summary>
        public int BaseMeasureId { get; set; }
        /// <summary>
        /// Gets or sets PRIORITYID
        /// </summary>
        public string PriorityId { get; set; }     
        /// <summary>
        /// Gets or sets SUPPORTCATEGORY
        /// </summary>
        public string SupportCategory { get; set; }
        /// <summary>
        /// Gets or sets TECHNOLOGY
        /// </summary>
        public string Technology { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureValue
        /// </summary>
        public string BaseMeasureValue { get; set; }
    }

    /// <summary>
    /// This class holds MainspringTicketSummaryBaseMeasureModel details
    /// </summary>
    public class MainspringTicketSummaryBaseMeasureModel
    {
        /// <summary>
        /// Gets or sets ServiceID
        /// </summary>
        public Int32 ServiceId { get; set; }
        /// <summary>
        /// Gets or sets TicketSummaryBaseMeasureID
        /// </summary>
        public Int32 TicketSummaryBaseMeasureId { get; set; }
        /// <summary>
        /// Gets or sets MainspringPriorityID
        /// </summary>
        public string MainspringPriorityId { get; set; }
        /// <summary>
        /// Gets or sets MainspringSUPPORTCATEGORYID
        /// </summary>
        public string MainspringSupportCategoryId { get; set; }
        /// <summary>
        /// Gets or sets TicketBaseMeasureValue
        /// </summary>
        public string TicketBaseMeasureValue { get; set; }
    }

    public class MainspringUDBaseMeasureModel
    {
        /// <summary>
        /// Gets or sets ReportPeriodID
        /// </summary>
        public Int64 ServiceId { get; set; }
        /// <summary>
        /// Gets or sets ServiceMetricBaseMeasureId
        /// </summary>
        public Int64 BaseMeasureId { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureValue
        /// </summary>
        public string BaseMeasureValue { get; set; }
    }


    /// <summary>
    /// This class holds MainspringBaseMeasureProgressModel details
    /// </summary>
    public class MainspringBaseMeasureProgressModel
    {
        /// <summary>
        /// Gets or sets ValuesAvailableCount
        /// </summary>
        public decimal ValuesAvailableCount { get; set; }
        /// <summary>
        /// Gets or sets ValuesTotalCount
        /// </summary>
        public decimal ValuesTotalCount { get; set; }
        /// <summary>
        /// Gets or sets ProgressPercentage
        /// </summary>
        public decimal ProgressPercentage { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasureType
        /// </summary>
        public string BaseMeasureType { get; set; }
    }
    public class CommonModel
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
    }

    public class SaveBaseMeasure
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string UserID { get; set; }
        /// <summary>
        /// Gets or sets BaseMeasures
        /// </summary>
        public List<SearchBaseMeasureParams> lstBaseMeasures { get; set; }

    }


    /// <summary>
    /// save model for base measures
    /// </summary>
    public class SaveMainSpringBaseMeasures {
        /// <summary>
        /// Project Id
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// Frequency id
        /// </summary>
        public int? FrequencyID { get; set; }
        /// <summary>
        /// service id
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string ServiceIDs { get; set; }
        /// <summary>
        /// metric id
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string MetricsIDs { get; set; }
        /// <summary>
        /// report frequency id
        /// </summary>
        public int? ReportFrequencyID { get; set; }
        /// <summary>
        /// List of base measures
        /// </summary>
        public List<MainspringUserDefinedBaseMeasureModel> lstBaseMeasures { get; set; }
        /// <summary>
        /// List of Ticket summary
        /// </summary>
        public  List<MainspringTicketSummaryBaseMeasureModel> lstTicketSummaryBaseODC { get; set; }
        /// <summary>
        /// User Id 
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string UserId { get; set; }
    }

    public class SaveLoadFactor {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// Gets or sets MetricName
        /// </summary>
        [MaxLength(int.MaxValue)] 
        public string MetricName { get; set; }
        /// <summary>
        /// Gets or sets ReportPeriodID
        /// </summary>
        public int ReportPeriodID { get; set; }
        /// <summary>
        /// Gets or sets LoadFactor
        /// </summary>
        public int LoadFactor { get; set; }
    }
}
