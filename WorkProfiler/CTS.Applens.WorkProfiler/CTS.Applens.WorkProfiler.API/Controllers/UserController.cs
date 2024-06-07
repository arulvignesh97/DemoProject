using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.API.Controllers;
using CTS.Applens.WorkProfiler.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;
namespace CTS.Applens.WorkProfiler.API.Controllers
{
    /// <summary>
    /// Controller to get user related details
    /// </summary>    
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("AzureADAuth")]
    public class UserController : BaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="_httpContextAccessor"></param>
        public UserController(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor, 
            IWebHostEnvironment hostingEnvironment) : base(configuration, _httpContextAccessor, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        ///// <summary>
        ///// This method to get user details
        ///// </summary>
        ///// <param name="UserId"></param>
        ///// <returns>User Details</returns>

        [HttpGet]
         [Route("GetAssociateInfo")]
        public ActionResult<Associate> GetAssociateInfo()
        {
            return this.CurrentUser;
        }

    }
}
