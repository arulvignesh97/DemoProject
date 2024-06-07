using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Entities;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace CTS.Applens.WorkProfiler.Common
{
    public static class UserProfile
    {
        public static Associate GetUserProfile(string AssociateID, string UserAccountActiveDirectory)
        {
            Associate associate = null;
            if (!string.IsNullOrEmpty(AssociateID.Trim()))
            {
                DirectoryEntry de = new DirectoryEntry();
                de.Path = UserAccountActiveDirectory;
                DirectorySearcher search = new DirectorySearcher();
                search.Filter = "(SAMAccountName=" + AssociateID + ")";
                search.PropertiesToLoad.Add("cn");
                search.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + AssociateID + "))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("thumbnailPhoto");
                search.PropertiesToLoad.Add("Company");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");
                SearchResult user;
                user = search.FindOne();

                if (user != null)
                {
                    associate = new Associate();
                    associate.ID = AssociateID;
                    associate.Name = string.Format("{0},{1}", user.Properties["givenName"][0], user.Properties["sn"][0]);
                    if (user.Properties["thumbnailPhoto"] != null && (user.Properties["thumbnailPhoto"]).Count > 0)
                    {
                        byte[] bb = (byte[])user.Properties["thumbnailPhoto"][0];
                        associate.UserProfileImageString =new StringBuilder().
                            Append("data:image/png;base64,").
                            Append(Convert.ToBase64String(bb)).ToString();
                    }

                }
            }
            return associate;
        }
    }
}
