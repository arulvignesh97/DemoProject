using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CTS.Applens.Framework
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
    }
}
