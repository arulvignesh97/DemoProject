using System;
using System.Security.Principal;

namespace CTS.Applens.WorkProfiler.Entities
{
    public interface IExcpetionLogging
    {
        void LogException(Exception exception, string userId);
    }
}
