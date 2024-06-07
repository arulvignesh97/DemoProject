using CTS.Applens.WorkProfiler.Common.Extensions;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.Framework;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace CTS.Applens.WorkProfiler.DAL
{

    public class MyTaskRepository : DBContext
    {
        private static string trustedHosts =new AppSettings().AppsSttingsKeyValues["MyTaskHost"];
        private static string myTaskURL = new AppSettings().AppsSttingsKeyValues["MyTaskURL"];
        
        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None)
                {
                    return true;
                }

                var request = sender as HttpWebRequest;
                if (request != null)
                {
                    return trustedHosts.Contains(request.RequestUri.Host);
                }

                return false;
            };
        }
        
        /// <summary>
        /// ErrorLOG
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <param name="step"></param>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public void ErrorLOG(string ErrorMessage, string step, Int64 ProjectID)
        {
            try
            {
                SqlParameter[] prms = new SqlParameter[4];
                prms[0] = new SqlParameter("@CustomerID", ProjectID);
                prms[1] = new SqlParameter("@ErrSource", step);
                prms[2] = new SqlParameter("@Message", ErrorMessage);
                prms[3] = new SqlParameter("@UserID", "SYSTEM");
                (new DBHelper()).ExecuteNonQuery("AVL_InsertError", prms, ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}