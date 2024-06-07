using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// InitialLearningConstants
    /// </summary>
    public class InitialLearningConstants
    {

        public static readonly string EncryptionEnabledIL = "Enabled";
        public static readonly string DefaultTicketDesc = "TicketDescription";
        public static readonly string DefaultTicketSummary = "TicketSummary";
        public static readonly string MLInputFile = "{0}{1}_Inputfile.csv";
        public static readonly string NoiseInputFile = "{0}{1}_NoiseInput.csv";
        public static readonly string SamplingInputFile = "{0}{1}_Samplingfile.csv";
        public static readonly string NoiseListDescFile = "{0}{1}_Desc_NoiseList.csv";
        public static readonly string NoiseListResFile = "{0}{1}_Res_NoiseList.csv";
        public static readonly string InputDestinationPath = "/{0}/Inputfilepaths.csv";
        public static readonly string MLDetailsChoiceAfterProcess = "AfterProcess";
        public static readonly string JobTypeML = "ML";
        public static readonly string JobTypeSampling = "Sampling";
        public static readonly string MLDetailsChoiceBeforeProcess = "BeforeProcess";
        public static readonly string SaveInitialLearningChoicesNoiseUpd = "NoiseEliminationUpd";
        public static readonly string SaveInitialLearningChoicesSampleUpd = "SamplingUpdation";
        public static readonly string SaveInitialLearningChoicesMLUpd = "MLUpdation";
        public static readonly string SaveInitialLearningChoicesSaveML = "SaveML";
        public static readonly string FileStatusNotFound = "NotFound";
        public static readonly string FileStatusSuccess = "Success";
        public readonly bool FlagTrue = true;
        public readonly bool FlagFalse = false;
        public static readonly string MLOutputFile = "_MLFile";
        public static readonly string MLBaseFile = "_ML_BASE_File";
        public static readonly string MlBaseExcelName = "MLTicketDetails";
        public static readonly string MLDataExtractionFileName = "Ticket_Info";
        public static readonly string AuditChoiceFrom = "From";
        public static readonly string AuditChoiceTo = "To";
        public static readonly string Flag = "Y";
        public static readonly string TypeDebtTickets = "TVP_DEBTTICKETS";
        public static readonly string TypeDebtTicketsInfra = "TVP_DEBTTICKETSInfra";
        public static readonly string TypeMLTicketDescWordList = "TVP_MLTICKETDESCWORDLIST";
        public static readonly string TypeMLOptionalWordList = "TVP_MLOPTIONALWORDLIST";
        public static readonly string TypeSaveDebtSampledTickets = "[AVL].[InfraSaveDebtSampleTickets]";
        public static readonly string TypeDebtMLPattern = "TVP_DEBTMLPATTERN";
        public static readonly string TypeDebtSampledTickets ="TVP_DEBTSAMPLEDTICKETS";
        public static readonly string TypeDebtUploadTickets = "TVP_SAVEDEBTUPLOADTICKETS";
        public static readonly string TypeSaveApprovedPatternValidation = "TVP_SaveApprovedPatternValidation";
        public static readonly string TypeRegenerateApplicationDetails = "TVP_REGENERATEAPPLICATIONDETAIL";
        public static readonly string TypeTicketMasterAuditDetail = "TVP_TicketMasterAuditLog";

    }
}