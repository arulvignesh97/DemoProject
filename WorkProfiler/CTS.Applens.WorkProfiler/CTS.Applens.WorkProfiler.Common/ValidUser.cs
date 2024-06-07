using CTS.Applens.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Associate = CTS.Applens.WorkProfiler.Entities.Associate.Associate;

namespace CTS.Applens.WorkProfiler.Common
{
    public static class ValidUser
    {
        public static bool IsValidAccessUser(Associate CurrentUser, string user, long? customer, long? project)
        {
            bool validUser = true; if (user != null && CurrentUser.ID != user)
            {
                validUser = false;
            }
            if (customer != null && validUser != false)
            {
                validUser = CurrentUser.Customers.Where(x => x == customer).Count() > 0 ? true : false;
            }
            if (project != null && validUser != false)
            {
                validUser = CurrentUser.Projects.Where(x => x == project).Count() > 0 ? true : false;
            }
            return validUser;
        }
    }
}
