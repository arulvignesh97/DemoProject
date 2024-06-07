using System;

namespace CTS.Applens.WorkProfiler.Models.ServiceClassification
{
       
        public class ServiceClassificationDictionaryDetails
        {
            public Int64 RuleId { get; set; }

            public Int64 Priority { get; set; }

            public string WorkPattern { get; set; }

            public string CauseCode { get; set; }

            public string ResolutionCode { get; set; }

            public string ServiceName { get; set; }
        }
       
}
