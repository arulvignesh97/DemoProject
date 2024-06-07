using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AssociateLensCertification = CTS.Applens.WorkProfiler.DAL.AssociateLensCertification;

namespace CTS.Applens.WorkProfiler.Repository
{
    public class AssociateLensCertificationRepository
    {
        public DataTable GetCertificationDetails()
        {
            try
            {
                return new AssociateLensCertification().GetCertificationDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDebtRelatedDetails()
        {
            try
            {
                return new AssociateLensCertification().GetDebtRelatedDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
