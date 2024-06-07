using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyTask = CTS.Applens.WorkProfiler.DAL.MyTaskRepository;
namespace CTS.Applens.WorkProfiler.Repository
{

    public class MyTaskRepository
    {

        /// <summary>
        /// EnableTrustedHosts
        /// </summary>
        public void EnableTrustedHosts()
        {
            try
            {
                new MyTask().EnableTrustedHosts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                new MyTask().ErrorLOG(ErrorMessage, step, ProjectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}