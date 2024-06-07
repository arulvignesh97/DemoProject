using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Globalization;

namespace CTS.Applens.WorkProfiler.Common
{
    public class AuditLogUtility : DBContext
    {
        /// <summary>
        /// CompareAuditListObjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">This parameter holds self value </param>
        /// <param name="to">This parameter holds to value</param>
        /// <param name="ProjectID">This parameter holds ProjectID value</param>
        /// <param name="modifiedBy">This parameter holds modifiedBy value</param>
        /// <param name="ignore">This parameter holds ignore value</param>
        /// <returns> Datatable Result</returns>
        public DataTable CompareAuditListObjects<T>(List<T> self, List<T> to, string ProjectID,
            string modifiedBy,params string[] ignore) where T : class
        {
           
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add("ProjectID");
            dt.Columns.Add("TicketID");
            dt.Columns.Add("FieldName");
            dt.Columns.Add("FromValue");
            dt.Columns.Add("ToValue");
            dt.Columns.Add("Action");
            dt.Columns.Add("ModifiedBy");
            dt.Columns.Add("ModifiedTimeStamp");

            int count = self.Count();

            for (int i = 0; i < count ; i++)
            {

                if (self[i] != null && to[i] != null)
                {
                    Type type = typeof(T);
                    List<string> ignoreList;
                    if (ignore != null)
                    {
                        ignoreList = new List<string>(ignore);
                    }
                    else
                    {
                        ignoreList = new List<string>();
                    }
                    object TicketID = "";
                    foreach (System.Reflection.PropertyInfo pi in type.GetProperties
                        (System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {

                        if (!ignoreList.Contains(pi.Name))
                        {

                            object fromValue = type.GetProperty(pi.Name).GetValue(self[i], null);
                            object toValue = type.GetProperty(pi.Name).GetValue(to[i], null);
                            if (pi.Name == "TicketID")
                            {
                                TicketID = type.GetProperty(pi.Name).GetValue(to[i], null);
                            }

                            if (fromValue != toValue && (fromValue == null || !fromValue.Equals(toValue)))
                            {
                                dt.Rows.Add(ProjectID, TicketID, pi.Name, fromValue, toValue, "M", modifiedBy,
                                    DateTimeOffset.Now.DateTime);
                            }
                        }
                    }
                }
            }
            
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlParameter[] prms = new SqlParameter[1];               
                prms[0] = new SqlParameter("@AuditDetails", dt);
                prms[0].TypeName = InitialLearningConstants.TypeTicketMasterAuditDetail;
                (new DBHelper()).ExecuteNonQuery("ML_Audit_InsertTicketMasterAuditLog", prms,ConnectionString);
            }

            return dt;
        }

    }
    
}
