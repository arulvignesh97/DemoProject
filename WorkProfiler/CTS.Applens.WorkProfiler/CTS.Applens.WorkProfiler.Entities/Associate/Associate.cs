using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Entities.Associate
{
    public class Associate
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string UserProfileImageString { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string Extension { get; set; }
        [IgnoreDataMember]
        public IList<string> UserGroups { get; set; }
        public IList<long> Customers { get; set; }
        public IList<long> Projects { get; set; }
    }

}
