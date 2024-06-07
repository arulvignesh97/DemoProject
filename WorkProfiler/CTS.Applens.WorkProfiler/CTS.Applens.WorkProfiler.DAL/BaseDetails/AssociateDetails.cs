using CTS.Applens.WorkProfiler.Common;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.Framework;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using CTS.Applens.WorkProfiler.Entities.Base;
using System;
using System.Globalization;
using CTS.Applens.WorkProfiler.Entities.Associate;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;
using System.DirectoryServices;

namespace CTS.Applens.WorkProfiler.DAL
{
    /// <summary>
    /// Data Access Layer for Associate Details
    /// </summary>
    public class AssociateDetails : DBContext
    {
        /// <summary>
        /// Get the Current Associate's Base Details
        /// </summary>
        /// <param name="AssociateID"></param>
        /// <returns></returns>
        private readonly AppSettings settings;
        private readonly IGraphAPI GraphAPI;
        private readonly IUserProfile userProfile;
        private readonly IAssociateDetails _associateDetails;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IAssociateDetails associateDetails;

        public AssociateDetails()
        {
        }


        public AssociateDetails(IUserProfile _userProfile, IGraphAPI GraphAPI, IConfiguration configuration, IHttpContextAccessor _httpContextAccessor
           , IWebHostEnvironment hostingEnvironment, IAssociateDetails associateDetails) //: base(_userProfile, GraphAPI,configuration, _httpContextAccessor, hostingEnvironment, associateDetails)
        {
            settings = new AppSettings();
            userProfile = _userProfile;
            this.GraphAPI = GraphAPI;
            _associateDetails = associateDetails;
            this.configuration = configuration;
            httpContextAccessor = _httpContextAccessor;
        }
        public Associate GetCurrentUserDetails(string AssociateID, string associateName, string accessToken, IConfiguration configuration)
        {

            DataSet DS_GetAssociateDetails = new DBHelper().GetDatasetFromSP(
                                            StoredProcedure.AssociateDetails,
                                            new SqlParameter[] { new SqlParameter(nameof(AssociateID), AssociateID) },
                                            ConnectionString);

            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues[KeyCloakConstants.KeyCloakEnabled], CultureInfo.InvariantCulture);
            if (keyCloakEnabled)
            {
                Associate associate = new Associate();
                associate.ID = AssociateID;
                associate.Name = associateName;
                //GraphAPI graph = new GraphAPI();
                //associate.UserProfileImageString = graph.GetProfilePicture(accessToken, configuration);
                return associate;
            }
            else
            {
                Associate associate = new Associate();
                string isLDAP = new AppSettings().AppsSttingsKeyValues["IsLDAPNeeded"];
                if (isLDAP == "true")
                {
                    
                    associate = GetUserProfile(AssociateID, new AppSettings().AppsSttingsKeyValues["LDAPConnection"]);

                }
                else
                {

                    associate.ID = AssociateID;
                    associate.Name = associateName;

                }

                return associate;
            }

        }

        public Associate GetUserProfile(string AssociateID, string UserAccountActiveDirectory)
        {
            Associate associate = null;
            if (!string.IsNullOrEmpty(AssociateID) && !string.IsNullOrEmpty(AssociateID.Trim()))
            {
                DirectoryEntry de = new DirectoryEntry();
                de.Path = UserAccountActiveDirectory;
                DirectorySearcher search = new DirectorySearcher();
                search.Filter = "(SAMAccountName=" + new SanitizeString(AssociateID).Value + ")";
                search.PropertiesToLoad.Add("cn");
                search.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + new SanitizeString(AssociateID).Value + "))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("thumbnailPhoto");
                search.PropertiesToLoad.Add("Company");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("memberOf");
                SearchResult user;
                //user = search.FindOne();

                //if (user != null)
                //{
                //    associate = new Associate
                //    {
                //        ID = AssociateID
                //    };
                //    associate = FillAssociateDetails(associate, user);
                //}
            }
            return associate;
        }

    }
}

