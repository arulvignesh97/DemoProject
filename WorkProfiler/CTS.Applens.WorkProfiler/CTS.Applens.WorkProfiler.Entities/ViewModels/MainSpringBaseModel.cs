using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    public class BaseMeasureProjectwiseDetails
    {
        public string BaseMeasureSystemDefinedOrUserDefined { get; set; }

        public string BaseMeasureProgressTotalCount { get; set; }

        public string BaseMeasureProgressAvailableCount { get; set; }

        public string ProgressPercentage { get; set; }

        public IList<SearchBaseMeasureParams> MainspringAvailabilityModels { get; set; }
    }

    public class BaseMeasureProjectwiseODCDetails
    {
        public List<MainspringUserDefinedBaseMeasureModel> MainspringAvailabilityModels { get; set; }

        public int TotalCount { get; set; }

        public int CompletedCount { get; set; }

        public int ProgressPercent { get; set; }
    }
}
