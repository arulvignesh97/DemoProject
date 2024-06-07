namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// AdminSupportType
    /// </summary>
    public class AdminSupportType
    {
        /// <summary>
        /// SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }
        /// <summary>
        /// AppSuppDisabled
        /// </summary>
        public bool AppSupportDisabled { get; set; }
        /// <summary>
        /// InfraSuppDisabled
        /// </summary>
        public bool InfraSupportDisabled { get; set; }

    }
}