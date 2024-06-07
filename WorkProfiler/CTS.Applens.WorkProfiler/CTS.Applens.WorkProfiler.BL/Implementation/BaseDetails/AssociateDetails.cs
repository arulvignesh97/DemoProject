using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;

namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// Data Access Layer for Associate Details
    /// </summary>
    public class AssociateDetails
    {
        /// <summary>
        /// Get the Current Associate's Base Details
        /// </summary>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        public Associate GetCurrentUserDetails(string AssociateID,string associateName,string accessToken, IConfiguration configuration)
        {
            try
            {
                Associate associate = null;
                var associateCache = new CacheManager();
                var DAL = new DAL.AssociateDetails();

                //----------Redis Cache Code--------------//
                RedisCacheModel objRedisCache = new RedisCacheModel();
                objRedisCache.CacheKey = string.Format(CacheKeys.AssociteDetails, AssociateID,CultureInfo.CurrentCulture);
                objRedisCache.EnabledRedis = Convert.ToBoolean(new Entities.Base.AppSettings().AppsSttingsKeyValues["EnabledRedisCache"], CultureInfo.CurrentCulture);
                associate = associateCache.GetOrCreate<Associate>(objRedisCache,
                    () => DAL.GetCurrentUserDetails(AssociateID, associateName, accessToken,configuration), CacheDuration.Long);
                //----------End Redis Cache Code----------//
                return associate; 
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
