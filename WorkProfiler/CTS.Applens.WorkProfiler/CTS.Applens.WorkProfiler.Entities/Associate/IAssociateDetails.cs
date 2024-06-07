using CTS.Applens.Framework;

namespace CTS.Applens.WorkProfiler.Entities.Associate
{
    public interface IAssociateDetails
    {
        Associate GetCurrentUserDetails(string AssociateID);
    }
}
