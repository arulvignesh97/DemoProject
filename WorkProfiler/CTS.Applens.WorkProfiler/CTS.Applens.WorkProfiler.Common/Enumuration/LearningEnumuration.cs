using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.LearningWeb.Common.Enumuration
{
    public class LearningEnumuration
    {
        public enum InitialLearningState
        {
            PreRequiste = 1,
            NoiseElimination = 2,
            Sampling = 3,
            RuleExtraction = 4,
        }
        public enum JobType
        {
            Sampling = 1,
            MlRuleExtraction = 2
        }
    }
}
