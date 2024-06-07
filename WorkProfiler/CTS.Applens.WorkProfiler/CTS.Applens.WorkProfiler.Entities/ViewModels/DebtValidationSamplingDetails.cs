using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    public class DebtValidationSamplingDetails
    {
        public DebtValidation Param { get; set; }


        public List<DebtSamplingGetModel> DebtSamplings { get; set; }
    }

    public class NoiseEliminationDetails 
    {
        public List<NoiseEliminationTicketDescription> NoiseTicketDescriptions { get; set; }
        public List<NoiseEliminationResolutionRemarks> NoiseResolutions { get; set; }
        public DebtValidation Param { get; set; }
    }
}
