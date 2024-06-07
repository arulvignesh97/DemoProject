using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities
{
    [Serializable]
    public class DBInfo
    {
        
        public string ConnectionString { get; set; } = string.Empty;

        public string AssociateID { get; set; }
    }
}
