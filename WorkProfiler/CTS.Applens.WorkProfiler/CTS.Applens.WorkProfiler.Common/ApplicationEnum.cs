namespace CTS.Applens.WorkProfiler.Common
{
    public class ApplicationEnum
    {
        /// <summary>
        /// ErrorTicketsResult
        /// </summary>
        public enum ErrorTicketsResult
        {
            Success = 1,
            Failure = -1,
            TranslationFailure = -2,
            EncryptionFailure = -3
        }
    }
}
