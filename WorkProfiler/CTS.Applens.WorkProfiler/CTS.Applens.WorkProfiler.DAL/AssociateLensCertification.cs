using CTS.Applens.WorkProfiler.Entities;
using System.Data;
using Microsoft.Data.SqlClient;
using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Common;

namespace CTS.Applens.WorkProfiler.DAL
{
    public class AssociateLensCertification : DBContext
    {
        public DataTable GetCertificationDetails()
        {
            return new DBHelper()
                .GetDataTableFromSP(StoredProcedure.GetCertificationDetails, ConnectionString);
        }
        public DataTable GetDebtRelatedDetails()
        {
            return new DBHelper()
                .GetDataTableFromSP(StoredProcedure.GetDebtRelatedDetails, ConnectionString);
        }
    }
}
