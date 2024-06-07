namespace CTS.Applens.WorkProfiler.Common
{
    /// <summary>
    /// Constants for maintaining the Stored Procedures
    /// </summary>
    public static class StoredProcedure  
    {
        public static readonly string    AssociateDetails             =   "[dbo].[GetAssociateDetails]";
        public static readonly string ErrorLog = "[dbo].[AVL_InsertError]";
        public static string SendMailSP = "[AVL].[SendDBEmail]";

        #region Associate
        public static readonly string GetAutomationticketdetails = "[AVL].[GetAutomationAssociateList]";
        public static readonly string GetHealticketdetails = "[AVL].[GetHealticketdetailsList]";
        public static readonly string GetCertificationDetails = "AC.GetCertificationDetails";
        public static readonly string GetDebtUnClassifiedTickets = "[AVL].[GetDebtUnClassifiedTickets]";
        public static readonly string GetDebtUnClassifiedTicketTemplate = "AVL.GetDebtUnClassifiedTicketTemplate";
        public static readonly string UploadDebtUnClassifiedTickets = "AVL.UploadDebtUnClassifiedTickets";
        #endregion

        #region CommonAPI
        public static readonly string GetDebtRelatedDetails = "[AVL].[GetDebtRelatedDetails]";
        #endregion
    }
}
  