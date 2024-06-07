using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace CTS.Applens.Framework
{
    [SupportedOSPlatform("windows")]
    public class UserProfile : IUserProfile
    {
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
                user = search.FindOne();
                if (user != null)
                {
                    associate = new Associate
                    {
                        ID = AssociateID
                    };
                    associate = FillAssociateDetails(associate, user);                 
                }
            }
            return associate;
        }
        // fills associate details if user exists
        private static Associate FillAssociateDetails(Associate associate, SearchResult user)
        {
            if (associate != null && user != null)
            {
                associate.Name = user.Properties["givenName"][0] + "," + user.Properties["sn"][0];
                //UserGroups for LogViewer
                List<string> userDLList = new List<string>();
                int propertyCount = user.Properties["memberOf"].Count;
                String dn;
                int equalsIndex, commaIndex;
                for (int propertyCounter = 0; propertyCounter < propertyCount;
                        propertyCounter++)
                {
                    dn = (String)user.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    userDLList.Add(dn.Substring((equalsIndex + 1),
                                   (commaIndex - equalsIndex) - 1));
                }
                associate.UserGroups = userDLList;
                if (user.Properties.Contains("thumbnailphoto"))
                {
                    byte[] bb = (byte[])user.Properties["thumbnailPhoto"][0];
                    associate.UserProfileImageString = Convert.ToBase64String(bb);
                    switch (associate.UserProfileImageString[0])
                    {
                        case '/':
                            {
                                associate.Extension = "jpg";
                                break;
                            }
                        case 'i':
                            {
                                associate.Extension = "png";
                                break;
                            }
                        case 'R':
                            {
                                associate.Extension = "gif";
                                break;
                            }
                        case 'U':
                            {
                                associate.Extension = "webp";
                                break;
                            }
                        default:
                            associate.Extension = "jpg";
                            break;
                    }
                    associate.UserProfileImageString = Constants.dataImage + associate.Extension +
                               Constants.base64 + associate.UserProfileImageString;


                }
                else
                {
                    byte[] noImage = Array.Empty<byte>();
                    associate.UserProfileImageString = Convert.ToBase64String(noImage);
                }
            }
            return associate;
        }
    }
}
