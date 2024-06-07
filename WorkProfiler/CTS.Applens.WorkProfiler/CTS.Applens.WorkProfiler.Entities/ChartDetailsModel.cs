using System;
using System.Collections.Generic;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds ChartDetailsModel details
    /// </summary>
    public class ChartDetailsModel
    {
        /// <summary>
        /// Gets or sets label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets displayvalue
        /// </summary>
        public string Displayvalue { get; set; }
        /// <summary>
        /// Gets or sets WeekDay
        /// </summary>
        public string WeekDay { get; set; }
    }
    /// <summary>
    /// This class holds EffortDetailsByDate details
    /// </summary>
    public class EffortDetailsByDate
    {
        /// <summary>
        /// Gets or sets TimesheetDate
        /// </summary>
        public DateTime TimesheetDate { get; set; }
        /// <summary>
        /// Gets or sets TicketedEffort
        /// </summary>
        public string TicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets NonTicketedEffort
        /// </summary>
        public string NonTicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets Holiday
        /// </summary>
        public string Holiday { get; set; }
        /// <summary>
        /// Gets or sets NoEffort
        /// </summary>
        public string NoEffort { get; set; }
        /// <summary>
        /// Gets or sets PartialEfforts
        /// </summary>
        public string PartialEfforts { get; set; }
        /// <summary>
        /// Gets or sets FullEfforts
        /// </summary>
        public string FullEfforts { get; set; }
    }    
    /// <summary>
    /// This class holds ChartCategories details
    /// </summary>
    public class ChartCategories
    {
        /// <summary>
        /// Gets or sets label
        /// </summary>
        public string Label { get; set; }       

    }
    /// <summary>
    /// This class holds ChartCategoryDataset details
    /// </summary>
    public class ChartCategoryDataset
    {
        /// <summary>
        /// Gets or sets category
        /// </summary>
        public List<ChartCategorieslabel> Category { get; set; }

    }
    /// <summary>
    /// This class holds ChartCategorieslabel details
    /// </summary>
    public class ChartCategorieslabel
    {
        /// <summary>
        /// Gets or sets label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets WeekNo
        /// </summary>
        public int WeekNo { get; set; }
        /// <summary>
        /// Gets or sets WeekDay
        /// </summary>
        public string WeekDay { get; set; }
        /// <summary>
        /// Gets or sets WeekDate
        /// </summary>
        public int WeekDate { get; set; }

    }
    /// <summary>
    /// This class holds ChartDataProd details
    /// </summary>
    public class ChartDataProd
    {
        /// <summary>
        /// Gets or sets DebtMonthTrend
        /// </summary>
        public Dictionary<string, object> DebtMonthTrend { get; set; }
        /// <summary>
        /// Gets or sets TotalMandatoryHours
        /// </summary>
        public decimal TotalMandatoryHours { get; set; }
    }
    /// <summary>
    /// This class holds ChartDatasourceLatent details
    /// </summary>
    public class ChartDatasourceLatent
    {
        /// <summary>
        /// Gets or sets category
        /// </summary>
        public List<ChartCategoryDataset> Category { get; set; }
        /// <summary>
        /// Gets or sets datasets
        /// </summary>
        public List<ChartDataset> Datasets { get; set; }
    }
    /// <summary>
    /// This class holds ChartDataset details
    /// </summary>
    public class ChartDataset
    {
        /// <summary>
        /// Gets or sets label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets TicketedEffort
        /// </summary>
        public string TicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets NonTicketedEffort
        /// </summary>
        public string NonTicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets Holiday
        /// </summary>
        public string Holiday { get; set; }
        /// <summary>
        /// Gets or sets NoEffort
        /// </summary>
        public string NoEffort { get; set; }
        /// <summary>
        /// Gets or sets PartialEfforts
        /// </summary>
        public string PartialEfforts { get; set; }
        /// <summary>
        /// Gets or sets FullEfforts
        /// </summary>
        public string FullEfforts { get; set; }
        /// <summary>
        /// Gets or sets seriesname
        /// </summary>
        public String Seriesname { get; set; }
        /// <summary>
        /// Gets or sets WeekNo
        /// </summary>
        public int WeekNo { get; set; }
        /// <summary>
        /// Gets or sets WeekDay
        /// </summary>
        public string WeekDay { get; set; }
        /// <summary>
        /// Gets or sets WeekDate
        /// </summary>
        public int WeekDate { get; set; }
        /// <summary>
        /// Gets or sets color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Gets or sets anchorbordercolor
        /// </summary>
        public String AnchorBorderColor { get; set; }
        /// <summary>
        /// Gets or sets anchorbgcolor
        /// </summary>
        public String AnchorBackGroundColor { get; set; }
        /// <summary>
        /// Gets or sets data
        /// </summary>
        public List<ChartDetailsModel> Data { get; set; }
    }
    /// <summary>
    /// This class holds LabelValues details
    /// </summary>
    public class LabelValues
    {
        /// <summary>
        /// Gets or sets label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets NonTicketedEffort
        /// </summary>
        public string NonTicketedEffort { get; set; }
        /// <summary>
        /// Gets or sets Holiday
        /// </summary>
        public string Holiday { get; set; }
        /// <summary>
        /// Gets or sets NoEffort
        /// </summary>
        public string NoEffort { get; set; }
        /// <summary>
        /// Gets or sets PartialEfforts
        /// </summary>
        public string PartialEfforts { get; set; }
        /// <summary>
        /// Gets or sets FullEfforts
        /// </summary>
        public string FullEfforts {get;set;}
    }
    }
